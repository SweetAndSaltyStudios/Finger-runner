using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Collectable : GameModel
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            var hittedObject = collision.gameObject;

            switch(hittedObject.layer)
            {
                case 8:

                    ObjectPoolManager.Instance.Spawn<PickUpEffect>(transform.position, Quaternion.identity, null);

                    GameManager.Instance.OnScoreIncreased.Invoke();

                    OnDespawn();

                    break;

                default:

                    break;
            }
        }
    }
}