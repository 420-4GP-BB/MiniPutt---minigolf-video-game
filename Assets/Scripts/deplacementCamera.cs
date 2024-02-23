using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deplacementCamera : MonoBehaviour
{
    [SerializeField] private GameObject balle;
    [SerializeField] private Vector3 distance;
    [SerializeField] private float vitesseRotation;
    private Vector3 forwardInitial;
    private float VitesseZoom = 0.1f; 
    private float minZoom = 0.6f;
    private float maxZoom = 5.0f; 
    private float leZoom; 


    private void Start()
    {
        leZoom = minZoom;
    }

    void Update()
    {
        
        leZoom -= Input.GetAxis("Vertical") * VitesseZoom;

        
        if (leZoom < minZoom)
        {
            leZoom = minZoom;
        }
        else if (leZoom > maxZoom)
        {
            leZoom = maxZoom;
        }


        transform.position = balle.transform.position - balle.transform.forward * leZoom + new Vector3(0,0.3f,0);
       
        transform.LookAt(balle.transform.position);
    }
}



