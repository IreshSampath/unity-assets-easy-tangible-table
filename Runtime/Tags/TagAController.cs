using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GAG.EasyTangibleTable
{
    public class TagAController : EasyTangibleTagControllerBase
    {
        [SerializeField] List<Sprite> _states;
        [SerializeField] Image _innerImage;

        float _initialDegree = -1;
        int _currentSector;

        protected override void UpdateVisual()
        {
            base.UpdateVisual(); // includes position + default rotation

            // Rotation is common (optional override)
            if (_isRotatable)
            {
                HandleSectorByRotation(TagData.Degree);
            }

        }

        void HandleSectorByRotation(float degree)
        {
            if (_initialDegree < 0) _initialDegree = degree;

            int sector = 0;
            if (degree > 350 || degree < 90) sector = 1;
            else if (degree >= 90 && degree < 210) sector = 2;
            else if (degree >= 210 && degree <= 350) sector = 3;

            if (sector != _currentSector)
            {
                _currentSector = sector;
                if (sector >= 0 && sector < _states.Count)
                    _innerImage.sprite = _states[sector];
            }
        }

        // Optional: If you want Tag201 to have its own rotation style
        protected override void HandleRotation()
        {
            // e.g., lock rotation to only visual updates, skip base
            _uiTransform.localRotation = Quaternion.identity;
        }
    }
}
