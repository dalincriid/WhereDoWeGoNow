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
            LEVELS,
            DIFFICULTY,
            OPTIONS,
            VIDEO,
            SOUNDS,
            LANGUAGES,
            CONTROLS,
        }

        #region VARIABLES
        [NonSerialized]
        public Stage stage;
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        private void LoadScene()
        {
            Texture2D backGround = this.curentStage.backGround;

            this.curentStage.scene.Initialize();
            this.transform.position = this.curentStage.position;
            this.transform.localEulerAngles = this.curentStage.rotation;
            //if (backGround)
            //    this.backGround.change(backGround);
        }

        private void BuildScenes()
        {
            foreach (Stage stage in this.stages)
                stage.scene.Build();
        }

        public void MoveInto(StageID identity)
        {
            foreach (Stage stage in this.stages)
            {
                if (stage.identity == identity)
                {
                    this.curentStage = stage;
                    break;
                }
            }
            this.LoadScene();
        }
        #endregion

        void Start()
        {
            this.BuildScenes();
            this.MoveInto(StageID.MAIN);
            //this.backGround = GameObject.FindGameObjectWithTag(Tags.backGround).GetComponent<BackGround>();

            //this.manager.manageCursor(true, false);
            //this.audioPlayer.playMusic(this.audioDataBase.mainMenuMusic);
        }

        void Update()
        {
            Scene scene = this.curentStage.scene;

            scene.ManageButtons();
            scene.ManageInputs();
        }
    }
}