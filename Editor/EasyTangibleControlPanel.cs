#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager.UI;
using UnityEngine;

namespace GAG.EasyTangibleTable.Editor
{
    public class EasyTangibleTableControlPanel : EditorWindow
    {
        const string EASY_UICONSOLE_PACKAGE = "com.ireshsampath.unity-assets.easy-ui-console";
        const string EASY_UICONSOLE_REPO = "https://github.com/IreshSampath/unity-assets-easy-ui-console.git";
        const string EASY_UICONSOLE_DEFINE = "EASY_UICONSOLE";

        AddRequest _installRequest;
        bool _sampleImported;
        string[] _consolePrefabs;
        int _selectedPrefabIndex;
        
        void OnEnable()
        {
            LoadConsolePrefabs();
        }
        
        [MenuItem("Tools/GAG/EasyTangibleTable")]
        public static void Open()
        {
            GetWindow<EasyTangibleTableControlPanel>("EasyTangibleTable");
        }

        void OnGUI()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Control Panel", EditorStyles.boldLabel);

            EditorGUILayout.Space();
            DrawEasyUIConsoleIntegration();
        }

        // ------------------------------------------------------
        // EASY UI CONSOLE INTEGRATION
        // ------------------------------------------------------

        void DrawEasyUIConsoleIntegration()
        {
            EditorGUILayout.LabelField("EasyUIConsole Integration", EditorStyles.boldLabel);

            bool installed = IsEasyUIConsoleInstalled();
            bool defineEnabled = HasDefine(EASY_UICONSOLE_DEFINE);

            // EditorGUILayout.HelpBox(
            //     $"Installed: {(installed ? "YES" : "NO")}\nDefine ({EASY_UICONSOLE_DEFINE}): {(defineEnabled ? "ENABLED" : "DISABLED")}",
            //     installed ? MessageType.Info : MessageType.Warning);

            if (!installed)
            {
                EditorGUILayout.HelpBox(
                    "EasyUIConsole is not installed.",
                    MessageType.Warning);
            }
            else if (_sampleImported)
            {
                EditorGUILayout.HelpBox(
                    "EasyUIConsole installed successfully.\nEasyUIConsole sample imported successfully.",
                    MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox(
                    "EasyUIConsole installed successfully.\nYou can now import the sample.",
                    MessageType.Info);
            }

            
            EditorGUILayout.BeginHorizontal();

            using (new EditorGUI.DisabledScope(installed || _installRequest != null))
            {
                if (GUILayout.Button("Install EasyUIConsole", GUILayout.Height(30)))
                    InstallEasyUIConsole();
            }

            using (new EditorGUI.DisabledScope(!installed))
            {
                if (GUILayout.Button("Import Sample", GUILayout.Height(30)))
                    ImportEasyUIConsoleSample();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            using (new EditorGUI.DisabledScope(!installed || defineEnabled))
            {
                if (GUILayout.Button("Enable Define"))
                    AddDefine(EASY_UICONSOLE_DEFINE);
            }

            using (new EditorGUI.DisabledScope(!installed || !defineEnabled))
            {
                if (GUILayout.Button("Disable Define"))
                    RemoveDefine(EASY_UICONSOLE_DEFINE);
            }

            EditorGUILayout.EndHorizontal();

            if (_installRequest != null && !_installRequest.IsCompleted)
            {
                EditorGUILayout.HelpBox("Installing EasyUIConsole... Please wait.", MessageType.Info);
            }
            EditorGUILayout.Space();
            
            if (_consolePrefabs != null && _consolePrefabs.Length > 0)
            {
                _selectedPrefabIndex = EditorGUILayout.Popup(
                    "UIConsole Prefab",
                    _selectedPrefabIndex,
                    _consolePrefabs);
            }
            
            EditorGUILayout.Space();

            using (new EditorGUI.DisabledScope(!installed))
            {
                if (GUILayout.Button("Add UIConsole Prefab", GUILayout.Height(30)))
                    AddUIConsolePrefabToScene();
            }
        }

        bool IsEasyUIConsoleInstalled()
        {
            return UnityEditor.PackageManager.PackageInfo
                .GetAllRegisteredPackages()
                .Any(p => p.name == EASY_UICONSOLE_PACKAGE);
        }

        void InstallEasyUIConsole()
        {
            _installRequest = Client.Add(EASY_UICONSOLE_REPO);
            EditorApplication.update += InstallProgress;
        }

        void InstallProgress()
        {
            if (_installRequest == null)
                return;

            if (!_installRequest.IsCompleted)
                return;

            EditorApplication.update -= InstallProgress;

            if (_installRequest.Status == StatusCode.Success)
            {
                Debug.Log("[EasyTT] EasyUIConsole installed.");

                AddDefine(EASY_UICONSOLE_DEFINE);

                _installRequest = null;

                Repaint();
            }
            else
            {
                Debug.LogError("[EasyTT] Install failed: " + _installRequest.Error.message);
                _installRequest = null;
            }
        }

        void ImportEasyUIConsoleSample()
        {
            var pkg = UnityEditor.PackageManager.PackageInfo
                .GetAllRegisteredPackages()
                .FirstOrDefault(p => p.name == EASY_UICONSOLE_PACKAGE);

            if (pkg == null)
            {
                Debug.LogWarning("[EasyTT] EasyUIConsole not installed.");
                return;
            }

            var samples = Sample.FindByPackage(pkg.name, pkg.version)?.ToList();

            if (samples == null || samples.Count == 0)
            {
                Debug.LogWarning("[EasyTT] No samples found.");
                return;
            }

            samples[0].Import(Sample.ImportOptions.OverridePreviousImports);

            _sampleImported = true;
            Repaint();
        }

        void AddUIConsolePrefabToScene()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab EasyUIConsole");

            if (guids.Length == 0)
            {
                Debug.LogWarning("[EasyTT] No EasyUIConsole prefabs found.");
                return;
            }

            var path = AssetDatabase.GUIDToAssetPath(guids[_selectedPrefabIndex]);
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;

            Undo.RegisterCreatedObjectUndo(instance, "Add EasyUIConsole");

            Selection.activeGameObject = instance;
            
            // var prefab = AssetDatabase
            //     .FindAssets("t:Prefab EasyUIConsole")
            //     .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
            //     .Select(path => AssetDatabase.LoadAssetAtPath<GameObject>(path))
            //     .FirstOrDefault();
            //
            // if (prefab == null)
            // {
            //     Debug.LogWarning("[EasyTT] EasyUIConsole prefab not found.");
            //     return;
            // }
            //
            // var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            //
            // if (instance != null)
            // {
            //     Undo.RegisterCreatedObjectUndo(instance, "Add EasyUIConsole");
            //     Selection.activeGameObject = instance;
            //
            //     Debug.Log("[EasyTT] EasyUIConsole prefab added to scene.");
            // }
        }
        
        void LoadConsolePrefabs()
        {
            var guids = AssetDatabase.FindAssets("t:Prefab EasyUIConsole");

            _consolePrefabs = guids
                .Select(g => AssetDatabase.GUIDToAssetPath(g))
                .Select(p => System.IO.Path.GetFileNameWithoutExtension(p))
                .ToArray();
        }
        
        // ------------------------------------------------------
        // DEFINE HELPERS
        // ------------------------------------------------------

        bool HasDefine(string symbol)
        {
            var group = EditorUserBuildSettings.selectedBuildTargetGroup;
            var named = NamedBuildTarget.FromBuildTargetGroup(group);

            var defines = PlayerSettings.GetScriptingDefineSymbols(named)
                .Split(';')
                .Select(d => d.Trim())
                .Where(d => !string.IsNullOrEmpty(d))
                .ToList();

            return defines.Contains(symbol);
        }

        void AddDefine(string symbol)
        {
            var group = EditorUserBuildSettings.selectedBuildTargetGroup;
            var named = NamedBuildTarget.FromBuildTargetGroup(group);

            var defines = PlayerSettings.GetScriptingDefineSymbols(named)
                .Split(';')
                .Select(d => d.Trim())
                .Where(d => !string.IsNullOrEmpty(d))
                .ToList();

            if (!defines.Contains(symbol))
                defines.Add(symbol);

            PlayerSettings.SetScriptingDefineSymbols(named, string.Join(";", defines));
        }

        void RemoveDefine(string symbol)
        {
            var group = EditorUserBuildSettings.selectedBuildTargetGroup;
            var named = NamedBuildTarget.FromBuildTargetGroup(group);

            var defines = PlayerSettings.GetScriptingDefineSymbols(named)
                .Split(';')
                .Select(d => d.Trim())
                .Where(d => !string.IsNullOrEmpty(d))
                .ToList();

            defines.RemoveAll(d => d == symbol);

            PlayerSettings.SetScriptingDefineSymbols(named, string.Join(";", defines));
        }
    }
}
#endif