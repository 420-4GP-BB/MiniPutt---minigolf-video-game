using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptRotation : MonoBehaviour
{
    public float rotationSpeed = 100.0f;

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * rotationSpeed * Time.deltaTime);
    }
}
