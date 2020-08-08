using UnityEngine;
using UnityEngine.Events;

namespace Sweet_And_Salty_Studios
{
    public class UI_Manager : Singelton<UI_Manager>
    {
        #region VARIABLES

        public UnityEvent OnSwitchedScreen = new UnityEvent();

        public UI_Screen StartScreen;

        private UI_Screen[] ui_Screens;
        private UI_Screen currentScreen;
        private UI_Screen previousScreen;

        #endregion VARIABLES

        #region PROPERTIES

        #endregion PROPERTIES

        public UI_ScreenFader UI_ScreenFader
        {
            get;
            private set;
        }

        #region UNITY_FUNCTIONS

        private void Awake()
        {
            InitializeScreens();

            UI_ScreenFader = transform.Find("HUDCanvas").GetComponentInChildren<UI_ScreenFader>(true);
        }

        private void Start()
        {
            SwitchScreen(StartScreen);

            UI_ScreenFader.gameObject.SetActive(true);

            StartCoroutine(UI_ScreenFader.IFade(0f));
        }

        #endregion UNITY_FUNCTIONS

        #region CUSTOM_FUNCTIONS

        private void InitializeScreens()
        {
            ui_Screens = GetComponentsInChildren<UI_Screen>(true);

            foreach(var ui_Screen in ui_Screens)
            {
                if(ui_Screen != StartScreen)
                {
                    ui_Screen.gameObject.SetActive(false);
                }
            }
        }

        public void SwitchScreen(UI_Screen ui_Screen)
        {
            if(ui_Screen == null)
            {
                return;
            }

            if(currentScreen)
            {
                currentScreen.Close();     

                currentScreen.gameObject.SetActive(false);
                previousScreen = currentScreen;
            }

            currentScreen = ui_Screen;
            currentScreen.gameObject.SetActive(true);
            currentScreen.Open();

            OnSwitchedScreen.Invoke();
        }

        public void SwitchPreviousScreen()
        {
            SwitchScreen(previousScreen);
        }

        #endregion CUSTOM_FUNCTIONS
    }
}
