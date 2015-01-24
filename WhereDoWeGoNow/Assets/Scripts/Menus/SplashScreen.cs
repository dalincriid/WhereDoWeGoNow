using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Menus
{
    [RequireComponent(typeof(GUITexture))]
    public class SplashScreen : MonoBehaviour
    {
        #region VARIABLES
        [SerializeField]
        private float fadeTimeLapse;
        [SerializeField]
        private float displayTimeLapse;
        [SerializeField]
        private List<Texture2D> textureList;
        private Vector2 screenResolution;
        private GUITexture receptacle;
        private Camera mainCamera;
        private Color ambient;
        private bool fadeOut;
        private int texture;
        private float timer;
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        private void ResizeVisual()
        {
            Texture2D texture = this.textureList[this.texture];
            Vector2 resolution;

            if (texture.width > this.screenResolution.x || texture.height > this.screenResolution.y)
                resolution = this.screenResolution;
            else
                resolution = new Vector2(texture.width, texture.height);
            this.receptacle.pixelInset = new Rect(-resolution.x / 2, -resolution.y / 2, resolution.x, resolution.y);
        }

        private bool ChangeSplashScreen()
        {
            Fade.FadeIn(this.fadeTimeLapse, this.ambient);
            Texture2D texture = this.textureList[this.texture];

            this.timer = 0.0f;
            this.fadeOut = true;
            this.ResizeVisual();
            this.receptacle.texture = texture;
            this.ambient = texture.GetPixel(0, 0);
            this.mainCamera.backgroundColor = this.ambient;
            this.texture++;
            return true;
        }

        private void FadeOut()
        {
            this.fadeOut = false;
            Fade.FadeOut(this.fadeTimeLapse, this.ambient);
        }
        #endregion

        void OnEnable()
        {
            this.mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            this.screenResolution = new Vector2(Screen.width, Screen.height);
            this.receptacle = this.gameObject.GetComponent<GUITexture>();
            this.texture = 0;
        }

        void Start()
        {
            this.ChangeSplashScreen();
        }

        void Update()
        {
            this.timer += Time.deltaTime;
            if (this.texture >= this.textureList.Count && !Fade.isFading && this.timer >= this.displayTimeLapse)
                Fade.LoadLevel("Menus", this.fadeTimeLapse, this.fadeTimeLapse, this.ambient);
            else if (this.fadeOut && this.timer >= this.displayTimeLapse + this.fadeTimeLapse)
                this.FadeOut();
            else if (this.timer >= this.displayTimeLapse + this.fadeTimeLapse * 2)
                this.ChangeSplashScreen();
            if (Input.GetButtonDown("Skip"))
                Application.LoadLevel("Menus");//Application.LoadLevel("FlyLeaf");
        }
    }
}