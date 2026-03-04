using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GAG.EasyTangibleTable
{
    public class MultiTapHandler : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnMultiTapped;

        public float TapResetTime => _tapResetTime;
        [SerializeField] float _tapResetTime = 0.7f; // seconds allowed between taps

        public int RequierdTapCount => _requiredTapCount;
        [SerializeField] int _requiredTapCount = 3;

        float _lastTapTime = 0f;
        int _tapCount = 0;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Time.time - _lastTapTime > _tapResetTime)
                _tapCount = 0; // reset if too slow

            _lastTapTime = Time.time;
            _tapCount++;

            if (_tapCount >= RequierdTapCount)
            {
                _tapCount = 0; // reset immediately after trigger
                OnMultiTapped?.Invoke();
            }
        }
    }
}

