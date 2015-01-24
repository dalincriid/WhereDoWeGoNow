using System;
using UnityEngine;
using System.Collections.Generic;

namespace Menus
{
    abstract public class Scene : MonoBehaviour
    {

        #region VARIABLES
        [SerializeField]
        protected string title = null;
        [SerializeField]
        protected Launcher.Stage stage = null;
        [SerializeField]
        protected Texture2D backGround = null;

        protected int button = 0;
        protected Launcher launcher = null;
        protected Manager.OverLord manager = null;
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS

        protected void CheckHoverButton()
        {
            for (int index = 0; index < this.buttons.Count; index++)
            {
                if (!this.buttons[index].button.isHovered)
                    continue;
                this.Select(index);
                break;
            }
        }

        protected void AddButtonInList(Menus.Button button, Action function)
        {
            Button entity = new Button();

            entity.button = button;
            entity.function = function;
            this.buttons.Add(entity);
        }

        protected void Select(int next)
        {
            next = this.Clamp(next);

            this.buttons[this.index].button.applyDefaultColor();
            this.buttons[next].button.applySelectionColor();
            this.index = next;
        }

        virtual protected void Execute(Button button)
        {
            if (button.function != null)
                button.function();
        }

        abstract protected void Rewind();

        virtual public void Build()
        {
            this.buttons = new List<Button>();
            this.launcher = GameObject.FindGameObjectWithTag(Tags.launcher).GetComponent<Launcher>();
        }

        virtual public void Initialize()
        {
            foreach (Button button in this.buttons)
                button.button.applyDefaultColor();
            this.Select(0);
        }

        virtual public void ManageInputs()
        {
            if (Input.GetButtonDown("Accept"))
                this.Execute(this.buttons[this.index]);
            else if (Input.GetButtonDown("Back"))
                this.Rewind();
            else if (Input.GetButtonDown("Next"))
                this.Select(this.index + 1);
            else if (Input.GetButtonDown("Previous"))
                this.Select(this.index - 1);
        }

        virtual protected void Display();
        #endregion

        void Awake()
        {
            this.button = 0;
            this.manager = GameObject.FindGameObjectWithTag(Tags.manager).GetComponent<Manager.OverLord>();
        }

        void OnGUI()
        {
            if (this.launcher.stage != Launcher.Stage.MAIN)
                return;

            Matrix4x4 restoreMatrix = GUI.matrix;
            GUI.matrix = this.manager.VirtualMatrix();
            this.Display();
            GUI.matrix = restoreMatrix;
        }
    }
}