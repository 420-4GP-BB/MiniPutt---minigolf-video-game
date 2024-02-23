using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] lesPosDepart;
    [SerializeField] GameObject[] posCamera;
    [SerializeField] GameObject[] lesTrous;
    [SerializeField] GameObject[] lesPistes;
    [SerializeField] Camera laCamera;
    [SerializeField] GameObject balle;
    private GameObject trouCourant;
    private GameObject pisteCourante;
    private GameObject posCameracourante;
    private bool jeuCommence;
    [SerializeField]  private AnimationDebut anime;
    private DeplacementBalle dep;
    private Rigidbody rb;
    private int numPiste = 0;
    private int compteur= 0;

    // Start is called before the first frame update
    void Start()
    {
        
        placerCameraAnimationDebut();
        anime.finAnimation += debuterJeu;
        
        balle.GetComponent<DeplacementBalle>().BalleTiree += traiterBalleTiree;
        balle.GetComponent<DeplacementBalle>().BalleArretee += replacerCameraDerriereJoueur;
        
        
        lesTrous[0].GetComponent<CollisionTrou>().balleEntree += changerDePiste;
        lesTrous[1].GetComponent<CollisionTrou>().balleEntree += changerDePiste;
        lesTrous[2].GetComponent<CollisionTrou>().balleEntree += changerDePiste;

        //balleForward = balle;
        // Quand on change de piste on ajoute de 1


    }

    // Update is called once per frame
    

    void Update()
    {
        faireTouches();
    }

    //Balle arrêtée, Vitesse presque 0, on augmente le compteur. 
    // retourner la caméra


    void traiterBalleEntree()
    {
        // si la balle est entrée d'un seul coup on affiche un message

        // faire la coroutinequi entre la balle dans le trou

       
        changerDePiste();
    }
    void changerDePiste()
    {
        
        print("Balle entrée");
        if (pisteCourante == lesPistes[0])
        {
            versPiste(2);
        }else if (pisteCourante == lesPistes[1])
        {
            versPiste(3);
        }
        else
        {
            // Vers la fin du jeu
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
        rb = balle.GetComponent<Rigidbody>();
        float force = leCube.transform.localScale.y * 350;
        rb.AddForce(balle.transform.forward * force);
    }

    void debuterJeu()
    {
        anime.gameObject.SetActive(false);
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
        laCamera.GetComponent<deplacementCamera>().enabled=true;
        compteur += 1;

        balle.GetComponent<DeplacementBalle>().reinitialiserBalleFrappe();
        //balle.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        print(compteur);
        print(numPiste);
    }

    void faireTouches()
    {
       if (jeuCommence == true)
        {
            if (Input.GetKeyDown("1")){
                versPiste(1);

            }else if (Input.GetKeyDown("2"))
            {
                versPiste(2);
            }else if (Input.GetKeyDown("3"))
            {
                versPiste(3);
            }else if (Input.GetKeyDown("9"))
            {
                
            }else if (Input.GetKeyDown("0"))
            {
                float angle = Random.Range(0, 2 * Mathf.PI);
                float rayon = trouCourant.GetComponent<SphereCollider>().radius;
                float distanceDuTrou = 2 * rayon;
                float x = Mathf.Cos(angle) * distanceDuTrou;
                float z = Mathf.Sin(angle) * distanceDuTrou;
                Vector3 nouvellePos = trouCourant.transform.position + new Vector3(x,0.5f, z);
                balle.transform.position = nouvellePos;
            }
        }
    }

    void versPiste(int num)
    {
        laCamera.GetComponent<deplacementCamera>().enabled = true;
        balle.transform.position = lesPosDepart[num-1].transform.position;
        posCameracourante = posCamera[num - 1];
        trouCourant = lesTrous[(num-1)];
        pisteCourante = lesPistes[(num-1)];
        numPiste = num;
        compteur = 0;
    }


}
