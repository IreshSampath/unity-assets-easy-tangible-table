using GAG.EasyTangibleTable;
using UnityEngine;

// Example tag showing target alignment behaviour
public class TagDController : EasyTangibleTagControllerBase
{
    [SerializeField] private GameObject _msgPanel;
   
    protected override void OnTargetReached()
    {
        if (_msgPanel != null)
            _msgPanel.SetActive(true);
    }
    
    protected override void OnTargetDeparted()
    {
        if (_msgPanel != null)
            _msgPanel.SetActive(false);
    }
}

