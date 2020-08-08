using TMPro;
using UnityEngine.Events;

namespace Sweet_And_Salty_Studios
{
    public class UI_GameScreen : UI_Screen
    {
        #region VARIABLES

        public TextMeshProUGUI ScoreText;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            GameManager.Instance.OnScoreIncreased.AddListener(
                () =>
                {
                    ScoreText.text = GameManager.Instance.Score.ToString();
                });
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        #endregion CUSTOM_FUNCTIONS
    }
}
