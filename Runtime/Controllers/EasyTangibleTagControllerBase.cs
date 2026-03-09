using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public class EasyTangibleTagControllerBase : MonoBehaviour
    {
        public EasyTangibleTagModel TagData { get; private set; }

        [Header("References")]
        [SerializeField] protected RectTransform _tagUiTransform;
        [SerializeField] protected RectTransform _canvas;

        [Header("Settings")]
        [SerializeField] protected bool _isActiveThisTag = true;
        [SerializeField] protected bool _isMoveable = true;
        [SerializeField] protected bool _isRotatable = true;
        [SerializeField] protected bool _isTargetRequired = false;

        [SerializeField] bool _tagMarkAccessible = true;
        [SerializeField] protected GameObject _tagTargetMark;
        [SerializeField, Range(0f, 50f)] float _targetAlignmentThreshold = 10f;

        RectTransform _canvasRect;
        bool _isInitialized;
        bool _isAligned;

        protected float _xOffset;
        protected float _yOffset;

        void OnDisable()
        {
            if (_tagMarkAccessible && _tagTargetMark != null)
                _tagTargetMark.SetActive(true);
        }

        public virtual void Initialize(EasyTangibleTagModel model)
        {
            TagData = model;
            _canvasRect = _canvas;
            _isInitialized = true;

            UpdateVisual();
        }

        public virtual void UpdateTag(EasyTangibleTagModel model)
        {
            TagData = model;
            UpdateVisual();
        }

        protected virtual void UpdateVisual()
        {
            if (!_isInitialized || _tagUiTransform == null)
                return;

            _tagUiTransform.gameObject.SetActive(_isActiveThisTag);

            if (!_isActiveThisTag)
                return;

            if (_isMoveable)
                HandlePosition();

            if (_isRotatable)
                HandleRotation();

            if (_isTargetRequired)
                HandleTargetLogic();
        }

        protected virtual void HandlePosition()
        {
            _xOffset = PlayerPrefs.GetFloat("XOffset", _xOffset);
            _yOffset = PlayerPrefs.GetFloat("YOffset", _yOffset);

            float x = TagData.XPos * _canvasRect.rect.width - _canvasRect.rect.width * 0.5f;
            float y = TagData.YPos * _canvasRect.rect.height - _canvasRect.rect.height * 0.5f;

            _tagUiTransform.anchoredPosition = new Vector2(x + _xOffset, y + _yOffset);
        }

        protected virtual void HandleRotation()
        {
            _tagUiTransform.localRotation = Quaternion.Euler(0, 0, -TagData.Degree);
        }

        protected virtual void HandleTargetLogic()
        {
            if (!_tagMarkAccessible || _tagTargetMark == null)
                return;

            float distance = Vector3.Distance(
                _tagUiTransform.position,
                _tagTargetMark.transform.position
            );

            bool alignedNow = distance <= _targetAlignmentThreshold;

            if (alignedNow && !_isAligned)
            {
                _isAligned = true;
                _tagTargetMark.SetActive(false);

                EasyTangibleTagEvents.RaiseTagAligned(TagData.FiducialID);
                OnTargetReached();
            }
            else if (!alignedNow && _isAligned)
            {
                _isAligned = false;
                _tagTargetMark.SetActive(true);

                EasyTangibleTagEvents.RaiseTagAlignmentLost(TagData.FiducialID);
                OnTargetDeparted();
            }
        }

        protected virtual void OnTargetReached() { }

        protected virtual void OnTargetDeparted() { }
    }
}