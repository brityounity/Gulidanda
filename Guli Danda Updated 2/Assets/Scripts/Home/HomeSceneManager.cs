using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeSceneManager : MonoBehaviour
{
    private DatabaseController databaseController;
    private SceneSwitchManager sceneSwitchManager;

    DateTime blockTime;
    long getBlockTime;

    public Text coinText;
    public Text playerNameText;
    public Text playerIDText;
    public GameObject instructionPopUp;
    public GameObject adRewardPopUp;
    public Button adButton;
    public Text adButtonText;
    // Start is called before the first frame update
    void Start()
    {
        databaseController = FindObjectOfType<DatabaseController>();
        sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();

        coinText.text = databaseController.GetUserInfo().total_coin.ToString();
        if(databaseController.GetUserInfo().player_name == null || databaseController.GetUserInfo().player_name == "")
        {
            SetUserName();
        }
        else
        {
            playerNameText.text = databaseController.GetUserInfo().player_name.ToString();
            playerIDText.text = databaseController.GetUserInfo().player_id.ToString();
        }
        if(databaseController.GetUserInfo().total_coin < 500)
        {
            adRewardPopUp.SetActive(true);
        }
        else
        {
            adRewardPopUp.SetActive(false);
        }
        instructionPopUp.SetActive(false);
    }

    private void Update()
    {
        AdTimeCounter();
    }
    void AdTimeCounter()
    {
        DateTime currentDate = DateTime.Now;
        //Grab the old time from the player prefs as a long
        string isBlockTimeAvailable = PlayerPrefs.GetString("block time");
        if (isBlockTimeAvailable == null || isBlockTimeAvailable == "" || isBlockTimeAvailable == " ")
        {
            getBlockTime = Convert.ToInt64(DateTime.Now.ToBinary().ToString());
        }
        else
        {
            getBlockTime = Convert.ToInt64(PlayerPrefs.GetString("block time"));

        }
        //Convert the old time from binary to a DataTime variable
        blockTime = DateTime.FromBinary(getBlockTime);

        //Use the Subtract method and store the result as a timespan variable
        TimeSpan difference = blockTime.Subtract(currentDate);

        if (difference.TotalSeconds >= 1)
        {
            adButtonText.gameObject.SetActive(true);
            int miniute = difference.Minutes;
            int seconds = difference.Seconds;
            adButtonText.text = miniute + ":" + seconds;
            adButton.interactable = false;
        }
        else if (difference.TotalSeconds < 1)
        {
            adButtonText.gameObject.SetActive(false);
            adButton.interactable = true;
        }
        else
        {
            adButton.interactable = false;
        }
    }

    public void OnRateUsButtonClick()
    {
#if UNITY_ANDROID
            Application.OpenURL("market://details?id=" + Application.identifier);
#elif UNITY_IPHONE
        Application.OpenURL("itms-apps://itunes.apple.com/app/APP_ID");
#endif
    }

    void SetUserName()
    {
        int deviceUID = SystemInfo.graphicsDeviceID;
        deviceUID = deviceUID % 100000;
        string playerName = "Player#" + deviceUID;
     
        databaseController.SetUserName(playerName);
        databaseController.SetUserID(deviceUID.ToString());

        playerNameText.text = databaseController.GetUserInfo().player_name.ToString();
        playerIDText.text = databaseController.GetUserInfo().player_id.ToString();

    }

    // Update is called once per frame
    public void OnProfileButtonClick()
    {
        sceneSwitchManager.LoadProfileScene();
    }

    public void OnPlayButtonClick(){
        sceneSwitchManager.LoadSelectEnvironmentScene();
    }

    public void OnSettingsButtonClick(){
        sceneSwitchManager.LoadSettingsScene();
    }

    public void OnShopButtonClick(){
        sceneSwitchManager.LoadShopScene();
    }

    public void OnHelpButtonClick(){
        sceneSwitchManager.LoadHelpScene();
    }
    public void OnLeaderBoardButtonClick(){
        sceneSwitchManager.LoadLeaderBoardScene();
    }

    public void OnInstructionButtonClick()
    {
        instructionPopUp.SetActive(true);
    }

    public void OnInstructionCloseButtonClick()
    {
        instructionPopUp.SetActive(false);
    }
    public void OnAdRewardCloseButtonClick()
    {
        adRewardPopUp.SetActive(false);
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
