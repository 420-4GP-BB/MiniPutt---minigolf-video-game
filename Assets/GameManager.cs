using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] lesPosDepart;
    [SerializeField] GameObject[] posCamera;
    [SerializeField] GameObject[] lesTrous;
    [SerializeField] GameObject[] lesPistes;
    [SerializeField] Camera laCamera;
    [SerializeField] GameObject balle;
    [SerializeField] GameObject gestionnaire;
    private GameObject trouCourant;
    private GameObject pisteCourante;
    private GameObject posCameracourante;
    private bool jeuCommence = false;
    private Rigidbody rb;
    private int numPiste = 0;
    private int compteur = 0;
    private int[] lesScores = new int[3];
    private Vector3 checkpoint;


    void Start()
    {
        placerCameraAnimationDebut();

        rb = balle.GetComponent<Rigidbody>();
        placerCameraAnimationDebut();
        //anime.finAnimation += debuterJeu;

        balle.GetComponent<DeplacementBalle>().BalleTiree += traiterBalleTiree;
        balle.GetComponent<DeplacementBalle>().BalleArretee += replacerCameraDerriereJoueur;
        balle.GetComponent<DeplacementBalle>().BalleSortie += traiterBalleSortie;


        lesTrous[0].GetComponent<CollisionTrou>().balleEntree += traiterBalleEntree;
        lesTrous[1].GetComponent<CollisionTrou>().balleEntree += traiterBalleEntree;
        lesTrous[2].GetComponent<CollisionTrou>().balleEntree += traiterBalleEntree;
    }

    // on peut pas monter la piste 3
    // ajouter la force 
    // régler le bug après avoir entré dans la piste 2 (caméra est bizarre)

    void Update()
    {
        if (!jeuCommence && Input.anyKeyDown)
        {
            gestionnaire.GetComponent<GestionAnim>().arreterAnimationDebut();
            gestionnaire.GetComponent<GestionAnim>().animDebut.gameObject.SetActive(false);
            debuterJeu();
        }

        faireTouches();
    }


    void traiterBalleEntree()
    {

        // faire la coroutinequi entre la balle dans le trou

        balle.GetComponent<DeplacementBalle>().switchDePiste = true;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        StartCoroutine(animationBalleEntree());


        changerDePiste();
    }


    void traiterBalleSortie()
    {
        print("Balle sortie TRAITER");
        incrementerCompteur();
        StartCoroutine(coroutineBalleSortie());
        gestionnaire.GetComponent<GestionAnim>().commencerUneAnimation(2);
        laCamera.GetComponent<deplacementCamera>().enabled = true;
        balle.transform.Find("Cube").gameObject.SetActive(true);
        balle.transform.Find("crosshair").gameObject.SetActive(true);
        balle.transform.position = checkpoint;
        if (compteur < 6)
        {
            traiterBalleCoteMuret();
            balle.GetComponent<DeplacementBalle>().reinitialiserBalleFrappe();
        }

    }

    IEnumerator coroutineBalleSortie()
    {
        yield return new WaitForSeconds(0.5f);
    }



    IEnumerator animationBalleEntree()
    {
        yield return new WaitForSeconds(2f);
    }



    void changerDePiste()
    {

        if (pisteCourante == lesPistes[0])
        {
            lesScores[0] = compteur;
            versPiste(2);
        }
        else if (pisteCourante == lesPistes[1])
        {
            lesScores[1] = compteur;
            versPiste(3);
        }
        else
        {
            lesScores[2] = compteur;
            placerCameraAnimationDebut();
            gestionnaire.GetComponent<GestionAnim>().commencerAnimFin(lesScores[0], lesScores[1], lesScores[2]);
        }
    }

    void deplacerCameraAprTir()
    {
        print("Espace appuyé");
        laCamera.GetComponent<deplacementCamera>().enabled = false;
        laCamera.transform.position = posCameracourante.transform.position;
        laCamera.transform.rotation = posCameracourante.transform.rotation;
    }

    void traiterBalleTiree()
    {
        GameObject leCube = balle.transform.Find("Cube").gameObject;
        GameObject crosshair = balle.transform.Find("crosshair").gameObject;
        deplacerCameraAprTir();
        leCube.gameObject.SetActive(false);
        crosshair.SetActive(false);

        float force = leCube.transform.localScale.y * 300;
        rb.AddForce(balle.transform.forward * force);

        checkpoint = balle.transform.position;

        incrementerCompteur();

    }

    void debuterJeu()
    {
        gestionnaire.GetComponent<GestionAnim>().activerAnimationPisteEtCoups();
        balle.SetActive(true);
        versPiste(1);
        jeuCommence = true;
    }

    void placerCameraAnimationDebut()
    {
        laCamera.transform.position = posCamera[3].transform.position;
        laCamera.transform.rotation = posCamera[3].transform.rotation;
        laCamera.GetComponent<deplacementCamera>().enabled = false;
        balle.SetActive(false);
        jeuCommence = false;
    }

    void replacerCameraDerriereJoueur()
    {
        balle.transform.Find("Cube").gameObject.SetActive(true);
        balle.transform.Find("crosshair").gameObject.SetActive(true);
        laCamera.GetComponent<deplacementCamera>().enabled = true;
        if (compteur < 6)
        {
            traiterBalleCoteMuret();
            balle.GetComponent<DeplacementBalle>().reinitialiserBalleFrappe();
        }
        else
        {
            changerDePiste();
            gestionnaire.GetComponent<GestionAnim>().commencerUneAnimation(3);

        }

    }

    void traiterBalleCoteMuret()
    {
        float rayon = balle.GetComponent<SphereCollider>().radius;
        GameObject[] murs = GameObject.FindGameObjectsWithTag("Mur");

        foreach (GameObject mur in murs)
        {
            Collider murCol = mur.GetComponent<Collider>();
            Vector3 pointProche = murCol.ClosestPoint(balle.transform.position);
            float distance = (pointProche - balle.transform.position).magnitude;

            if (distance < rayon * 2)
            {
                Vector3 directionDeplacement = (balle.transform.position - pointProche).normalized;
                balle.transform.position = pointProche + directionDeplacement * (rayon * 2);
            }
        }

    }

    void faireTouches()
    {
        if (jeuCommence == true)
        {
            if (Input.GetKeyDown("1"))
            {
                versPiste(1);

            }
            else if (Input.GetKeyDown("2"))
            {
                versPiste(2);
            }
            else if (Input.GetKeyDown("3"))
            {
                versPiste(3);
            }
            else if (Input.GetKeyDown("9"))
            {
                traiterBalleSortie();
            }
            else if (Input.GetKeyDown("0"))
            {
                float angle = Random.Range(0, 2 * Mathf.PI);
                float rayon = trouCourant.GetComponent<SphereCollider>().radius;
                float distanceDuTrou = 2 * rayon;
                float x = Mathf.Cos(angle) * distanceDuTrou;
                float z = Mathf.Sin(angle) * distanceDuTrou;
                Vector3 nouvellePos = trouCourant.transform.position + new Vector3(x, 0.5f, z);
                balle.transform.position = nouvellePos;
            }
        }
    }

    void versPiste(int num)
    {
        if (compteur == 1)
        {
            gestionnaire.GetComponent<GestionAnim>().commencerUneAnimation(1);
        }
        laCamera.GetComponent<deplacementCamera>().enabled = true;
        balle.transform.position = lesPosDepart[num - 1].transform.position;
        checkpoint = balle.transform.position;
        posCameracourante = posCamera[num - 1];
        trouCourant = lesTrous[(num - 1)];
        pisteCourante = lesPistes[(num - 1)];
        numPiste = num;
        compteur = 0;
        gestionnaire.GetComponent<GestionAnim>().MettreAJourAnimationPiste(numPiste, compteur);
    }

    void incrementerCompteur()
    {
        compteur++;
        gestionnaire.GetComponent<GestionAnim>().MettreAJourAnimationPiste(numPiste, compteur);
    }

}
