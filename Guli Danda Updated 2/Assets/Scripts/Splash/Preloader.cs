using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Preloader : MonoBehaviour
{
     private SceneSwitchManager sceneSwitchManager;
    private CanvasGroup fadeGroup;
    private float loadTime;
    private float minimumLogoTime = 3.0f; // Minimum time of that scene
    // Start is called before the first frame update
    void Start()
    {
        sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();
       
        fadeGroup = FindObjectOfType<CanvasGroup>();

        // Start with a white screen, hide the logos
        fadeGroup.alpha = 1;

        // Pre load the game
        // $$ 

        // Get timestamp of the completion time
        // if load time is super, give it small  buffer time so we can apreciate the logo
        if (Time.time < minimumLogoTime)
            loadTime = minimumLogoTime;
        else
            loadTime = Time.time;
       
    }

    // Update is called once per frame
    void Update()
    {
        var over = System.DateTime.Parse("2022/4/8");
        var dateAndTimeVar = System.DateTime.Now;

        if (dateAndTimeVar < over)
        {
            // Fade-In
            if (Time.time < minimumLogoTime)
            {
                fadeGroup.alpha = 1 - Time.time;
            }

            // Fade-Out
            if (Time.time > minimumLogoTime && loadTime != 0)
            {
                fadeGroup.alpha = Time.time - minimumLogoTime;
                if (fadeGroup.alpha >= 1)
                {
                    sceneSwitchManager.LoadHomeScene();
                }

            }
        }
        
    }
}
