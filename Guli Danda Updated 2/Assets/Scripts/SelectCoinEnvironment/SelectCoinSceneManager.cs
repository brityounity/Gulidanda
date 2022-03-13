using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCoinSceneManager : MonoBehaviour
{
    private SceneSwitchManager sceneSwitchManager;
    private DatabaseController databaseController;
    public string selectedScene;
   
    void Start()
    {
        sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();
        databaseController = FindObjectOfType<DatabaseController>();

        selectedScene = PlayerPrefs.GetString("current environment");
    }

    public void OnCoinSelectClick(int amount){
        
        if(databaseController.GetUserInfo().total_coin >= amount){
            PlayerPrefs.SetInt("current coin", amount);
            databaseController.SubtractCoin(amount);
            sceneSwitchManager.LoadGameScene(selectedScene);
        }
        
    }

    public void OnBackButtonClick(){
       
        sceneSwitchManager.LoadSelectEnvironmentScene();
    }
}
