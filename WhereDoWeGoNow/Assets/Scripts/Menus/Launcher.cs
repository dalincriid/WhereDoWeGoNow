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
        }

        public void MoveInto(Stage stage)
        {
            var scenes = Resources.FindObjectsOfTypeAll(typeof(Scene)) as Scene[];

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
            this.stage = Stage.MAIN;
        }

        void Update()
        {
            
        }
    }
}