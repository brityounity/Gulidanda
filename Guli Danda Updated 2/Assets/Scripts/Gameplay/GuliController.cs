using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuliController : MonoBehaviour
{
    private GameplayManager gameplayManager;
    private Transform ground;
    private float power;
   
   

    Rigidbody rb;
    private LineRenderer lr;
    private Camera camera;
    Vector3 startPoint,endPoint, startPos;
    int touchCount = 0;

    [Header("Phase 1")]
    public float minMaxX;
    public float maxY;
    public float maxZ;
    public float zCoFactor;

    [Header("Phase 2")]
    
    public float xCoFactor;
    public float upPower = 20f;
    public float forwardPower = 100f;
    private bool isInAir = false; // for second phase 2

    void Start(){
       gameplayManager = FindObjectOfType<GameplayManager>();
        ground = GameObject.FindGameObjectWithTag("ground").transform;
       lr = GetComponent<LineRenderer>();
       rb = GetComponent<Rigidbody>();
       camera = Camera.main;
       EndLine();
    }
    void Update() {

        if(transform.position.y < ground.position.y)
        {
            transform.position = new Vector3(transform.position.x, ground.position.y, transform.position.z);
        }

        #region Phase_1
        if (gameplayManager.currentPhase == 1 && gameplayManager.guliReadyForFirstHit == true){

            if(Input.GetMouseButtonDown(0)){
                startPoint  = Input.mousePosition;
            }
            if(Input.GetMouseButton(0)){
                Vector3 currentPoint = Input.mousePosition;
            
                Vector3 endPoint2 = currentPoint;
                endPoint2.x = (startPoint.x - currentPoint.x) / (Screen.width/2);
                DrawLine(transform.position, endPoint2);

            }
            if(Input.GetMouseButtonUp(0)){
                endPoint = Input.mousePosition;
                power = gameplayManager.powerMeterValue;
                float xDirection = ((startPoint.x - endPoint.x) / (Screen.width/2)) * (-1 * minMaxX);
                float yDirection = maxY * power;
                float zDirection = zCoFactor * maxZ * power;
                Vector3 direction  = new Vector3(xDirection, yDirection, zDirection);
                
                rb.isKinematic = false;
                rb.AddForce(direction, ForceMode.Impulse);
                gameplayManager.ActiveGuliCamera();
                
                EndLine();
                gameplayManager.guliFirstHitDone = true;
                gameplayManager.timeLeft = 10f;
            }

        }
        #endregion

        #region Phase_2
        if (gameplayManager.currentPhase == 2){
            if(gameplayManager.readyForSecondHit == true )
            {
                gameplayManager.setHitterForSecondPhase = false;
                if (gameplayManager.secondHitDone == false)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        rb.AddForce(Vector3.up * upPower);
                        gameplayManager.powerMeterOn = true;
                        
                        touchCount += 1;
                        isInAir = true;
                        gameplayManager.secondHitDone = true;
                    }
                }
                else
                {
                    if(gameplayManager.secondSwingDone == false)
                    {
                        if (isInAir == true)
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                startPos = Input.mousePosition;
                                touchCount += 1;

                            }

                            if (Input.GetMouseButtonUp(0) && touchCount >= 2)
                            {
                                gameplayManager.secondSwingDone = true;
                                Vector3 endPos = Input.mousePosition;
                                power = gameplayManager.powerMeterValue;
                                
                                float xDirection = ((startPos.x - endPos.x) / (Screen.width / 2)) * (minMaxX);
                                float yDirection = 10f * power;
                                float zDirection = zCoFactor * maxZ * power;
                                Vector3 direction = new Vector3(xDirection, yDirection, zDirection);
                               
                                rb.isKinematic = false;
                                rb.AddForce(direction, ForceMode.Impulse);
                                
                                gameplayManager.checkGuliDistance = true;
                                gameplayManager.HitterResetToIdle();
                            }
                        }
                    }
                }
  
            }
            
        }
        #endregion

    }

    void OnCollisionEnter(Collision col){
        Debug.Log(col.gameObject.name);
        if(gameplayManager.currentPhase == 1){
            
            if (col.gameObject.tag == "dangerZone"){
                gameplayManager.DangerAreaOut();
              //  rb.isKinematic = true;
            }

            if(col.gameObject.tag == "ground"){
               // rb.isKinematic = true;
                gameplayManager.checkFielderDistance = true;
                
                gameplayManager.ActiveHitterCamera();
                gameplayManager.timeLeft = 10f;
            }
        }

        if(gameplayManager.currentPhase == 2)
        {
            if (col.gameObject.tag == "ground")
            {
                touchCount = 0;
                if (gameplayManager.secondHitDone == true && gameplayManager.secondSwingDone == false)
                {
                    gameplayManager.lifeCount -= 1;
                    gameplayManager.ResetDataForSecondPhase();
                }
                else
                {
                    if (isInAir == true)
                    {
                        gameplayManager.secondPhaseEnd = true;
                        isInAir = false;
                    }
                }
                
            }
        }
    }

    void DrawLine(Vector3 startPoint, Vector3 cPoint){
        lr.positionCount = 2;
        cPoint.y = transform.position.y;
        cPoint.z = -1f;

        Vector3[] allPoint = new Vector3[2];
        allPoint[0]=transform.position;
        allPoint[1]=cPoint;
        lr.SetPositions(allPoint);
    }

    void EndLine(){
        lr.positionCount = 0;
    }
    
}

