using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrou : MonoBehaviour
{
    public event Action balleEntree;
    [SerializeField] private GameObject balleActive;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == balleActive)
        {
            balleEntree?.Invoke();
            print("Balle rentrée");
        }
    }

    
}
