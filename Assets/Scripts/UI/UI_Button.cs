using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Sweet_And_Salty_Studios
{
    public class UI_Button : UI_Element
    {
        #region VARIABLES

        public UnityEvent ButtonEvent = new UnityEvent();

        private Vector2 defaultScale;
        private Vector2 activeScale;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            defaultScale = transform.localScale;
            activeScale = defaultScale + new Vector2(0.025f, 0.025f);
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            transform.localScale = activeScale;
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            transform.localScale = defaultScale;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            ButtonEvent.Invoke();           
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}
