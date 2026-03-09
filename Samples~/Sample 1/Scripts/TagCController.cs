using GAG.EasyTangibleTable;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Example tag showing how rotation can change colors
public class TagCController : EasyTangibleTagControllerBase
{
    [SerializeField] List<Image> _images;

    float _initialDegree = -1;
    int _currentSector = 0;

    private void OnDisable()
    {
        _initialDegree = -1;
    }

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
                ChangeColor();
            //if (sector == 0 && sector < _images.Count)
            //    _images[sector].color = Color.green;
        }
    }

    void ChangeColor()
    {
        for (int i = 0; i < _images.Count; i++)
        {
            if (i == _currentSector-1)
                _images[i].color = Color.green;
            else
                _images[i].color = Color.white;
        }
    }
    protected override void HandleRotation()
    {
        // e.g., lock rotation to only visual updates, skip base
        _tagUiTransform.localRotation = Quaternion.identity;
    }
}

