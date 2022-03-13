using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    public bool hit = false;
    public int upPower = 5;
    public float xCofactor;
    Rigidbody rb;
    bool isInAir = false;
    bool swipe = false;
    Vector3 startPos;

    int touchCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit == true)
        {

            if (Input.GetMouseButtonUp(0))
            {

                rb.AddForce(Vector3.up * upPower);
               
                Time.timeScale = 0.25f;
                touchCount += 1;
                isInAir = true;
            }

            if (isInAir == true)
            {
               
                if (Input.GetMouseButtonDown(0))
                {
                    startPos = Input.mousePosition;
                    touchCount += 1;

                }
               
                if (Input.GetMouseButtonUp(0) && touchCount >= 2)
                 {

                     Vector3 endPos = Input.mousePosition;
                     Debug.Log(endPos.x);
                     Debug.Log(endPos.x - startPos.x);
                     float xDirection = ((endPos.x - startPos.x) / (Screen.width / 2)) * (xCofactor);
                     Debug.Log(xDirection);
                     float yDirection = 20f;
                     float zDirection = 100f;
                     Vector3 direction = new Vector3(xDirection, yDirection, zDirection);
                     rb.isKinematic = false;
                     rb.AddForce(direction);

                     swipe = true;

                 }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.tag == "ground")
        {
            if (isInAir == true)
            {
                Time.timeScale = 1f;
                isInAir = false;
                swipe = false;
                touchCount = 0;
            }
        }
        
    }
}
