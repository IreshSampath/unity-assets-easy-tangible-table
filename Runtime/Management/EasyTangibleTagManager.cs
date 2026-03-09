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
            public EasyTangibleTagControllerBase prefab;
            public EasyTangibleTagControllerBase existingTag;
        }

        [SerializeField] RectTransform _canvas;
        [SerializeField] List<TagPrefabBinding> _tagPrefabs;
        readonly Dictionary<int, TagPrefabBinding> _tagLookup = new();
        readonly Dictionary<int, EasyTangibleTagControllerBase> _activeTags = new();

        void Awake()
        {
            foreach (var binding in _tagPrefabs)
            {
                if (!_tagLookup.ContainsKey(binding.tagID))
                    _tagLookup.Add(binding.tagID, binding);
                else
                    Debug.LogWarning($"Duplicate Tag ID detected: {binding.tagID}");
            }
        }
        
        void OnEnable()
        {
            EasyTangibleTagEvents.TagPlaced += OnTagPlaced;
            EasyTangibleTagEvents.ActiveTagsUpdated += OnActiveTagsUpdated;
        }

        void OnDisable()
        {
            EasyTangibleTagEvents.TagPlaced -= OnTagPlaced;
            EasyTangibleTagEvents.ActiveTagsUpdated -= OnActiveTagsUpdated;
        }

        void Start()
        {
            foreach (var tag in _tagPrefabs)
                tag.existingTag?.gameObject.SetActive(false);
        }

        void OnTagPlaced(EasyTangibleTagModel tag)
        {
            if (_activeTags.TryGetValue(tag.FiducialID, out var controller))
            {
                controller.UpdateTag(tag);
                return;
            }

            if (!_tagLookup.TryGetValue(tag.FiducialID, out var binding))
            {
                Debug.LogWarning($"No tag binding found for ID: {tag.FiducialID}");
                return;
            }

            controller = binding.existingTag;

            if (controller == null && binding.prefab != null)
                controller = Instantiate(binding.prefab, _canvas);

            if (controller == null)
            {
                Debug.LogWarning($"Tag {tag.FiducialID} has no prefab or existingTag assigned.");
                return;
            }

            controller.Initialize(tag);
            controller.gameObject.SetActive(true);

            _activeTags[tag.FiducialID] = controller;
        }

        void OnActiveTagsUpdated(IReadOnlyList<int> tagIds)
        {
            var aliveSet = new HashSet<int>(tagIds);

            foreach (var tag in _tagPrefabs)
                tag.existingTag?.gameObject.SetActive(aliveSet.Contains(tag.tagID));
        }
    }
}