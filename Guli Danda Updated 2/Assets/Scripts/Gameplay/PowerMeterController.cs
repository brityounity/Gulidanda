using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerMeterController : MonoBehaviour
{
    private GameplayManager gameplayManager;
    public Image powerMeter;
    protected internal float currentHeight;
    protected internal float counter;
    RectTransform myRectTransform;
    // Start is called before the first frame update
    void Start()
    {
        //heightMeter.fillAmount = 0.01f;
        counter = 0f;
       
        gameplayManager = GameplayManager.GetGameplayManager();
        myRectTransform = GetComponent<RectTransform>();
    }

    public void PowerMeterStart()
    {
       
        if (gameplayManager.powerMeterOn)
        {
            Time.timeScale = 1f;
           
            counter += Time.deltaTime * 700f;

            currentHeight = Mathf.PingPong(counter, 280f);
            currentHeight -= 140f;
           
            if (currentHeight < 0)
            {
                gameplayManager.powerMeterValue = (140f + currentHeight) / 140f;
            }

            if (currentHeight > 0)
            {
                gameplayManager.powerMeterValue = (140f - currentHeight) / 140f;
            }
            powerMeter.GetComponent<RectTransform>().transform.localPosition = new Vector3(0, currentHeight, 0);
            
        }


    }
}
