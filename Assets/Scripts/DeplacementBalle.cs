using System;
using System.Collections;
using UnityEngine;

public class DeplacementBalle : MonoBehaviour
{
    public float vitesseRotation = 100.0f; // vitesse de rotation de la balle
    public event Action BalleTiree; // L'action qui sera d�clench� lorsque la balle a �t� tir�
    public event Action BalleArretee; // L'Action qui sera d�clench� lorsque la balle s'est arr�t�
    public event Action BalleSortie; // L'action qui sera d�clench� lorsque la balle sort d'une des pistes 
    private Rigidbody rb; // le rigidbody de la balle
    private bool balleFrappe = false; // Un booleen qui verifie si la balle a �t� frappe ou non
    private bool verificationArretActive = false;
    private Quaternion rotationInitiale; // Un quaternion qui nous indique la rotation initiale de la balle (son forward initial)
    public bool piste3; // un booleen qui nous dit si la balle est dans la piste 3
    private bool estArreteeSurPente = false; // un booleen qui confirme si la balle s'est arrete sur la pente inclin�e de la piste3
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        // on associe la rotation � la rotation de la balle au d�but, ce qui va permettre de sauvegarder son forward
        rotationInitiale = transform.rotation;
    }

    void Update()
    {
        // Si on appuie sur A, D, fl�che gauche, fl�che droite. L'axe de l'input est horizontal
        float horizontalInput = Input.GetAxis("Horizontal");
        // on rotate la balle selon le input
        transform.Rotate(Vector3.up, horizontalInput * vitesseRotation * Time.deltaTime);

        // Si on appuie sur espace, on d�clenche l'action de balle tir�e, et on met balle frapp� � true
        if (Input.GetKeyDown(KeyCode.Space))
        {
            BalleTiree?.Invoke();
            balleFrappe = true;
            verificationArretActive = false; 
        }

        // si la balle est arrete, on attend quelques secondes pour confirmer son arret
        if (balleFrappe && !verificationArretActive && rb.velocity.magnitude < 0.1f)
        {
            verificationArretActive = true;
            StartCoroutine(attendreAvantDeclencher());
        }


        //Si le y de la balle est plus petit que -2, cela veut dire que la balle est sortie des pistes
        // On utilise un y plus grand que -2 pour faire entrer la balle dans le trou
        if (transform.position.y < -2)
        {
            print("Balle sortie");
            BalleSortie?.Invoke();
        }
    }

    /*
    IEnumerator attendreJusquaSaDescente()
    {
        bool balleArreteDef = false;
        

        while (balleArreteDef == false)
        {
            yield return new WaitForSeconds(3.0f);
            print("ON entre dans la boucle");
            if (rb.velocity.magnitude < 0.1f && transform.position.y == 0.27)
            {
                print("ON entre dans la condition");
                balleArreteDef = true;
                verificationArretActive = true;
                break;
            }

        }
        
    }
    */

    // Une coroutine qui attend quelques secondes avant de d�clencher que la balle s'est arr�t�e. 
    // On utilise cette coroutine pour confirmer que la balle s'est belle et bien arr�t�e
    IEnumerator attendreAvantDeclencher()
    {
        yield return new WaitForSeconds(3f);
        BalleArretee?.Invoke();
        balleFrappe = false;
        verificationArretActive = false;
    }

    // On r�initialise la balle frapp� commme elle �tait au d�but
    public void reinitialiserBalleFrappe()
    {
        balleFrappe = false;
        verificationArretActive = false;
        transform.rotation = rotationInitiale;
        rb.angularVelocity = Vector3.zero;
        rb.velocity = Vector3.zero;
    }

    private bool EstSurPenteEtArretee()
    {
        float hauteurFinPente = -2.3f;
        float hauteurDebutPente = -1.5f;
        return transform.position.y >= hauteurDebutPente && transform.position.y <= hauteurFinPente && rb.velocity.magnitude < 0.1f;
    }
}
