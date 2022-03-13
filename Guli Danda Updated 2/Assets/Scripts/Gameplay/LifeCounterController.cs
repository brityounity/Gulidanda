using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeCounterController : MonoBehaviour
{
    private GameplayManager gameplayManager;
    public GameObject[] lifes;
    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = GetComponent<GameplayManager>();
        CheckLife();
    }

    public void CheckLife(){

        if(gameplayManager.lifeCount >= 3){
            lifes[0].SetActive(true);
            lifes[1].SetActive(true);
            lifes[2].SetActive(true);
        }
        if(gameplayManager.lifeCount == 2){
            lifes[0].SetActive(true);
            lifes[1].SetActive(true);
            lifes[2].SetActive(false);
        }
        if(gameplayManager.lifeCount == 1){
            lifes[0].SetActive(true);
            lifes[1].SetActive(false);
            lifes[2].SetActive(false);
        }
        if(gameplayManager.lifeCount == 0){
            lifes[0].SetActive(false);
            lifes[1].SetActive(false);
            lifes[2].SetActive(false);
        }
        
    }
}
