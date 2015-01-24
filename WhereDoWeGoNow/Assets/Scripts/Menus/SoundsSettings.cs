using System;
using UnityEngine;

namespace Menus
{
    public class SoundsSettings : Scene
    {
        #region VARIABLES
        private float sfx = 0.0f;
        private float music = 0.0f;

        private Action<float>[] funtcions = null;
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        private void Save()
        {
            PlayerPrefs.SetFloat("Music", this.music);
            PlayerPrefs.SetFloat("SoundEffects", this.sfx);
        }

        private void SFX(float value)
        {
            this.sfx = Mathf.Clamp(this.sfx + value, 0.0f, 1.0f);
        }

        private void Music(float value)
        {
            this.music = Mathf.Clamp(this.music + value, 0.0f, 1.0f);
        }

        private void DisplayData()
        {
            Vector2 size = new Vector2(500, 60);
            Vector2 resolution = this.manager.defaultResolution;
            Rect sfxPosition = new Rect((resolution.x / 2) - size.x, (resolution.y / 2) + 220, size.x, size.y);
            Rect musicPosition = new Rect((resolution.x / 2) - size.x, (resolution.y / 2) - 80, size.x, size.y);

            this.sfx = GUI.HorizontalSlider(sfxPosition, this.sfx, 0.0f, 1.0f, this.skin.horizontalSlider, this.skin.horizontalSliderThumb);
            this.music = GUI.HorizontalSlider(musicPosition, this.music, 0.0f, 1.0f, this.skin.horizontalSlider, this.skin.horizontalSliderThumb);

            sfxPosition.x += 600;
            musicPosition.x += 600;
            GUI.Label(sfxPosition, ((int)(this.sfx * 100)).ToString(), this.skin.GetStyle("Text"));
            GUI.Label(musicPosition, ((int)(this.music * 100)).ToString(), this.skin.GetStyle("Text"));
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
            this.manager.SuperviseAudio();
        }

        override protected void Rewind()
        {
            this.launcher.stage = Launcher.Stage.OPTIONS;
        }

        override protected void Display()
        {
            base.Display();
            this.DisplayData();
        }

        override public void Reload()
        {
            this.sfx = this.manager.sfx;
            this.music = this.manager.music;
        }

        override protected void ManageInputs()
        {
            base.ManageInputs();
            if (Input.GetButtonDown("Increase"))
                this.funtcions[this.button](0.1f);
            else if (Input.GetButtonDown("Decrease"))
                this.funtcions[this.button](-0.1f);
        }
        #endregion

        void Start()
        {
            this.stage = Launcher.Stage.SOUNDS;
            this.buttons = new string[] { "Music", "Sound Effects"};
            this.funtcions = new Action<float>[] { this.Music, this.SFX };
        }
    }
}