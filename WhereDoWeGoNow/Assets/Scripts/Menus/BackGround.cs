using UnityEngine;

namespace Menus
{
    [RequireComponent(typeof(GUITexture))]
    public class BackGround : MonoBehaviour
    {
        #region VARIABLES
        private GUITexture receptacle = null;
        #endregion

        #region PROPERTIES
        #endregion

        #region FUNCTIONS
        public void change(Texture2D texture)
        {
            this.receptacle.texture = texture;
        }
        #endregion

        void Start()
        {
            Vector2 resolution = new Vector2(Screen.width, Screen.height);

            this.receptacle = this.gameObject.GetComponent<GUITexture>();
            receptacle.pixelInset = new Rect(-resolution.x / 2, -resolution.y / 2, resolution.x, resolution.y);
        }

        void Update()
        {
        }
    }
}