using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Manager
{
    public class OverLord : MonoBehaviour
    {
        private enum Langage
        {
            French,
            English,
            Japonese,
            Chinese,
        }

        #region VARIABLES
        #endregion

        #region PROPERTIES
        public int quality { get; private set; }
        public int fullScreen { get; private set; }
        public int resolution { get; private set; }
        public int antiAliasing { get; private set; }
        public int vSynchronization { get; private set; }
        #endregion

        #region FUNCTIONS
        private void LoadSettings()
        {
            this.quality = PlayerPrefs.GetInt("Quality");
            this.fullScreen = PlayerPrefs.GetInt("FullSreen");
            this.resolution = PlayerPrefs.GetInt("Resolution");
            this.antiAliasing = PlayerPrefs.GetInt("AntiAliasing");
            this.vSynchronization = PlayerPrefs.GetInt("VerticalSynchronization");
        }

        private void ApplySettings()
        {
            Resolution resolution = Screen.resolutions[this.resolution];
            bool fullScreen = (this.fullScreen > 0) ? true : false;

            QualitySettings.SetQualityLevel(this.quality);
            QualitySettings.antiAliasing = this.antiAliasing;
            QualitySettings.vSyncCount = this.vSynchronization;
            Screen.SetResolution(resolution.width, resolution.height, fullScreen);
        }

        public void SuperviseScreen()
        {
            this.LoadSettings();
            this.ApplySettings();
        }

        public Matrix4x4 VirtualMatrix()
        {
            return (Matrix4x4.TRS(new Vector3(1, 1, 1), Quaternion.identity, new Vector3(Screen.width / 1920, Screen.height / 1080, 1)));
        }
        #endregion

        void Awake()
        {
            DontDestroyOnLoad(this);
        }

        void Start()
        {
#if UNITY_EDITOR
            PlayerPrefs.DeleteAll();
#endif
            this.SuperviseScreen();
            Application.LoadLevel("SplashScreen");
        }

        void Update()
        {
        }
    }
}