using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timer : MonoBehaviour {
    
    public Text time; // text for the timer
    private int step; // this is mila seconds
    private int sec; // this is seconds
    private int min; // this is minits 
    private int hours; // this is hours
    private int torf; // check if it one
	void Start () {
        // set all varbuls to zeros
        torf = 0; 
        step = 00;
        sec = 00;
        min = 00;
        hours = 00;
	}
	
	void Update () {
        // the acule timer it self
        if (torf == 1)
        {
            step = step + 1;
            if (step == 60)
            {
                sec = sec + 1;
                step = 0;
            }
            if (sec == 60)
            {
                min = min + 1;
                sec = 0;
            }
            if (min == 60)
            {
                hours = hours + 1;
                min = 0;
            }
            time.text = "Time: " + hours.ToString() + " : " + min.ToString() + " : " + sec.ToString();
        }
    }
    void OnTriggerExit(Collider Other)
    {
        torf = 1;
    }
    void OnTriggerEnter(Collider Other)
    {
        torf = 0;
    }
}
