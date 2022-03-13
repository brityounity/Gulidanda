using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FielderAI : MonoBehaviour
{
    private GameplayManager gameplayManager;
    FielderThrow fielderThrow;
    GameObject guli;
    protected internal int fielderIndex;
    public float distanceFromGuli;
    bool throwGuli;
    public Transform guliThrowTarget;
    Animator fielderAnimator;
    // Start is called before the first frame update
    void Start()
    {
        gameplayManager = GameplayManager.GetGameplayManager();
        fielderThrow = GameObject.FindObjectOfType<FielderThrow>();
        guli = GameObject.FindGameObjectWithTag("guli");        

        throwGuli = false;
        guliThrowTarget = GameObject.FindGameObjectWithTag("guliThrowPosition").transform;
        fielderAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        if (gameplayManager.currentPhase == 1 && gameplayManager.checkFielderDistance == true)
        {
            distanceFromGuli = Vector3.Distance(this.transform.position, guli.transform.position);
            gameplayManager.lowestFielderDistance.SetValue(distanceFromGuli, fielderIndex);

            if (gameplayManager.CheckNearestFielderIndex() == fielderIndex)
            {
                Vector3 targetPosition = new Vector3(guli.transform.position.x, this.transform.position.y, guli.transform.position.z);
                float step = 5f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                
                //transform.LookAt(guli.transform.position);
                if (distanceFromGuli > 0.79)
                {
                    var dir = guli.transform.position - transform.position;
                    var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

                    fielderAnimator.SetBool("run", true);
                }

                if (distanceFromGuli <0.8 && throwGuli==false)
                {
                    
                    throwGuli = true;
                    var dir = guliThrowTarget.transform.position - transform.position;
                    var angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.up);

                    fielderAnimator.SetBool("run", false);
                    fielderAnimator.SetTrigger("throw");
                   
                    throwGuli = true;
                    guli.SetActive(false);
                   // gameplayManager.fielderGuli.SetActive(true);
                    fielderThrow.fielderPosition = this.transform;
                    fielderThrow.throwGuli = true;
                    fielderThrow.Launch();
                    
                }
            }
            
        }

        //Player out expression
        if (gameplayManager.playerCatchOut == true)
        {
            fielderAnimator.SetTrigger("win");
        }
 
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.gameObject.tag == "guli")
        {
            fielderAnimator.SetTrigger("catch");
            other.gameObject.SetActive(false);
            gameplayManager.PlayerCatchOut();
           
        }
    }

}
