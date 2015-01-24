using System;
using UnityEngine;
using System.Collections.Generic;

namespace Menus
{
    public class Launcher : MonoBehaviour
    {
        public enum Stage
        {
            MAIN,
            OPTIONS,
            VIDEO,
            SOUNDS,
        }

        #region VARIABLES
        [NonSerialized]
        public Stage stage;

        private Scene scene = null;
        private BackGround backGround = null;
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        private void ChargeScene(Scene scene)
        {
            scene.Reload();
            if (scene.backGround)
                this.backGround.change(scene.backGround);
            this.scene = scene;
        }

        public void MoveInto(Stage stage)
        {
            Scene[] scenes = Resources.FindObjectsOfTypeAll(typeof(Scene)) as Scene[];

            foreach (Scene scene in scenes)
                if (scene.stage == stage)
                    this.ChargeScene(scene);
            this.stage = stage;
        }
        #endregion

        void Awake()
        {
            this.backGround = GameObject.FindGameObjectWithTag(Tags.backGround).GetComponent<BackGround>();
        }


        void Start()
        {
            this.MoveInto(Stage.MAIN);
        }

        void Update()
        {
            this.scene.ManageInputs();
        }
    }
}