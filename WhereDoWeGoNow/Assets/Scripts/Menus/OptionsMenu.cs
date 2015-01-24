using System;
using UnityEngine;

namespace Menus
{
    public class OptionsMenu : Scene
    {
        #region VARIABLES
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        private void Video()
        {
            this.launcher.MoveInto(Launcher.Stage.VIDEO);
        }

        private void Sounds()
        {
            this.launcher.MoveInto(Launcher.Stage.SOUNDS);
        }

        override protected void Rewind()
        {
            this.launcher.stage = Launcher.Stage.MAIN;
        }
        #endregion

        void Start()
        {
            this.stage = Launcher.Stage.OPTIONS;
            this.buttons = new string[] { "Video", "Sounds" };
            this.actions = new Action[] { this.Video, this.Sounds };
        }
    }
}