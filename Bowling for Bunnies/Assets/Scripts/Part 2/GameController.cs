using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Reset button
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1.0f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        // Quit button
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Time.timeScale = 1.0f;
            Application.Quit();
        }

        //Slow Motion
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Time.timeScale == 1.0f)
            {
                Time.timeScale = 0.5f;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
            
        }
    }
}
