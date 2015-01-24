using System;
using UnityEngine;
using System.Collections.Generic;

namespace Menus
{
    abstract public class Scene : MonoBehaviour
    {

        #region VARIABLES
        [SerializeField]
        protected GUISkin skin = null;
        [SerializeField]
        protected string title = null;

        public Launcher.Stage stage;
        public Texture2D backGround = null;

        protected int button = 0;
        protected string[] buttons = null;
        protected Action[] actions = null;
        protected Launcher launcher = null;
        protected Manager.OverLord manager = null;

        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        protected void Select(int index)
        {
            int limit = this.buttons.Length - 1;

            if (index > limit)
                this.button = 0;
            else if (index < 0)
                this.button = limit;
            else
                this.button = index;
        }

        virtual protected void Execute()
        {
            this.actions[this.button]();
        }

        abstract protected void Rewind();

        

        private void DisplayTitle()
        {
            Vector2 resolution = this.manager.defaultResolution;
            Vector2 size = this.skin.GetStyle("Title").CalcSize(new GUIContent(this.title));

            GUI.Label(new Rect((resolution.x / 2) - (size.x / 2), 50, size.x, size.y), this.title, this.skin.GetStyle("Title"));
        }

        virtual protected void DisplayButtons()
        {
            Vector2 size = new Vector2(500, 600);
            Vector2 resolution = this.manager.defaultResolution;
            Rect position = new Rect((resolution.x / 2) - (size.x / 2), (resolution.y / 2) - (size.y / 2) + 100, size.x, size.y);

            int selection = GUI.SelectionGrid(position, this.button, this.buttons, 1, this.skin.button);
            if (selection == this.button)
                return;
            this.button = selection;
            this.Execute();
        }

        virtual protected void Display()
        {
            this.DisplayTitle();
            this.DisplayButtons();
        }

        virtual public void Reload()
        {
        }

        virtual protected void ManageInputs()
        {
            if (Input.GetButtonDown("Accept"))
                this.Execute();
            else if (Input.GetButtonDown("Back"))
                this.Rewind();
            else if (Input.GetButtonDown("Next"))
                this.Select(this.button + 1);
            else if (Input.GetButtonDown("Previous"))
                this.Select(this.button - 1);
        }
        #endregion

        virtual protected void Awake()
        {
            this.button = 0;
            this.launcher = GameObject.FindGameObjectWithTag(Tags.launcher).GetComponent<Launcher>();
            this.manager = GameObject.FindGameObjectWithTag(Tags.manager).GetComponent<Manager.OverLord>();
        }

        void Update()
        {
            this.ManageInputs();
        }

        void OnGUI()
        {
            if (this.launcher.stage != this.stage)
                return;

            Matrix4x4 restoreMatrix = GUI.matrix;
            GUI.matrix = this.manager.VirtualMatrix();
            this.Display();
            GUI.matrix = restoreMatrix;
        }
    }
}