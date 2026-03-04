using UnityEngine;
using UnityEngine.Serialization;
using GAG.EasyTangibleTable;

public class EasyTangibleTableHandler : MonoBehaviour
{
     [SerializeField] MultiTapHandler _hiddenSettingsBtn;
     [SerializeField] GameObject _settingsPanel;
    
     void OnEnable()
     {
         if (_hiddenSettingsBtn != null)
             _hiddenSettingsBtn.OnMultiTapped += ToggleSettingsPanel;
     }

     void OnDisable()
     {
         if (_hiddenSettingsBtn != null)
             _hiddenSettingsBtn.OnMultiTapped -= ToggleSettingsPanel;
     }
    
    void ToggleSettingsPanel()
    {
        if (_settingsPanel == null) return;

        _settingsPanel.SetActive(!_settingsPanel.activeSelf);
    }
    
    public void OpenConsole()
    {
        EasyTT.OpenConsole();
    }
}

