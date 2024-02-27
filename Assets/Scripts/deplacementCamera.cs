using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deplacementCamera : MonoBehaviour
{
    [SerializeField] private GameObject balle; // la balle
    [SerializeField] private Vector3 distance; // la distance entre la balle et la caméra quand la caméra est derrière
    [SerializeField] private float vitesseRotation; // vitesse de rotation de la caméra
    private float VitesseZoom = 0.1f;  // vitesse de zoom
    private float minZoom = 0.6f; // zoom minimum
    private float maxZoom = 5.0f; // zoom maximum
    private float leZoom; // le zoom courant


    private void Start()
    {
        // au debut du jeu, on initialise le zoom courant au zoom minimum
        leZoom = minZoom;
    }

    void Update()
    {
        
        leZoom -= Input.GetAxis("Vertical") * VitesseZoom;

        // si le zoom actuel est plus petit que le zoom minimum, on met le zoom actuel comme minimum 
        // et vice-versa avec le zoom maximum
        // Cela permet de controler les zooms
        if (leZoom < minZoom)
        {
            leZoom = minZoom;
        }
        else if (leZoom > maxZoom)
        {
            leZoom = maxZoom;
        }

        // on ajuste la position de la camera selon la balle, elle suit toujours le forward de la balle
        // + un vecteur qui leve la camera un peu du sol pour une meilleur vision sur le jeu
        transform.position = balle.transform.position - balle.transform.forward * leZoom + new Vector3(0,0.3f,0) * leZoom;
       
        // La caméra regarde la balle pour qu'elle soit l'objet principal du jeu
        transform.LookAt(balle.transform.position);
    }
}



