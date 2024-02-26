using System;
using System.Collections;
using UnityEngine;

public class DeplacementBalle : MonoBehaviour
{
    public float vitesseRotation = 100.0f;
    public event Action BalleTiree;
    public event Action BalleArretee;
    public event Action BalleSortie;
    private Rigidbody rb;
    private bool balleFrappe = false; 
    private bool verificationArretActive = false;
    private Quaternion rotationInitiale;
    public bool switchDePiste = false;
    //public Vector3 checkpoint;
    public string piste;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rotationInitiale = transform.rotation;
        //checkpoint = transform.position;
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        transform.Rotate(Vector3.up, horizontalInput * vitesseRotation * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            BalleTiree?.Invoke();
            balleFrappe = true;
            verificationArretActive = false; 
        }
        
        if (balleFrappe && !verificationArretActive && rb.velocity.magnitude < 0.1f)
        {
            verificationArretActive = true;
            StartCoroutine(attendreAvantDeclencher());   
        }

        if(transform.position.y < -1)
        {
            print("Balle sortie");
            BalleSortie?.Invoke();
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
        //transform.position = checkpoint;
    }

    


}
