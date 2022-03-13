using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileSceneManager : MonoBehaviour
{
    private DatabaseController databaseController;
    private SceneSwitchManager sceneSwitchManager;

    public GameObject itemImagePrefab;
    public GameObject dandaListContent;
    public GameObject guliListContent;
    public GameObject profileEditPopUp;
    public Text playerNameText;
    public Text totalMatchText;
    public Text totalWinText;
    public Text winRatioText;
    public Text totalCoinText;
    public InputField nameEditInput;
    // Start is called before the first frame update
    void Start()
    {
        databaseController = FindObjectOfType<DatabaseController>();
        sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();
        profileEditPopUp.SetActive(false);
        int totalMatchPlayed = databaseController.GetUserInfo().total_match;
        int totalWin = databaseController.GetUserInfo().total_win;       
        playerNameText.text = databaseController.GetUserInfo().player_name;
        totalMatchText.text = totalMatchPlayed.ToString();
        totalWinText.text = totalWin.ToString();
        totalMatchPlayed = totalMatchPlayed == 0 ? 1 : totalMatchPlayed;
        Debug.Log(totalWin * 100 / (totalMatchPlayed == 0 ? 1 : totalMatchPlayed));
        winRatioText.text = (totalWin * 100 / (totalMatchPlayed == 0 ? 1 : totalMatchPlayed)).ToString() + " %";
        totalCoinText.text = databaseController.GetUserInfo().total_coin.ToString();
        
        LoadItems();
    }

    // Update is called once per frame
    void LoadItems()
    {
        List<Danda> dandas = databaseController.GetAllUserDandas();
        List<Guli> gulis = databaseController.GetAllUserGulis();
        Debug.Log(dandas.Count);
        foreach(Danda danda in dandas){
            GameObject newDanda = Instantiate(itemImagePrefab);
            //newDanda.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/mcc");
            newDanda.transform.GetChild(0).GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("UI/new/shop/danda/"+danda.danda_id);
            newDanda.transform.parent = dandaListContent.transform;
            newDanda.transform.localScale = Vector3.one;
        }

        foreach(Guli guli in gulis){
            GameObject newGuli = Instantiate(itemImagePrefab);
            
           // newGuli.transform.GetComponent<Image>().sprite = Resources.Load<Sprite>("UI/new/shop/box-item-select");
            newGuli.transform.GetChild(0).GetComponentInChildren<Image>().sprite =Resources.Load<Sprite>("UI/new/shop/guli/"+guli.guli_id);
            newGuli.transform.parent = guliListContent.transform;
            newGuli.transform.localScale = Vector3.one;
        }
    }

    public void OnProfileSaveButtonClick()
    {
        string updatedName = nameEditInput.text;
        if(updatedName.Length > 0)
        {
            databaseController.SetUserName(updatedName);
            playerNameText.text = databaseController.GetUserInfo().player_name;
            profileEditPopUp.SetActive(false);
        }
        else
        {
            profileEditPopUp.SetActive(false);
        }
       
    }

    public void OnNameEditButtonClick()
    {
        profileEditPopUp.SetActive(true);
        nameEditInput.text = databaseController.GetUserInfo().player_name;
    }

    public void OnCloseButtonClick()
    {
        profileEditPopUp.SetActive(false);
    }
    public void OnBackButtonClick(){
        sceneSwitchManager.LoadHomeScene();
    }

    
}
