using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlleyController : MonoBehaviour
{

    // variables for the count and time remaining
    private int count;
    private float timeLeft;

    // text variables
    public Text countText;
    public Text timeText;
    public Text gameText;

    // Used individiual variables to count each pin
    public GameObject pin1;
    public GameObject pin2;
    public GameObject pin3;
    public GameObject pin4;
    public GameObject pin5;
    public GameObject pin6;

    // boolean variables to check if the pins had already been toppled over
    private bool pin1Check;
    private bool pin2Check;
    private bool pin3Check;
    private bool pin4Check;
    private bool pin5Check;
    private bool pin6Check;

    // boolean variable to check if the text signalling the end of the game is already showing
    private bool gameTextCheck;

    // Use this for initialization
    void Start()
    {
        count = 0;
        timeLeft = 10.0f;
        SetText();
        gameTextCheck = false;
        pin1Check = false;
        pin2Check = false;
        pin3Check = false;
        pin4Check = false;
        pin5Check = false;
        pin6Check = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Decrement the time
        timeLeft -= Time.deltaTime;

        // Check each pin if it gets toppled over, thereby changing the boolean variable to show the pin had toppled over
        pin1Check = AddToCount(pin1, pin1Check);
        pin2Check = AddToCount(pin2, pin2Check);
        pin3Check = AddToCount(pin3, pin3Check);
        pin4Check = AddToCount(pin4, pin4Check);
        pin5Check = AddToCount(pin5, pin5Check);
        pin6Check = AddToCount(pin6, pin6Check);

        // Call Text method
        SetText();

        // Check if player had toppled over all 6 pins and if the time had not yet expired
        if (count == 6 && !gameTextCheck)
        {
            gameText.text = "Congrats! You won the game!";
            gameTextCheck = true;
        }

        // Reset button
        if(Input.GetKeyDown(KeyCode.R))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        // Quit button
        if(Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }
    }

    // Add the toppled pin to the count if it had just been toppled
    bool AddToCount(GameObject pin, bool pinCheck)
    {
        if(pinCheck)
        {
            return true;   
        }

        if (pin.transform.eulerAngles.y > 45.0f)
        {
            count++;
            return true;
        }
        return false;
    }

    // Text for count, time, and game (if game ended without toppling over all 6 pins)
    void SetText()
    {
        
        countText.text = "Count: " + count.ToString();
        if (timeLeft < 0.0f)
        {
            timeText.text = "0.0";
            if(!gameTextCheck)
            {
                gameText.text = "What are you doing?";
                gameTextCheck = true;
            }
            
        }
        else
        {
            timeText.text = timeLeft.ToString("N1");
        }
    }
}
