using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneSwitchManager : MonoBehaviour
{
    public void LoadSplashScene()
    {
        SceneManager.LoadScene("0Splash");
    }
    public void LoadHomeScene()
    {
        SceneManager.LoadScene("1Home");
    }
    public void LoadSelectEnvironmentScene()
    {
        SceneManager.LoadScene("2SelectEnvironment");
    }
    public void LoadSelectCoinScene()
    {
        SceneManager.LoadScene("4SelectCoin");
    }
    public void LoadProfileScene()
    {
        SceneManager.LoadScene("5Profile");
    }
     public void LoadSettingsScene()
    {
        SceneManager.LoadScene("6Settings");
    }
    public void LoadLeaderBoardScene()
    {
        SceneManager.LoadScene("7LeaderBoard");
    }
    public void LoadShopScene()
    {
        SceneManager.LoadScene("8Shop");
    }
    public void LoadHelpScene()
    {
        SceneManager.LoadScene("9Help");
    }

    public void LoadGameScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
    

}
