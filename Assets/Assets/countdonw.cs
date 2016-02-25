using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class countdonw : MonoBehaviour {
    public Text time; // text for the timer
    private int step; // this is mila seconds
    private int sec; // this is seconds
    private int min; // this is minits 
    private int hours; // this is hours
    private int torf; // check if it one
    void Start()
    {
        // set all varbuls to zeros
        torf = 0;
        step = 60;
        sec = 60;
        min = 06;
        hours = 00;
    }

    void Update()
    {
        // the acule timer it self
        if (torf > 0)
        {
            if (torf <= 3)
            {
                step = step - 1;
                if (step == 0)
                {
                    sec = sec - 1;
                    step = 60;
                }
                if (sec == 00)
                {
                    min = min - 1;
                    sec = 60;
                }
                if (min == 00)
                {
                    hours = hours - 1;
                    min = 60;
                }
                time.text = "Time: " + hours.ToString() + " : " + min.ToString() + " : " + sec.ToString() + "  " + "LAP: " + torf.ToString() + "/3";
            }
        }
    }
    void OnTriggerExit(Collider Other)
    {
        torf = torf + 1;
    }
    
}
