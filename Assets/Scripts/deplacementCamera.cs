using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deplacementCamera : MonoBehaviour
{
    [SerializeField] private GameObject balle;
    [SerializeField] private Vector3 distance;
    [SerializeField] private float vitesseRotation; 
    private float VitesseZoom = 0.1f; 
    private float minZoom = 0.6f;
    private float maxZoom = 10.0f; 
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

        
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            distance = Quaternion.AngleAxis(horizontalInput * vitesseRotation * Time.deltaTime, Vector3.up) * distance;
        }

        
        Vector3 nouvellePosition = balle.transform.position + distance.normalized * leZoom;
        transform.position = nouvellePosition;

        
        transform.LookAt(balle.transform.position);
        
    }
}



