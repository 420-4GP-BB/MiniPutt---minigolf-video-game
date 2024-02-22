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
    DeplacementBalle dep;

    // Start is called before the first frame update
    void Start()
    {
        
        placerCameraAnimationDebut();
        anime.finAnimation += debuterJeu;

        dep = balle.GetComponent<DeplacementBalle>();
        dep.BalleTiree += deplacerCameraAprTir;


    }

    // Update is called once per frame
    

    void Update()
    {
        faireTouches();
    }

    void deplacerCameraAprTir()
    {
        print("Espace appuyé");
        laCamera.GetComponent<deplacementCamera>().enabled = false;
        laCamera.transform.position = posCameracourante.transform.position;
        laCamera.transform.rotation = posCameracourante.transform.rotation;
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

                //balle.GetComponent<Rigidbody>().position = nouvellePos;
                balle.transform.position = nouvellePos;


            }
        }
    }

    void versPiste(int num)
    {
        laCamera.GetComponent<deplacementCamera>().enabled = true;
        balle.transform.position = lesPosDepart[num-1].transform.position;
        //laCamera.transform.position = posCamera[num-1].transform.position;
        //laCamera.transform.rotation = posCamera[num-1].transform.rotation;
        posCameracourante = posCamera[num - 1];
        trouCourant = lesTrous[(num-1)];
        pisteCourante = lesPistes[(num-1)];
    }
}
