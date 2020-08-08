using UnityEngine;
using UnityEngine.EventSystems;

namespace Sweet_And_Salty_Studios
{
    public class InputManager : Singelton<InputManager>
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        public bool IsValidMousePositionDown
        {
            get
            {
                return Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject() == false;
            }
        }

        public bool IsValidMousePosition
        {
            get
            {
                return Input.GetMouseButton(0) && EventSystem.current.IsPointerOverGameObject() == false;
            }
        }

        public bool IsValidMousePositionUp
        {
            get
            {
                return Input.GetMouseButtonUp(0) && EventSystem.current.IsPointerOverGameObject() == false;
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}
