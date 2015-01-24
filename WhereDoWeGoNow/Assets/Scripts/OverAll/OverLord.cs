using System;
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
        [NonSerialized]
        public Vector2 defaultResolution;
        #endregion

        #region PROPERTIES
        public float sfx { get; private set; }
        public float music { get; private set; }
        public int quality { get; private set; }
        public int fullScreen { get; private set; }
        public int resolution { get; private set; }
        public int antiAliasing { get; private set; }
        public int vSynchronization { get; private set; }
        #endregion

        #region FUNCTIONS
        private void LoadVideoSettings()
        {
            this.quality = PlayerPrefs.GetInt("Quality");
            this.fullScreen = PlayerPrefs.GetInt("FullSreen");
            this.resolution = PlayerPrefs.GetInt("Resolution");
            this.antiAliasing = PlayerPrefs.GetInt("AntiAliasing");
            this.vSynchronization = PlayerPrefs.GetInt("VerticalSynchronization");
        }

        private void ApplyVideoSettings()
        {
            Resolution resolution = Screen.resolutions[this.resolution];
            bool fullScreen = (this.fullScreen > 0) ? true : false;

            QualitySettings.SetQualityLevel(this.quality);
            QualitySettings.antiAliasing = this.antiAliasing;
            QualitySettings.vSyncCount = this.vSynchronization;
            Screen.SetResolution(resolution.width, resolution.height, fullScreen);
        }

        private void LoadSoundSettings()
        {
            this.music = PlayerPrefs.GetFloat("Music");
            this.sfx = PlayerPrefs.GetFloat("SoundEffects");
        }

        private void ApplySoundSettings()
        {
        }

        public void SuperviseScreen()
        {
            this.LoadVideoSettings();
            this.ApplyVideoSettings();
        }

        public void SuperviseAudio()
        {
            this.LoadSoundSettings();
            this.ApplySoundSettings();
        }

        public Matrix4x4 VirtualMatrix()
        {
            return (Matrix4x4.TRS(new Vector3(1, 1, 1), Quaternion.identity, new Vector3(Screen.width / this.defaultResolution.x, Screen.height / this.defaultResolution.y, 1)));
        }
        #endregion

        void Awake()
        {
            DontDestroyOnLoad(this);
            this.defaultResolution = new Vector2(1920, 1080);
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