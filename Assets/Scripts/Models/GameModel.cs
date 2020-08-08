using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public abstract class GameModel : MonoBehaviour
    {
        public virtual void OnDespawn()
        {
            transform.SetPositionAndRotation(Vector2.zero, Quaternion.Euler(Vector2.zero));
            ObjectPoolManager.Instance.Despawn(this);
        }
    }
}