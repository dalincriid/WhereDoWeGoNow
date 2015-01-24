using UnityEngine;

namespace Menus
{
    public class Credits : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField]
        private float flowSpeed = 2.0f;
        [SerializeField]
        private float finalPosition = 0.0f;

        private float speed = 0.0f;
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        private void rollCredits()
        {
            this.transform.Translate(Vector3.up * this.speed * Time.deltaTime);
        }

        private void manageInput()
        {
            if (Input.GetButtonDown("Skip"))
                Fade.LoadLevel("Menus", 2, 2, Color.black);
            else if (Input.GetButton("Next") || Input.GetButton("Increase"))
                this.speed = this.flowSpeed * 4.0f;
            else if (Input.GetButton("Previous") || Input.GetButton("Decrease"))
                this.speed = this.flowSpeed * -4.0f;
            else
                this.speed = this.flowSpeed;
        }
        #endregion

        void Start()
        {
            //this.audioPlayer.playMusic(this.audioDataBase.creditsMusic);
        }

        void Update()
        {
            this.manageInput();
            if (this.transform.position.y < this.finalPosition)
                this.rollCredits();
        }
    }
}