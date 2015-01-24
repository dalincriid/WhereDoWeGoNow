using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

[Serializable]
public class EchoSphere{

    public enum ShaderPackinMode { Texture, Property };
    public ShaderPackinMode CurrentPackingMode = ShaderPackinMode.Property;

    public Texture2D    EchoTexture;
    public Material     EchoMaterial = null;
    public Vector3      Position;
    public int          SphereIndex = 0;

    public float        SphereMaxRadius = 10.0f;
    private float       SphereCurrentRadius = 0.0f;

    public float        FadeDelay = 0.0f;
    public float        FadeRate = 1.0f;
    public float        echoSpeed = 1.0f;

    private bool        IsAnimated = false;

    public float        PulseFrequency = 5.0f;
    private float       DeltaTime = 0.0f;
    private float       Fade = 0.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
        if (EchoMaterial == null) return;

        DeltaTime += Time.deltaTime;
        UpdateEcho();

        UpdateProperties();
	}

    public void TriggerPulse()
    {
        DeltaTime = 0.0f;
        SphereCurrentRadius = 0.0f;
        Fade = 0.0f;
        IsAnimated = true;
    }

    void HaltPulse()
    {
        IsAnimated = false;
    }

    void ClearPulse()
    {
        Fade = 0.0f;
        SphereCurrentRadius = 0.0f;
        IsAnimated = false;
    }

    void UpdateEcho()
    {
        if (!IsAnimated) return;
        if (SphereCurrentRadius >= SphereMaxRadius)
            HaltPulse();
        else
            SphereCurrentRadius += Time.deltaTime * echoSpeed;

        float radius = SphereCurrentRadius;
        float maxRadius = SphereMaxRadius;
        float maxFade = SphereMaxRadius / echoSpeed;
        if (Fade > maxFade)
            return;

        if (DeltaTime > FadeDelay)
            Fade += Time.deltaTime * FadeRate;
    }

    void UpdateProperties()
    {
        if (!IsAnimated) return;
        float MaxRadius = SphereMaxRadius;
        float MaxFade = SphereMaxRadius / echoSpeed;

        EchoMaterial.SetVector("_Position" + SphereIndex.ToString(), Position);
        EchoMaterial.SetFloat("_Radius" + SphereIndex.ToString(), SphereCurrentRadius);
        EchoMaterial.SetFloat("_Fade" + SphereIndex.ToString(), Fade);

        EchoMaterial.SetFloat("_MaxRadius", MaxRadius);
        EchoMaterial.SetFloat("_MaxFade", MaxFade);
    }


}


public class TriggerEchoes : MonoBehaviour
{
    public EchoSphere.ShaderPackinMode CurrentPackingMode = EchoSphere.ShaderPackinMode.Property;
    public Texture2D EchoTexture;
    public Material EchoMaterial = null;

    public int SphereCount = 1;
    public int CurrentSphere = 0;

    public float SphereMaxRadius = 10.0f;
    public float FadeDelay = 0.0f;
    public float FadeRate = 1.0f;
    public float echoSpeed = 1.0f;

    private List<EchoSphere> Spheres = new List<EchoSphere>();

    void Start()
    {
        CreateEchoTexture();
        InitializeSphere();
    }

    void InitializeSphere()
    {
        for (int i = 0; i < SphereCount; i++)
        {
            EchoSphere es = new EchoSphere{
                EchoMaterial = EchoMaterial,
                EchoTexture = EchoTexture,
                echoSpeed = echoSpeed,
                SphereMaxRadius = SphereMaxRadius,
                FadeDelay = FadeDelay,
                FadeRate = FadeRate,
                SphereIndex = i,
                CurrentPackingMode = CurrentPackingMode
            };
            Spheres.Add(es);
        }
    }

    void CreateEchoTexture()
    {
        EchoTexture = new Texture2D(128, 128, TextureFormat.RGBA32, false);
        EchoTexture.filterMode = FilterMode.Point;
        EchoTexture.Apply();

        EchoMaterial.SetTexture("_EchoTex", EchoTexture);
    }

    void Update()
    {
        if (EchoMaterial == null) return;
        foreach (EchoSphere es in Spheres)
            es.Update();
        UpdateRayCast();
    }

    void UpdateRayCast()
    {
        if (Input.GetMouseButton(0))
        {
            Spheres[CurrentSphere].TriggerPulse();
            Spheres[CurrentSphere].Position = this.transform.position;

            CurrentSphere++;
            if (CurrentSphere >= Spheres.Count) CurrentSphere = 0;
        }
    }
}