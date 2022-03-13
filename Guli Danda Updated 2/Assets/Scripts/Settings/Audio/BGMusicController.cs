using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMusicController : MonoBehaviour
{
     private static BGMusicController instance = null;
   void Awake()
    {
       if (instance != null && instance != this)
        {
            //Destroy game object if multiple instance exists
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
       
    }

    // Update is called once per frame
    void LateUpdate () {
        
        //check music volume on runtime
        if (PlayerPrefs.GetInt("set music") != 0)
        {
            this.GetComponent<AudioSource>().volume = PlayerPrefs.GetFloat("music volume");
        }
        else
        {
            this.GetComponent<AudioSource>().volume = 1;
        } 
    }
}
