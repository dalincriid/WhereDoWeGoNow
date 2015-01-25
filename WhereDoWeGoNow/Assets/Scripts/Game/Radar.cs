using System;
using UnityEngine;
using System.Collections.Generic;

public class Radar : MonoBehaviour
{
    #region VARIABLES
    [SerializeField]
    private float range = 0.0f;
    [SerializeField]
    private float timeLaps = 0.0f;

    private float timer = 0.0f;
    private float alpha = 0.0f;
    private bool inGame = false;
    private GUITexture curtain = null;
    private List<GameObject> partners = null;
    #endregion

    #region PROPERTIES
    #endregion

    #region FUNCTIONS
    private void EndGame()
    {
        this.alpha = Mathf.Lerp(this.alpha, 0.8f, 0.1f * Time.deltaTime);

        this.curtain.color = new Color(1, 1, 1, this.alpha);
        this.timer -= Time.deltaTime;
    }

    private void FindOutOthers()
    {
        CharacterController[] characters = GameObject.FindObjectsOfType<CharacterController>() as CharacterController[];

        Debug.Log(characters.Length);
        foreach (CharacterController player in characters)
            if (player.gameObject.tag != "Player")
                this.partners.Add(player.gameObject);
    }

    private bool Scan(Vector3 other)
    {
        Ray ray;
        RaycastHit hit;
        Vector3 direction = other - this.transform.position;

        direction.Normalize();
        ray = new Ray(this.transform.position, direction);

        Debug.DrawRay(this.transform.position, direction * this.range);

        if (!Physics.Raycast(ray, out hit, this.range) || !this.partners.Contains(hit.collider.gameObject))
            return false;
        Debug.Log("True");
        return true;
    }
    #endregion
    void Awake()
    {
        this.curtain = GameObject.FindGameObjectWithTag("FinalCurtain").guiTexture;
    }

    void Start()
    {
        this.inGame = true;
        this.partners = new List<GameObject>();
        this.curtain.color = new Color(1, 1, 1, this.alpha);
        this.FindOutOthers();
    }

    void Update()
    {
        if (inGame)
        {
            foreach (GameObject other in this.partners)
                if (!this.Scan(other.transform.position))
                    return;
            this.inGame = false;
            this.timer = this.timeLaps;
        }
        if (this.timer > 0.0f)
            this.EndGame();
        else
            Fade.LoadLevel("Credits", 2.0f, 2.0f, Color.black);
    }
}