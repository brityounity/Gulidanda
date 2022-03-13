using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterAnimationDisable : MonoBehaviour
{
     
    Animator animatorController;
    public void Start()
    {
        animatorController = GetComponent<Animator>();
    }
    public void IsActive()
    {
        this.gameObject.SetActive(false);
    }
}
