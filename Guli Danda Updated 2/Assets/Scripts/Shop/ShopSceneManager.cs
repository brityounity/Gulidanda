using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSceneManager : MonoBehaviour
{

    private DatabaseController databaseController;
    private SceneSwitchManager sceneSwitchManager;

    [Header("Danda")]
    public GameObject dandaItemHolder;
    public GameObject dandaActiveButton;
    public GameObject dandaHide;
    public Image currentDandaImage;

    [Header("Guli")]
    public GameObject  guliItemHolder;
    public GameObject guliActiveButton;
    public GameObject guliHide;
     public Image currentGuliImage;
   
    // Start is called before the first frame update
    void Start()
    {
        databaseController = FindObjectOfType<DatabaseController>();
        sceneSwitchManager = FindObjectOfType<SceneSwitchManager>();

        ActiveDandaItem();
        LoadCurrentDandaAndGuli();
    }


    public void ActiveDandaItem(){
        dandaItemHolder.SetActive(true);
        dandaActiveButton.SetActive(false);
        dandaHide.SetActive(true);

        guliItemHolder.SetActive(false);
        guliActiveButton.SetActive(true);     
        guliHide.SetActive(false);
    } 

    public void ActiveGuliItem(){

        guliItemHolder.SetActive(true);
        guliActiveButton.SetActive(false);     
        guliHide.SetActive(true);

        dandaItemHolder.SetActive(false);
        dandaActiveButton.SetActive(true);
        dandaHide.SetActive(false);
    }

    void LoadCurrentDandaAndGuli(){
        int currentDanda = databaseController.GetUserInfo().current_danda;
        int currentGuli = databaseController.GetUserInfo().current_guli;

        currentDandaImage.sprite = Resources.Load<Sprite>("UI/new/shop/danda/"+currentDanda);
        currentGuliImage.sprite = Resources.Load<Sprite>("UI/new/shop/guli/"+currentGuli);

    }
    public void OnBackButtonClick(){
        sceneSwitchManager.LoadHomeScene();
    }
}
