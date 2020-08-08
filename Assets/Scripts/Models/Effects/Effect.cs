using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class Effect : GameModel
    {
        #region VARIABLES

        private ParticleSystem _particleSystem;

        private Coroutine iEffectDuration_Coroutine;
        private WaitForSeconds effectDuration;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        protected void Awake()
        {
            _particleSystem = GetComponentInChildren<ParticleSystem>();
            effectDuration = new WaitForSeconds(_particleSystem.main.duration);
        }

        private void OnEnable()
        {
            if(iEffectDuration_Coroutine != null)
            {
                StopCoroutine(iEffectDuration_Coroutine);
                iEffectDuration_Coroutine = null;
            }

            iEffectDuration_Coroutine = StartCoroutine(IEffectDuration());
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private IEnumerator IEffectDuration()
        {
            yield return effectDuration;

            OnDespawn();
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
