using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTrou : MonoBehaviour
{
    // L'action qui d�clenche que la balle est entr� dans le trou
    public event Action balleEntree;
    // Le game object de la balle.
    [SerializeField] private GameObject balleActive;

    // On fait le OnTriggerEnter pour le trou
    private void OnTriggerEnter(Collider other)
    {
        // Si l'objet qui rentre dans la zone du trou est la zone active,
        // on d�clenche l'�v�nement : Balle entr�e pour le game manager
        if (other.gameObject == balleActive)
        {
            balleEntree?.Invoke();
        }
    }

    
}
