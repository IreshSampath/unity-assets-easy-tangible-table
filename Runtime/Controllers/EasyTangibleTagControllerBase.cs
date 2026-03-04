using GAG.EasyTangibleTable;
using UnityEngine;

namespace GAG.EasyTangibleTable
{
    public class EasyTangibleTagControllerBase : MonoBehaviour
    { 
        public EasyTangibleTagModel TagData { get; private set; }

        [Header("References")]
        [SerializeField] protected RectTransform _uiTransform;
        [SerializeField] protected RectTransform _canvas;

        [Header("Settings")]
        [SerializeField] protected bool _isMoveable = true;   // 👈 control this per tag prefab
        [SerializeField] protected bool _isActiveThisTag = true;
        [SerializeField] protected bool _isRotatable = true; // 👈 control this per tag prefab
        [SerializeField] protected bool _isTargetRequired = false;
        //[SerializeField] protected bool _isTargetReachRequired = false;
        [SerializeField] protected GameObject _tagTargetMark;
        [SerializeField, Range(0f, 50f)] float _alignmentThreshold = 10f;
        //[SerializeField] RectTransform _fixedTarget;
        [Header("Offsets & Scale")]
        protected float _xOffset = 0f;
        protected float _yOffset = 0f;
        protected float _zOffset = 0f;
        protected float _xScale = 1f;
        protected float _yScale = 1f;

        private void OnDisable()
        {
            _tagTargetMark.gameObject.SetActive(true);
        }
        public virtual void Initialize(EasyTangibleTagModel model)
        {
            TagData = model;
            UpdateVisual();
        }

        public virtual void UpdateTag(EasyTangibleTagModel model)
        {
            TagData = model;
            UpdateVisual();
        }

        protected virtual void UpdateVisual()
        {
            if (_uiTransform == null) return;

            if(_isActiveThisTag)
            {
                _uiTransform.gameObject.SetActive(true);
            }
            else
            {
                _uiTransform.gameObject.SetActive(false);
                return;
            }
            //if(!_isTargetReachRequired)
            //{
            //_uiTransform.gameObject.SetActive(true);
            //}
            //else
            //{
            if (_isMoveable)
                {
                    HandlePosition();

                    if (_isTargetRequired)
                    {
                    //_isTargetRequired = false;
                    // _uiTransform.gameObject.SetActive(false);
                    //_isActiveThisTag = false;
                    HandleFixedTarget();
                    }
                    else
                    {
                        _tagTargetMark.gameObject.SetActive(false);
                    }


                }

            //}



            // Rotation is common (optional override)
            if (_isRotatable)
            {
                HandleRotation();
            }
        }

        protected virtual void HandlePosition()
        {
            if (_canvas == null) return;

            RectTransform canvasRect = _canvas.GetComponent<RectTransform>();

            _xOffset = PlayerPrefs.GetFloat("XOffset", _xOffset);
            _yOffset = PlayerPrefs.GetFloat("YOffset", _yOffset);

            float expectedX = TagData.XPos * canvasRect.rect.width - canvasRect.rect.width * 0.5f;
            float expectedY = TagData.YPos * canvasRect.rect.height - canvasRect.rect.height * 0.5f;

            Vector2 expectedPos = new Vector2(expectedX + _xOffset, expectedY + _yOffset);
            _uiTransform.anchoredPosition = expectedPos;
        }

        protected virtual void HandleRotation()
        {
            // You can override this in derived classes if needed
            _uiTransform.localRotation = Quaternion.Euler(0, 0, -TagData.Degree);
        }

        protected virtual void HandleFixedTarget()
        {
            if (_tagTargetMark == null || _uiTransform == null) return;

            // Measure distance between UI and target
            float distance = Vector3.Distance(_uiTransform.position, _tagTargetMark.transform.position);

            // 👇 Compare using threshold (instead of exact equality)
            //bool isAligned = distance <= _alignmentThreshold;

            if(distance <= _alignmentThreshold)
            {
                _tagTargetMark.gameObject.SetActive(false);
                //_uiTransform.gameObject.SetActive(true);
                _isActiveThisTag = true;
            }

            //if (_tagTargetMark == null) return;
            //// Logic to align with fixed target
            //if(_uiTransform.position == _tagTargetMark.transform.position) 
            //{
            //    _tagTargetMark.gameObject.SetActive(false);
            //}
            //else
            //{
            //    _tagTargetMark.gameObject.SetActive(true);
            //}
        }
    }
}
