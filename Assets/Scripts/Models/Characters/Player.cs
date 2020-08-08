using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Player : Character
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected override void Awake()
        {
            base.Awake();

            movementSpeed = 500f;        
        }

        protected override IEnumerator IHandleTarget()
        {
            while(gameObject.activeSelf)
            {
                yield return new WaitUntil(() => InputManager.Instance.IsValidMousePosition);

                SetNewTargetPosition(GameManager.Instance.ConvertScreenToWorldPoint(Input.mousePosition));
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hittedObject = collision.gameObject;

            switch(hittedObject.layer)
            {
                case 9:

                    ObjectPoolManager.Instance.Spawn<Explosion>(
                        transform.position, 
                        Quaternion.identity, 
                        null);

                    OnDespawn();

                    break;

                default:

                    break;
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}