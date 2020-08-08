using UnityEngine;
using UnityEngine.Events;

namespace Sweet_And_Salty_Studios
{
    public class UI_Screen : MonoBehaviour
    {
        #region VARIABLES

        [Space]
        [Header("Events")]

        public UnityEvent OnScreenOpen = new UnityEvent();
        public UnityEvent OnScreenClose = new UnityEvent();

        private int openAnimaiton_Hash;
        private int closeAnimation_Hash;

        #endregion VARIABLES

        #region PROPERTIES

        public Animator Animator
        {
            get;
            private set;
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            Animator = GetComponent<Animator>();
            openAnimaiton_Hash = Animator.StringToHash("Open");
            closeAnimation_Hash = Animator.StringToHash("Close");
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public virtual void Open()
        {
            // Wait if previous screens close animation is playing         
            OnScreenOpen.Invoke();

            HandleAnimator(openAnimaiton_Hash);
        }

        public virtual void Close()
        {
            OnScreenClose.Invoke();

            HandleAnimator(closeAnimation_Hash);
        }

        private void HandleAnimator(int animationHash)
        {
            if(Animator)
            {
                Animator.SetTrigger(animationHash);
            }
        }

        public bool IsAnimatorPlaying()
        {
            return Animator.GetCurrentAnimatorStateInfo(0).length >
                   Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        }

        #endregion CUSTOM_FUNCTIONS       
    }
}
