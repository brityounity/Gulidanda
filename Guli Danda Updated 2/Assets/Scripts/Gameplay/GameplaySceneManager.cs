using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplaySceneManager : MonoBehaviour
{
    private GameplayManager gameplayManager;
    private DatabaseController databaseController;
    private SceneSwitchManager sceneSwitchManager;
    public Text playerNameText;
    public GameObject gameQuitWarningPanel;
    //public Text userCoinText;
    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = GetComponent<GameplayManager>();
        databaseController = FindObjectOfType<DatabaseController>();
        sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();
        playerNameText.text = databaseController.GetUserInfo().player_name;
        gameQuitWarningPanel.SetActive(false);
      //  userCoinText.text = databaseController.GetUserInfo().total_coin.ToString();
    }

    public void OnBackButtonClick()
    {
        gameplayManager.gamePause = true;
        gameQuitWarningPanel.SetActive(true);
    }

    public void CloseWarningPopup()
    {
        gameplayManager.gamePause = false;
        gameQuitWarningPanel.SetActive(false);
    }

    public void OnQuitGameConfirmation()
    {
        gameplayManager.gamePause = false;
        sceneSwitchManager.LoadHomeScene();
    }

    public void LoadHomeScene()
    {
        sceneSwitchManager.LoadHomeScene();
    }
}
