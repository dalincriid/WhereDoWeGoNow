using UnityEngine;
using System.Collections;

public class Countdown : MonoBehaviour
{
	public float myCountdown = 120.0f;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		this.myCountdown -= Time.deltaTime;
	}

	void OnGUI()
	{
		string timeToDisplay = "";
		if (this.myCountdown > 0.0f)
		{
			int minutes = (int)this.myCountdown / 60;
			timeToDisplay += minutes;
			timeToDisplay += ":";
			int seconds = (int)this.myCountdown % 60;
			if (seconds < 10)
			{
				timeToDisplay += "0";
			}
			timeToDisplay += seconds;
		}
		else
		{
			timeToDisplay = "Game Over";
		}
		GUI.Label(new Rect(Screen.width / 2, 100, 100, 100), timeToDisplay);
	}
}
