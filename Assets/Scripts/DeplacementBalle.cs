using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeplacementBalle : MonoBehaviour
{
    public float rotationSpeed = 100.0f;
    public event Action BalleTiree;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.Space))
        {
            BalleTiree?.Invoke();
        }
    }
}
