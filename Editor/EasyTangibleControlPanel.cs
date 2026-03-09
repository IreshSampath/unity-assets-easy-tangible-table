#if UNITY_EDITOR
using System.Linq;
using System.Collections.Generic;
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
        // ----------------------------------------------------
        // PACKAGE
        // ----------------------------------------------------

        const string PACKAGE_NAME = "com.ireshsampath.unity-assets.easy-tangible-table";

        // ----------------------------------------------------
        // EASY UI CONSOLE
        // ----------------------------------------------------

        const string EASY_UICONSOLE_PACKAGE = "com.ireshsampath.unity-assets.easy-ui-console";
        const string EASY_UICONSOLE_REPO = "https://github.com/IreshSampath/unity-assets-easy-ui-console.git";
        const string EASY_UICONSOLE_DEFINE = "EASY_UICONSOLE";

        AddRequest _installRequest;

        // ----------------------------------------------------
        // PREFABS
        // ----------------------------------------------------

        List<string> _prefabPaths = new();
        string[] _prefabOptions = new string[0];
        int _selectedPrefab;

        // ----------------------------------------------------
        // MENU
        // ----------------------------------------------------

        [MenuItem("Tools/GAG/EasyTangibleTable")]
        public static void Open()
        {
            GetWindow<EasyTangibleTableControlPanel>("EasyTangibleTable");
        }

        // ----------------------------------------------------
        // GUI
        // ----------------------------------------------------

        void OnGUI()
        {
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Control Panel", EditorStyles.boldLabel);

            EditorGUILayout.Space();

            DrawEasyTangibleTableSetup();

            EditorGUILayout.Space(10);

            DrawEasyUIConsoleIntegration();
        }

        // ----------------------------------------------------
        // EASY TANGIBLE TABLE SETUP
        // ----------------------------------------------------

        void RefreshPrefabList()
        {
            _prefabPaths.Clear();

            string[] guids = AssetDatabase.FindAssets("t:Prefab EasyTangibleTable");

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (path.Contains("Samples"))
                {
                    _prefabPaths.Add(path);
                }
            }

            _prefabOptions = _prefabPaths
                .Select(p => System.IO.Path.GetFileNameWithoutExtension(p))
                .ToArray();

        }

        void DrawPrefabSelector()
        {
            if (_prefabOptions.Length == 0)
                RefreshPrefabList();

            if (_prefabOptions.Length == 0)
            {
                EditorGUILayout.HelpBox(
                    "No prefabs found. Import EasyTangibleTable samples first.",
                    MessageType.Warning);
                return;
            }

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Prefab", GUILayout.Width(70));

            _selectedPrefab = EditorGUILayout.Popup(
                _selectedPrefab,
                _prefabOptions
            );

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Add Prefab", GUILayout.Height(30)))
            {
                AddSelectedPrefab();
            }
        }
        
        void DrawEasyTangibleTableSetup()
        {
            EditorGUILayout.LabelField("EasyTangibleTable Setup", EditorStyles.boldLabel);

            EditorGUILayout.Space(4);

            if (GUILayout.Button("Import Sample", GUILayout.Height(30)))
            {
                ImportEasyTangibleTableSample();
            }

            EditorGUILayout.Space(8);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Prefab", GUILayout.Width(70));

            _selectedPrefab = EditorGUILayout.Popup(
                _selectedPrefab,
                _prefabOptions
            );

            EditorGUILayout.EndHorizontal();

            if (GUILayout.Button("Add Prefab", GUILayout.Height(30)))
            {
                AddSelectedPrefab();
            }
        }

        void AddSelectedPrefab()
        {
            if (_prefabPaths.Count == 0)
                return;

            string prefabPath = _prefabPaths[_selectedPrefab];

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (prefab == null)
            {
                Debug.LogWarning("Prefab not found.");
                return;
            }

            PrefabUtility.InstantiatePrefab(prefab);

            Debug.Log($"[EasyTangibleTable] Added prefab: {prefab.name}");
        }
        
        void ImportEasyTangibleTableSample()
        {
            var pkg = UnityEditor.PackageManager.PackageInfo
                .GetAllRegisteredPackages()
                .FirstOrDefault(p => p.name == PACKAGE_NAME);

            if (pkg == null)
            {
                Debug.LogWarning("[EasyTangibleTable] Package not installed.");
                return;
            }

            var samples = Sample.FindByPackage(pkg.name, pkg.version)?.ToList();

            if (samples == null || samples.Count == 0)
            {
                Debug.LogWarning("[EasyTangibleTable] No samples found.");
                return;
            }

            samples[0].Import(Sample.ImportOptions.OverridePreviousImports);

            Debug.Log("[EasyTangibleTable] Sample imported.");
        }
        

        // ----------------------------------------------------
        // EASY UI CONSOLE INTEGRATION
        // ----------------------------------------------------

        void DrawEasyUIConsoleIntegration()
        {
            EditorGUILayout.LabelField("EasyUIConsole Integration", EditorStyles.boldLabel);

            bool installed = IsEasyUIConsoleInstalled();
            bool defineEnabled = HasDefine(EASY_UICONSOLE_DEFINE);

            EditorGUILayout.HelpBox(
                $"Installed: {(installed ? "YES" : "NO")}\nDefine ({EASY_UICONSOLE_DEFINE}): {(defineEnabled ? "ENABLED" : "DISABLED")}",
                installed ? MessageType.Info : MessageType.Warning);

            EditorGUILayout.BeginHorizontal();

            using (new EditorGUI.DisabledScope(_installRequest != null))
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

            if (GUILayout.Button("Enable Define"))
                AddDefine(EASY_UICONSOLE_DEFINE);

            if (GUILayout.Button("Disable Define"))
                RemoveDefine(EASY_UICONSOLE_DEFINE);

            EditorGUILayout.EndHorizontal();

            if (_installRequest != null)
                EditorGUILayout.HelpBox("Installing EasyUIConsole... Please wait.", MessageType.Info);
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
            if (_installRequest == null || !_installRequest.IsCompleted)
                return;

            EditorApplication.update -= InstallProgress;

            if (_installRequest.Status == StatusCode.Success)
            {
                Debug.Log("[EasyTangibleTable] EasyUIConsole installed.");
                AddDefine(EASY_UICONSOLE_DEFINE);
                Repaint();
            }
            else
            {
                Debug.LogError("[EasyTangibleTable] Install failed: " + _installRequest.Error.message);
            }

            _installRequest = null;
        }

        void ImportEasyUIConsoleSample()
        {
            var pkg = UnityEditor.PackageManager.PackageInfo
                .GetAllRegisteredPackages()
                .FirstOrDefault(p => p.name == EASY_UICONSOLE_PACKAGE);

            if (pkg == null)
            {
                Debug.LogWarning("[EasyTangibleTable] EasyUIConsole not installed.");
                return;
            }

            var samples = Sample.FindByPackage(pkg.name, pkg.version)?.ToList();

            if (samples == null || samples.Count == 0)
            {
                Debug.LogWarning("[EasyTangibleTable] No samples found.");
                return;
            }

            samples[0].Import(Sample.ImportOptions.OverridePreviousImports);
        }

        // ----------------------------------------------------
        // DEFINE HELPERS
        // ----------------------------------------------------

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