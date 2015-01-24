using System;
using UnityEngine;
using System.Collections.Generic;

namespace Menus
{
    public class VideoSettings : Scene
    {
        #region VARIABLES
        private int fullScreen = 0;
        private int resolution = 0;
        private int quality = 0;
        private int vSync = 0;
        private int antiAliasing = 0;

        private string[] qualities;
        private int[] aliasingFilters;
        private Resolution[] resolutions;
        private string[] synchronizations;
        private Action<int>[] funtcions = null;
        private Dictionary<string, string> datas = null;
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        private int Clamp(int value, int limit)
        {
            if (value > limit)
                return 0;
            else if (value < 0)
                return limit;
            else
                return value;
        }

        private void Save()
        {
            int filter = this.aliasingFilters[this.antiAliasing];

            PlayerPrefs.SetInt("AntiAliasing", filter);
            PlayerPrefs.SetInt("Quality", this.quality);
            PlayerPrefs.SetInt("FullSreen", this.fullScreen);
            PlayerPrefs.SetInt("Resolution", this.resolution);
            PlayerPrefs.SetInt("VerticalSynchronization", this.vSync);
        }

        private void FullScreen(int value)
        {
            if (value != 0)
                this.fullScreen ^= 1;
            this.datas["FullScreen"] = (this.fullScreen > 0) ? "ON" : "OFF";
        }

        private void Resolution(int value)
        {
            this.resolution = this.Clamp(this.resolution + value, this.resolutions.Length - 1);
            Resolution resolution = this.resolutions[this.resolution];

            this.datas["Resolution"] = resolution.width + " x " + resolution.height;
        }
        
        private void Quality(int value)
        {
            this.quality = this.Clamp(this.quality + value, this.qualities.Length - 1);

            this.datas["Quality"] = this.qualities[this.quality];
        }
        
        private void VerticalSynchronization(int value)
        {
            this.vSync = this.Clamp(this.vSync + value, this.synchronizations.Length - 1);

            this.datas["vSync"] = this.synchronizations[this.vSync];
        }
        
        private void AntiAliasing(int value)
        {
            this.antiAliasing = this.Clamp(this.antiAliasing + value, this.aliasingFilters.Length - 1);

            this.datas["AntiAliasing"] = "x " + this.aliasingFilters[this.antiAliasing].ToString();
        }

        private void DisplayEscape()
        {
            float screenWidth = this.manager.defaultResolution.x;

            if (GUI.Button(new Rect(50, 950, 200, 100), "Back", this.skin.button))
                this.launcher.MoveInto(Launcher.Stage.OPTIONS);
            if (GUI.Button(new Rect(screenWidth - 50, 950, 200, 100), "Apply", this.skin.button))
                this.Execute();
        }

        private void DisplayData()
        {
            Vector2 size = new Vector2(500, 600);
            Vector2 resolution = this.manager.defaultResolution;
            Rect position = new Rect(resolution.x / 2, (resolution.y / 2) - (size.y / 2) + 100, size.x, size.y);
            string[] labels = new string[this.datas.Count];

            this.datas.Values.CopyTo(labels, 0);
            GUI.SelectionGrid(position, 0, labels, 1, this.skin.GetStyle("Text"));
        }

        override protected void DisplayButtons()
        {
            Vector2 size = new Vector2(500, 600);
            Vector2 resolution = this.manager.defaultResolution;
            Rect position = new Rect((resolution.x / 2) - size.x, (resolution.y / 2) - (size.y / 2) + 100, size.x, size.y);

            int selection = GUI.SelectionGrid(position, this.button, this.buttons, 1, this.skin.GetStyle("Button"));
            if (selection == this.button)
                return;
            this.button = selection;
        }

        override protected void Execute()
        {
            this.Save();
            this.manager.SuperviseScreen();
        }

        override protected void Rewind()
        {
            this.launcher.stage = Launcher.Stage.OPTIONS;
        }

        override protected void Display()
        {
            base.Display();
            this.DisplayData();
            this.DisplayEscape();
        }

        override public void Reload()
        {
            this.quality = this.manager.quality;
            this.fullScreen = this.manager.fullScreen;
            this.resolution = this.manager.resolution;
            this.vSync = this.manager.vSynchronization;
            this.antiAliasing = Array.IndexOf(this.aliasingFilters, this.manager.antiAliasing);

            Resolution resolution = this.resolutions[this.resolution];

            this.datas["FullScreen"] = (this.fullScreen > 0) ? "ON" : "OFF";
            this.datas["Resolution"] = resolution.width + " x " + resolution.height;
            this.datas["Quality"] = this.qualities[this.quality];
            this.datas["vSync"] = this.synchronizations[this.vSync];
            this.datas["AntiAliasing"] = "x " + this.aliasingFilters[this.antiAliasing].ToString();
        }

        override protected void ManageInputs()
        {
            base.ManageInputs();
            if (Input.GetButtonDown("Increase"))
                this.funtcions[this.button](1);
            else if (Input.GetButtonDown("Decrease"))
                this.funtcions[this.button](-1);
        }
        #endregion

        override protected void Awake()
        {
            base.Awake();
            this.resolutions = Screen.resolutions;
            this.qualities = QualitySettings.names;
            this.aliasingFilters = new int[] { 0, 2, 4, 8 };
            this.synchronizations = new string[] { "Don't Sync", "Every VBlank", "Every Second VBlank" };
        }
            
        void Start()
        {
            this.stage = Launcher.Stage.VIDEO;
            this.datas = new Dictionary<string, string>();
            this.buttons = new string[] { "FullScreen", "Resolution", "Quality", "vSync", "Anti Aliasing" };
            this.funtcions = new Action<int>[] { this.FullScreen, this.Resolution, this.Quality, this.VerticalSynchronization, this.AntiAliasing };
        }
    }
}