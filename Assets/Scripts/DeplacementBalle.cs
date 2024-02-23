using System;
using System.Collections;
using UnityEngine;

public class DeplacementBalle : MonoBehaviour
{
    public float vitesseRotation = 100.0f;
    public event Action BalleTiree;
    public event Action BalleArretee;
    private Rigidbody rb;
    private bool balleFrappe = false;
    //private float tempsDepuisFrappe = 0f; 
    private bool verificationArretActive = false;
    private Quaternion rotationInitiale;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotationInitiale = transform.rotation;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * vitesseRotation * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BalleTiree?.Invoke();
            balleFrappe = true;
            //tempsDepuisFrappe = 0f; 
            verificationArretActive = false; 
        }

        /*
        if (balleFrappe)
        {
            tempsDepuisFrappe += Time.deltaTime;
            if (tempsDepuisFrappe > 1f) 
            {
                verificationArretActive = true;
            }
        }*/

        
        if (balleFrappe && !verificationArretActive && rb.velocity.magnitude < 0.1f)
        {
            verificationArretActive = true;
            
            StartCoroutine(attendreAvantDeclencher());
            
        }
    }

    IEnumerator attendreAvantDeclencher()
    {
        yield return new WaitForSeconds(3f);
        BalleArretee?.Invoke();
        balleFrappe = false;
        verificationArretActive = false;
    }

    public void reinitialiserBalleFrappe()
    {
        balleFrappe = false;
        verificationArretActive = false;
        transform.rotation = rotationInitiale;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;

    }

}
