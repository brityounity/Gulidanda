using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectEnvironmentSceneManager : MonoBehaviour
{
    private SceneSwitchManager sceneSwitchManager;
    // Start is called before the first frame update
    void Start()
    {
         sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();
    }

    // Update is called once per frame
    public void OnBackButtonClick()
    {
        sceneSwitchManager.LoadHomeScene();
    }
}
