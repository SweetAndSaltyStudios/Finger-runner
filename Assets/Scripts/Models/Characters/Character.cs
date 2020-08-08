using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public abstract class Character : GameModel
    {
        #region VARIABLES

        private TrailRenderer trailRenderer;
        private Rigidbody2D rb2D;

        private Vector2 currentTargetPosition;
        protected float movementSpeed = 400f;
        protected Coroutine iHandleTarget_Coroutine;

        #endregion VARIABLES

        #region PROPERTIES

        public Vector2 MovementDirection
        {
            get
            {
                return (currentTargetPosition - (Vector2)transform.position).normalized;
            }
        }

        public float MovementDistance
        {
            get
            {
                return (currentTargetPosition - (Vector2)transform.position).sqrMagnitude;
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected virtual void Awake()
        {
            rb2D = GetComponent<Rigidbody2D>();
            trailRenderer = GetComponentInChildren<TrailRenderer>();
        }

        protected virtual void OnEnable()
        {
            if(iHandleTarget_Coroutine == null)
            {
                iHandleTarget_Coroutine = StartCoroutine(IHandleTarget());
            }

            trailRenderer.Clear();
        }

        protected virtual void OnDisable()
        {
            if(iHandleTarget_Coroutine != null)
            {
                StopCoroutine(iHandleTarget_Coroutine);
                iHandleTarget_Coroutine = null;
            }
        }

        private void FixedUpdate()
        {
            if(MovementDistance < 0.1f)
            {
               

                return;
            }

            rb2D.velocity = MovementDirection * movementSpeed * Time.fixedDeltaTime;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        protected abstract IEnumerator IHandleTarget();

        protected void SetNewTargetPosition(Vector2 targetPosition)
        {
            if(currentTargetPosition == targetPosition)
            {
                return;
            }

            currentTargetPosition = targetPosition;
        }

        public override void OnDespawn()
        {
            currentTargetPosition = Vector2.zero;

            rb2D.velocity = Vector2.zero;
            rb2D.position = Vector2.zero;
            rb2D.rotation = 0f;

            base.OnDespawn();
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
