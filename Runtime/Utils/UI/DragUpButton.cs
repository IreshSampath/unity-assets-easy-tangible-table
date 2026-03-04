using UnityEngine;
using UnityEngine.EventSystems;

namespace GAG.EasyTangibleTable
{
    public class DragUpButton : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Vector2 startPosition;
        private RectTransform rectTransform;

        public float triggerDistance = 50f; // How far up to trigger
        private bool triggered = false;
        [SerializeField] int _buttonId;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPosition = rectTransform.anchoredPosition;
            triggered = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta;

            float distanceY = rectTransform.anchoredPosition.y - startPosition.y;

            if (!triggered && distanceY > triggerDistance)
            {
                triggered = true;
                TriggerAction();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            // Return button back to start
            rectTransform.anchoredPosition = startPosition;
        }

        void TriggerAction()
        {
            Debug.Log("Drag Up Triggered!");
            // Call your existing button method here
            // Example:
            // YourFunction();
        }
    }
}