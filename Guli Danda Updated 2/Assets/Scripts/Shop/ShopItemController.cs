using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemController : MonoBehaviour
{
    private DatabaseController databaseController;
    public enum ItemType {Danda, Guli}; 
    public ItemType itemType;
    public int itemID;
    public int itemBuyingCost;
    public GameObject usingItemButton;
    public GameObject useItemButton;
    public GameObject buyItemButton;
    public Image currentlyUsingItem;
  
    void Start()
    {
        databaseController = FindObjectOfType<DatabaseController>();

        usingItemButton.SetActive(false);
        useItemButton.SetActive(false);
        buyItemButton.SetActive(false);

        CheckItemStatus();
        
    }

    void Update(){
        if(itemType == ItemType.Danda){
            if(itemID != databaseController.GetUserInfo().current_danda){
                usingItemButton.SetActive(false);
                useItemButton.SetActive(true);
            }
        }else{
            if(itemID != databaseController.GetUserInfo().current_guli){
                usingItemButton.SetActive(false);
                useItemButton.SetActive(true);
            }
        }
    }
    // Update is called once per frame
    void CheckItemStatus()
    {
        if(itemType == ItemType.Danda){
            List<Danda> dandas = databaseController.GetUserInfo().dandas;
            bool existInPlayerLis = dandas.Any(item => item.danda_id == itemID);
            if(existInPlayerLis == true){
                if(itemID == databaseController.GetUserInfo().current_danda){
                    usingItemButton.SetActive(true);
                }else{
                    useItemButton.SetActive(true);
                }
            }else{
                buyItemButton.SetActive(true);
                buyItemButton.GetComponentInChildren<Text>().text = itemBuyingCost.ToString();
            }
        }else{
            List<Guli> gulis = databaseController.GetUserInfo().gulis;
            bool existInPlayerLis = gulis.Any(item => item.guli_id == itemID);
            if(existInPlayerLis == true){
                if(itemID == databaseController.GetUserInfo().current_guli){
                    usingItemButton.SetActive(true);
                }else{
                    useItemButton.SetActive(true);
                }
            }else{
                buyItemButton.SetActive(true);
                buyItemButton.GetComponentInChildren<Text>().text = itemBuyingCost.ToString();
            }
        }
    }

    public void OnItemBuyClick(){
        if(itemType == ItemType.Danda){
            Danda danda = new Danda();
            danda.danda_id = itemID;
            danda.buying_price = itemBuyingCost;
            if(databaseController.GetUserInfo().total_coin >= itemBuyingCost){
                databaseController.BuyNewDanda(danda);
                useItemButton.SetActive(true);
                usingItemButton.SetActive(false);
                buyItemButton.SetActive(false);
            }
            
        }else{
            Guli guli = new Guli();
            guli.guli_id = itemID;
            guli.buying_price = itemBuyingCost;
            if(databaseController.GetUserInfo().total_coin >= itemBuyingCost){
                databaseController.BuyNewGuli(guli);
                useItemButton.SetActive(true);
                usingItemButton.SetActive(false);
                buyItemButton.SetActive(false);
            }

        }
    }
    public void OnItemUseClick(){
        if(itemType == ItemType.Danda){
            databaseController.SetCurrentDanda(itemID);
            useItemButton.SetActive(false);
            usingItemButton.SetActive(true);
            buyItemButton.SetActive(false);
            currentlyUsingItem.sprite = Resources.Load<Sprite>("UI/new/shop/danda/"+itemID);

        }else{
            databaseController.SetCurrentguli(itemID);
            useItemButton.SetActive(false);
            usingItemButton.SetActive(true);
            buyItemButton.SetActive(false);
            currentlyUsingItem.sprite = Resources.Load<Sprite>("UI/new/shop/guli/"+itemID);
        }

        
    }

}
