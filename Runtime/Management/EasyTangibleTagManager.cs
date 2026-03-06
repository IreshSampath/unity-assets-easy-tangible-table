using System.Collections.Generic;
using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public class EasyTangibleTagManager : MonoBehaviour
    {
    [System.Serializable]
    public class TagPrefabBinding
    {
        public int tagID;
        public EasyTangibleTagControllerBase prefab;       // For future use (instantiation)
        public EasyTangibleTagControllerBase existingTag;  // For pre-placed tags
    }

    [SerializeField] RectTransform _canvas;
    [SerializeField] List<TagPrefabBinding> _tagPrefabs;

    readonly Dictionary<int, EasyTangibleTagControllerBase> _activeTags = new();

    void OnEnable()
    {
        EasyTangibleTagEvents.TagPlaced += OnTagPlaced;
        EasyTangibleTagEvents.TagAlived += OnTagAlived;
    }

    void OnDisable()
    {
        EasyTangibleTagEvents.TagPlaced -= OnTagPlaced;
        EasyTangibleTagEvents.TagAlived -= OnTagAlived;
    }

    private void Start()
    {
        foreach (var tag in _tagPrefabs)
        {
            tag.existingTag?.gameObject.SetActive(false);
        }
    }
    void OnTagPlaced(EasyTangibleTagModel tag)
    {
        if (_activeTags.TryGetValue(tag.FiducialID, out var controller))
        {
            controller.UpdateTag(tag);
            return;
        }

        var binding = _tagPrefabs.Find(b => b.tagID == tag.FiducialID);
        if (binding == null)
        {
            Debug.LogWarning($"⚠️ No tag binding found for ID: {tag.FiducialID}");
            return;
        }

        print($"Creating/Updating tag ID: {tag.FiducialID}");

        // 👇 Prefer pre-created tag object if assigned
        if (binding.existingTag != null)
        {
            controller = binding.existingTag;
            controller.Initialize(tag);
            controller.gameObject.SetActive(true);
            _activeTags[tag.FiducialID] = controller;
        }
        else if (binding.prefab != null)
        {
            // fallback: instantiate prefab
            var instance = Instantiate(binding.prefab, _canvas);
            controller = instance.GetComponent<EasyTangibleTagControllerBase>();
            controller.Initialize(tag);
            controller.gameObject.SetActive(true);
            _activeTags[tag.FiducialID] = controller;
        }
        else
        {
            Debug.LogWarning($"⚠️ Tag {tag.FiducialID} has no prefab or existingTag assigned.");
        }
    }

    void OnTagAlived(List<int> tagIds)
    {
        if (tagIds == null || tagIds.Count == 0)
        {
            foreach (var tag in _tagPrefabs)
            {
                tag.existingTag?.gameObject.SetActive(false);
            }
            //_tagTargetMark.SetActive(true);

            //if (_languageIndex == 0)
            //{
            //    _tagGraphics[0].transform.GetChild(0).GetComponent<Image>().sprite = _tag201ActivatedUis[0];
            //}
            //else
            //{
            //    _tagGraphics[0].transform.GetChild(0).GetComponent<Image>().sprite = _tag201ActivatedUis[4];
            //}
            //_activeTagId = 0; // Reset active tag ID
            //_isTagDetected = false;
            return;
        }

        var aliveSet = new HashSet<int>(tagIds);

        foreach (var tag in _tagPrefabs)
        {
            if (int.TryParse(tag.tagID.ToString(), out int tagId))
            {
                //tag.SetActive(aliveSet.Contains(tagId));

                //    if (!aliveSet.Contains(tagId))
                //    {
                //        if (tagId == 201)
                //        {
                //            _tag1InitialDegrees = -1;
                //        }
                //        else if (tagId == 202)
                //        {
                //            _tag2InitialDegrees = -1;
                //        }
                //        else if (tagId == 203)
                //        {
                //            _tag3InitialDegrees = -1;
                //        }
                //        else if (tagId == 204)
                //        {
                //            _tag4InitialDegrees = -1;
                //        }
                //        else if (tagId == 205)
                //        {
                //            _tag5InitialDegrees = -1;
                //        }
                //        else if (tagId == 206)
                //        {
                //            _tag6InitialDegrees = -1;
                //        }
                //    }
            }
            else
            {
                Debug.LogWarning($"Tag name '{tag.existingTag.gameObject.name}' is not a valid integer.");
                tag.existingTag.gameObject.SetActive(false);
            }
        }
    }
    }
}
