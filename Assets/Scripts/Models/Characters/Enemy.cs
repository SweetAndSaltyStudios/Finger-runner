using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Enemy : Character
    {
        #region VARIABLES

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hittedObject = collision.gameObject;

            switch(hittedObject.layer)
            {
                case 9:

                    ObjectPoolManager.Instance.Spawn<Explosion>(transform.position, Quaternion.identity, null);

                    OnDespawn();

                    break;

                default:

                    break;
            }
        }

        protected override IEnumerator IHandleTarget()
        {
            while(gameObject.activeSelf)
            {
                SetNewTargetPosition(GameManager.Instance.Player.transform.position);

                yield return null;
            }
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}