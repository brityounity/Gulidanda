using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class DatabaseController : MonoBehaviour
{

#region User
    public User GetUserInfo(){

        string Data;
        if (PlayerPrefs.GetString("user") != "")
        {
            Data = PlayerPrefs.GetString("user");
            
        }
        else
        {
            TextAsset file = Resources.Load("Database/user") as TextAsset;
            Data = file.text;
            
        }
         
        User user = JsonConvert.DeserializeObject<User>(Data.ToString());
       
        return user;
    }

    public void SetUserName(string name)
    {
        User user = GetUserInfo();
        user.player_name = name;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }

    public void SetUserID(string id)
    {
        User user = GetUserInfo();
        user.player_id = id;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
    #endregion

    #region Coin
    public void AddCoin(int coinAmount){
        User user = GetUserInfo();
        user.total_coin += coinAmount;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
    public void SubtractCoin(int coinAmount){
        User user = GetUserInfo();
        user.total_coin -= coinAmount;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
    public void UpdateUserCoin(int updatedCoin)
    {
        User user = GetUserInfo();
        user.total_coin = updatedCoin;
        
        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
#endregion

#region MatchData
    public void AddNewMatch(){
        User user = GetUserInfo();
        user.total_match += 1;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
    public void MatchResult(bool matchWin){
        User user = GetUserInfo();
        if(matchWin == true) user.total_win += 1;    
        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
#endregion

#region Dadna
    public List<Danda> GetAllUserDandas(){
        User user = GetUserInfo();
        List<Danda>dandas  = user.dandas;

        return dandas;
    }
    public void BuyNewDanda(Danda newDanda){
        User user = GetUserInfo();
        user.dandas.Add(newDanda);
        user.total_coin -= newDanda.buying_price;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
    public void SetCurrentDanda(int dandaID){
        User user = GetUserInfo();
        user.current_danda = dandaID;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
#endregion

#region Guli
    public List<Guli> GetAllUserGulis(){
        User user = GetUserInfo();
        List<Guli>gulis  = user.gulis;

        return gulis;
    }
    public void BuyNewGuli(Guli newGuli){
        User user = GetUserInfo();
        user.gulis.Add(newGuli);
        user.total_coin -= newGuli.buying_price;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));

    }
    public void SetCurrentguli(int guliID){
        User user = GetUserInfo();
        user.current_guli = guliID;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }
#endregion

#region Environment
    public  List<Environment> GetAllEnvironment(){
        User user = GetUserInfo();
        List<Environment>environments  = user.environments;

        return environments;
    }

    public void AddNewEnvironment(Environment newEnvironment){
        User user = GetUserInfo();
        user.environments.Add(newEnvironment);
        user.total_coin-=newEnvironment.unlock_cost;

        PlayerPrefs.SetString("user", JsonConvert.SerializeObject(user));
    }

#endregion
}
