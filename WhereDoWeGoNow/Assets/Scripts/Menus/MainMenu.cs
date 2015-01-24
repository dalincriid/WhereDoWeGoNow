using System;
using UnityEngine;

namespace Menus
{
    public class MainMenu : Scene
    {
        #region VARIABLES
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        private void Play()
        {
            Fade.LoadLevel("NetworkScene", 2.0f, 2.0f, Color.black);
        }

        private void Option()
        {
            this.launcher.MoveInto(Launcher.Stage.OPTIONS);
        }

        private void Credit()
        {
            Fade.LoadLevel("Credits", 2.0f, 2.0f, Color.black);
        }

        private void Quit()
        {
            Application.Quit();
        }

        override protected void Rewind()
        {
        }
        #endregion

        void Start()
        {
            this.stage = Launcher.Stage.MAIN;
            this.buttons = new string[] { "Play", "Options", "Credits", "Quits" };
            this.actions = new Action[] { this.Play, this.Option, this.Credit, this.Quit };
        }
    }
}