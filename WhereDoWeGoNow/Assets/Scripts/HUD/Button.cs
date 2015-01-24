using UnityEngine;
using System.Collections;

public class Button : MonoBehaviour
{
    private GameManager m_gameManager;

    protected void Awake()
    {
        var go = GameObject.FindGameObjectWithTag("GameManager");
        if (go != null)
        {
            m_gameManager = go.GetComponent<GameManager>();
        }
    }

    public void OnClick()
    {
        m_gameManager.OnButtonClicked(name);
    }
}
