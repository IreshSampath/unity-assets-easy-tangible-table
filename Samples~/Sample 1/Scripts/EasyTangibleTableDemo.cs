using GAG.EasyTangibleTable;
using System.Collections.Generic;
using UnityEngine;

public class EasyTangibleTableDemo : MonoBehaviour
{
    void OnEnable()
    {
        EasyTT.ActiveTagsUpdated += HandleActiveTags;
        EasyTT.TagAligned += HandleTagAligned;
        EasyTT.TagAlignmentLost += HandleTagLost;
        EasyTT.TagRemoved += HandleTagRemoved;

        EasyTT.TagUpdated += HandleTagUpdated;
        EasyTT.TagMoved += HandleTagMoved;
        EasyTT.TagRotated += HandleTagRotated;
    }

    void OnDisable()
    {
        EasyTT.ActiveTagsUpdated -= HandleActiveTags;
        EasyTT.TagAligned -= HandleTagAligned;
        EasyTT.TagAlignmentLost -= HandleTagLost;
        EasyTT.TagRemoved -= HandleTagRemoved;

        EasyTT.TagUpdated -= HandleTagUpdated;
        EasyTT.TagMoved -= HandleTagMoved;
        EasyTT.TagRotated -= HandleTagRotated;
    }

    void HandleActiveTags(IReadOnlyList<int> tags)
    {
        foreach (var tag in tags)
            Debug.Log($"Active Tag: {tag}");
    }

    void HandleTagAligned(int tagID)
    {
        Debug.Log($"Tag aligned: {tagID}");
    }

    void HandleTagLost(int tagID)
    {
        Debug.Log($"Tag alignment lost: {tagID}");
    }

    void HandleTagRemoved(int tagID)
    {
        Debug.Log($"Tag removed: {tagID}");
    }
    
    void HandleTagUpdated(EasyTangibleTagModel tag)
    {
        Debug.Log($"Tag updated: {tag.FiducialID} Pos({tag.XPos},{tag.YPos}) Rot({tag.Degree})");
    }

    void HandleTagMoved(int tagID, Vector2 position)
    {
        Debug.Log($"Tag moved: {tagID} → {position}");
    }

    void HandleTagRotated(int tagID, float rotation)
    {
        Debug.Log($"Tag rotated: {tagID} → {rotation}");
    }
    
    public void OpenConsole()
    {
        EasyTT.OpenConsole();
    }
}
