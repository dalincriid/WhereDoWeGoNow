using UnityEngine;
using System.Collections;


public class Fade : MonoBehaviour
{
    #region VARIABLES
    private int levelIndex = 0;
    private bool fading = false;
    private string levelName = null;
    private Material material = null;
    private static Fade instance = null;
    #endregion

    #region PROPERTIES
    private static Fade Instance
    {
        get
        {
            if (instance == null)
                instance = (new GameObject("Fade")).AddComponent<Fade>();
            return instance;
        }
    }

    public static bool isFading
    {
        get { return Instance.fading; }
    }
    #endregion

    #region FUNCTIONS
    private void drawQuad(Color color, float alpha)
    {
        color.a = alpha;
        this.material.SetPass(0);
        GL.PushMatrix();
        GL.LoadOrtho();
        GL.Begin(GL.QUADS);
        GL.Color(color);
        GL.Vertex3(0, 0, -1);
        GL.Vertex3(0, 1, -1);
        GL.Vertex3(1, 1, -1);
        GL.Vertex3(1, 0, -1);
        GL.End();
        GL.PopMatrix();
    }

    private IEnumerator CrossLevel(float fadeOutTime, float fadeInTime, Color color)
    {
        float time = 0.0f;

        while (time < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp01(time + Time.deltaTime / fadeOutTime);
            this.drawQuad(color, time);
        }
        if (this.levelName != null)
            Application.LoadLevel(this.levelName);
        else
            Application.LoadLevel(this.levelIndex);
        while (time > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp01(time - Time.deltaTime / fadeInTime);
            this.drawQuad(color, time);
        }
        this.fading = false;
    }

    private IEnumerator FadeInto(float timeLapse, Color color)
    {
        float time = 1.0f;

        while (time > 0.0f)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp01(time - Time.deltaTime / timeLapse);
            this.drawQuad(color, time);
        }
        this.fading = false;
    }

    private IEnumerator FadeAway(float timeLapse, Color color)
    {
        float time = 0.0f;

        while (time < 1.0f)
        {
            yield return new WaitForEndOfFrame();
            time = Mathf.Clamp01(time + Time.deltaTime / timeLapse);
            this.drawQuad(color, time);
        }
        this.fading = false;
    }

    public static void FadeIn(float timeLapse, Color color)
    {
        if (isFading)
            return;
        instance.fading = true;
        instance.StartCoroutine(instance.FadeInto(timeLapse, color));
    }
    public static void FadeOut(float timeLapse, Color color)
    {
        if (isFading)
            return;
        instance.fading = true;
        instance.StartCoroutine(instance.FadeAway(timeLapse, color));
    }

    public static void LoadLevel(string levelName, float fadeOutTime, float fadeInTime, Color color)
    {
        if (isFading)
            return;
        instance.fading = true;
        instance.levelName = levelName;
        instance.StartCoroutine(instance.CrossLevel(fadeOutTime, fadeInTime, color));
    }

    public static void LoadLevel(int levelIndex, float fadeOutTime, float fadeInTime, Color color)
    {
        if (isFading)
            return;
        instance.fading = true;
        instance.levelName = null;
        instance.levelIndex = levelIndex;
        instance.StartCoroutine(instance.CrossLevel(fadeOutTime, fadeInTime, color));
    }
    #endregion

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        this.material = new Material("Shader \"Plane/No zTest\" { SubShader { Pass { Blend SrcAlpha OneMinusSrcAlpha ZWrite Off Cull Off Fog { Mode Off } BindChannels { Bind \"Color\",color } } } }");
    }

    void Start()
    {
    }

    void Update()
    {
    }
}