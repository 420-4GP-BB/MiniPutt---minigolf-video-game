using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detecterBallesSortie : MonoBehaviour
{
    //public event Action balleEntree;
    [SerializeField] private GameObject balleActive;

    // Update is called once per frame
    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == balleActive)
        {
            print("Balle touche le sol");
        }
    }

}
