using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrou : MonoBehaviour
{
    // L'action qui déclenche que la balle est entré dans le trou
    public event Action balleEntree;
    // Le game object de la balle.
    [SerializeField] private GameObject balleActive;

    // On fait le OnTriggerEnter pour le trou
    private void OnTriggerEnter(Collider other)
    {
        // Si l'objet qui rentre dans la zone du trou est la zone active,
        // on déclenche l'événement : Balle entrée pour le game manager
        if (other.gameObject == balleActive)
        {
            balleEntree?.Invoke();
        }
    }

    
}
