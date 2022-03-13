using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;


public class FielderThrow : MonoBehaviour
{
    private GameplayManager gameplayManager;
    private GameObject hitGuli;
    
    //public float h = 25.0f;
    // public float gravity = -18.0f;
    public float zRange=7f;
    public float xRange=3f;
    public float guliThrowSpeed = 20f;
   // public GameObject outImage;

    GameObject groundDanda;
   
    Transform guliTargetPosition;
    Rigidbody rbGuli;
    public bool throwGuli = false;
    protected internal Transform fielderPosition;
    protected internal bool hitDanda;
    public GameObject tempGuli;
    bool targetset;
    float waittime;
    float targetXPosition;
    float targetZPosition;
    // Start is called before the first frame update
    void Start()
    {

        gameplayManager = GameplayManager.GetGameplayManager();
        hitGuli = GameObject.FindGameObjectWithTag("guli");        

        throwGuli = false;
        targetset = false;
        hitDanda = false;
        waittime = 0f;
        targetXPosition = 0f;
        targetZPosition = 0f;
        //outImage.gameObject.SetActive(false);
    }
    public void Launch()
    {
        groundDanda = GameObject.FindGameObjectWithTag("groundDanda");

        
        if (targetset == false)
        {
           
            tempGuli.transform.position = new Vector3(fielderPosition.transform.position.x + 0.3f, 1.23f, fielderPosition.transform.position.z + 0.5f);
 
            targetXPosition = Random.Range(groundDanda.transform.position.x - xRange, groundDanda.transform.position.x + xRange);
            targetZPosition = Random.Range(groundDanda.transform.position.z - zRange, groundDanda.transform.position.z + zRange);
            targetset = true;
            //tempGuli.SetActive(true);
        }
        

        waittime += Time.deltaTime;
        
        if (waittime > 2.7f && gameplayManager.currentPhase != 2)
        {
           
            tempGuli.SetActive(true);
            Vector3 targetPosition = new Vector3(targetXPosition, groundDanda.transform.position.y, targetZPosition);          
            float step = guliThrowSpeed * Time.deltaTime;
            tempGuli.transform.position = Vector3.MoveTowards(tempGuli.transform.position, targetPosition, step);
            float distanceFromDanda = Vector3.Distance(tempGuli.transform.position, groundDanda.transform.position);          
            float distanceFromTarget = Vector3.Distance(tempGuli.transform.position, targetPosition);

            if (distanceFromTarget == 0f)
            {
                gameplayManager.checkFielderDistance = false;
               // gameplayManager.throwGuliPosition = targetPosition;
                
                tempGuli.SetActive(false);
                hitGuli.SetActive(true);
                hitGuli.transform.position = targetPosition;
                hitGuli.GetComponent<Rigidbody>().isKinematic = false;
               // hitGuli.GetComponent<PlayerOutController>().checkRestart = false;
                gameplayManager.currentPhase = 2;
                gameplayManager.setHitterForSecondPhase = true;
               
               
            }
            
        }

    }
   
    private IEnumerator Reload(float t)
    {
        yield return new WaitForSeconds(t);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void CalculateTargetPosition()
    {
        float tempMinX = groundDanda.transform.position.x;
    }
    // Update is called once per frame
    void Update()
    {
        if (throwGuli == true)
        {
            Launch();
        }
    }
}
