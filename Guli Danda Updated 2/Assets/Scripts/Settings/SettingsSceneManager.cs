using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSceneManager : MonoBehaviour
{
    private SceneSwitchManager sceneSwitchManager;
    // Start is called before the first frame update
    void Start()
    {
        sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();
    }

    public void OnCloseButtonClick(){
        sceneSwitchManager.LoadHomeScene();
    }
}
