using System.Collections;
using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class UI_ScreenFader : MonoBehaviour
    {
        #region VARIABLES

        private CanvasGroup canvasGroup;
        private GameObject fadeImageGameObject;
        private bool isFading;
        private float startFadeTime;

        #endregion VARIABLES

        #region PROPERTIES

        public bool CanFade
        {
            get
            {
                return canvasGroup != null || isFading == false;
            }
        }

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            fadeImageGameObject = transform.GetChild(0).gameObject;
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        public IEnumerator IFade(float targetAlpha, float fadeDuration = 1f)
        {
            if(CanFade == false)
            {
                yield break;
            }

            fadeImageGameObject.SetActive(true);
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            isFading = true;

            startFadeTime = Time.time;
            var startAlpha = canvasGroup.alpha;
            var currentFadeTime = 0f;
            var percentageComplete = 0f;

            while(canvasGroup.alpha != targetAlpha)
            {
                currentFadeTime = Time.time - startFadeTime;
                percentageComplete = currentFadeTime / fadeDuration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, percentageComplete);
                yield return null;
            }

            if(targetAlpha == 0)
            {
                fadeImageGameObject.SetActive(false);
            }

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            isFading = false;
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
