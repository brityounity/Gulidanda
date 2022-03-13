using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnvironmentPropertyController : MonoBehaviour
{
    private DatabaseController databaseController;
    private SceneSwitchManager sceneSwitchManager;
    public GameObject lockImage;
    public GameObject unlockEnvironmentPopUp;
    public GameObject buyButton;
    public int environmentID;
    public enum EnvironmentName {TownRoad, Village, CropField, School, Jungle}; 
    public EnvironmentName environmentName;
    public int unlockCost;
    public bool environmentUnlocked = false;
    // Start is called before the first frame update
    void Start()
    {
        databaseController = FindObjectOfType<DatabaseController>();
        sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();
        buyButton.GetComponentInChildren<Text>().text = unlockCost.ToString();
        List<Environment> environments = databaseController.GetAllEnvironment();
        foreach(Environment environment in environments){
            if(environment.environment_id == environmentID){
                environmentUnlocked = true;
            }
        }
      
        lockImage.SetActive(!environmentUnlocked);
        buyButton.SetActive(!environmentUnlocked);
        if(databaseController.GetUserInfo().total_coin < unlockCost){
            buyButton.GetComponent<Button>().enabled = false;
            buyButton.GetComponent<Image>().color = new Color(0.5f,0.5f,0.5f,1f);
        }
		GetComponent<Button>().enabled = environmentUnlocked;      
       // unlockEnvironmentPopUp.SetActive(false);
    }

    public void OnEnvironmentSelect(){
        PlayerPrefs.SetString("current environment", environmentName.ToString());
        
        sceneSwitchManager.LoadSelectCoinScene();
    }

    public void UnlockThisEnvironmentCheck(){
        if(databaseController.GetUserInfo().total_coin > unlockCost){
            unlockEnvironmentPopUp.SetActive(true);
        }
    }

    public void UnlockThisEnvironment(){
        if(databaseController.GetUserInfo().total_coin >= unlockCost){
            Environment environment = new Environment();
            environment.environment_id = environmentID;
            environment.environment_name = environmentName.ToString();
            environment.unlock_cost = unlockCost;

            databaseController.AddNewEnvironment(environment);
            GetComponent<Button>().enabled = true;
            unlockEnvironmentPopUp.SetActive(false);
            lockImage.SetActive(false);
            buyButton.SetActive(false);
        }
    }

}


