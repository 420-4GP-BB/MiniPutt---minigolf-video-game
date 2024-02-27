using System.Collections;
using System.Collections.Generic;
using System.Net;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

// Le script qui est associé au game manager
public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] lesPosDepart; // Les positions de départ de la balle dans les pistes 1,2,3
    [SerializeField] GameObject[] posCamera; // Les positions de caméra après le tir de la balle dans les pistes 1,2,3
    [SerializeField] GameObject[] lesTrous; // Les trous dans les pistes 1,2,3
    [SerializeField] GameObject[] lesPistes; // les pistes 1,2,3
    [SerializeField] Camera laCamera; // La caméra principale
    [SerializeField] GameObject balle; // La balle
    [SerializeField] GameObject gestionnaire; // Le gestionnaire des animations
    private GameObject trouCourant; // Le trou courant dans la piste
    private GameObject pisteCourante; // la piste courante 
    private GameObject posCameracourante; // la position de la caméra courante après le tir de la balle dans une piste
    private bool jeuCommence = false; // le booleen qui dit si le jeu a commencé ou pas
    private Rigidbody rb; // le rigidbody de la balle
    private int numPiste = 0; // Le numero de piste
    private int compteur = 0; // le compteur est le nombre de coups
    private int[] lesScores = new int[3]; // un tableau qui enregistre le score total des pistes
    private Vector3 checkpoint; // Un checkpoint pour sauvegarder la position de la balle avant tir

    // LA methode Start
    void Start()
    {
        // on place la camera à sa position au début de l'animation du début
        placerCameraAnimationDebut();

        rb = balle.GetComponent<Rigidbody>();

        // L'événement balle tirée appelle la methode qui traite cette derniere
        balle.GetComponent<DeplacementBalle>().BalleTiree += traiterBalleTiree;
        // L'événement balle arrêtée appelle la methode qui traite cette derniere, qui est la methode qui replace la camera derriere le joueur
        balle.GetComponent<DeplacementBalle>().BalleArretee += replacerCameraDerriereJoueur;
        // L'événement balle sortie appelle la methode qui traite cette derniere
        balle.GetComponent<DeplacementBalle>().BalleSortie += traiterBalleSortie;

        // L'événement Balle entrée qui appelle la methode qui traite cette derniere
        lesTrous[0].GetComponent<CollisionTrou>().balleEntree += traiterBalleEntree;
        lesTrous[1].GetComponent<CollisionTrou>().balleEntree += traiterBalleEntree;
        lesTrous[2].GetComponent<CollisionTrou>().balleEntree += traiterBalleEntree;

        // L'evenement fin du jeu appelle la methode qui gere ce dernier
        gestionnaire.GetComponent<GestionAnim>().finJeu += procedureFinJeu;
    }


    void Update()
    {
        // Au début du jeu, si le joueur clique sur n'importe quelle bouton, on peut commencer le jeu
        if (!jeuCommence && Input.anyKeyDown)
        {
            // on arrete l'animation du debut
            gestionnaire.GetComponent<GestionAnim>().arreterAnimationDebut();
            // appeler la methode qui debute le jeu
            debuterJeu();
        }

        // faire les touches des raccourcis
        faireTouches();
    }

    // la methode qui traite la balle entree dans la piste
    void traiterBalleEntree()
    {
        // on arrete la vitesse de la balle et elle devient kinematic pour qu'on puisse la controler
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        balle.GetComponent<Rigidbody>().isKinematic = true;
        // on appelle la coroutine qui anime l'entrée de la balle dans le trou
        //StartCoroutine(animationBalleEntree());
        // on change de piste pour aller a la prochaine
        changerDePiste();
    }

    // la coroutine qui entre la balle dans le trou
    IEnumerator animationBalleEntree()
    {
        // si la position de la balle en y est plus grande que -1, on la descend verticalement 
        while (transform.position.y > -1f)
        {
            balle.transform.position += Vector3.down * Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
    }

    // LA methode qui traite la sortie de la balle
    void traiterBalleSortie()
    {
        // on incremente le compteur
        incrementerCompteur();
        // on appelle la coroutine de la balle sortie qui attend quelques secondes
        StartCoroutine(coroutineBalleSortie());
        // on lance l'animation quand la balle sort
        gestionnaire.GetComponent<GestionAnim>().commencerUneAnimation(2);
        
        // si la balle a ete tirée moins que 6 fois 
        if (compteur < 6)
        {
            // on traite la camera si elle est a coté du mur
            traiterBalleCoteMuret();
            // on appelle la methode qui reinitialise la balle pour un nouveau tir
            balle.GetComponent<DeplacementBalle>().reinitialiserBalleFrappe();
            // On remet la camera a sa position derriere la balle et on active le rectangle et le aim
            laCamera.GetComponent<deplacementCamera>().enabled = true;
            balle.transform.Find("Cube").gameObject.SetActive(true);
            balle.transform.Find("crosshair").gameObject.SetActive(true);
            // la balle revient à sa derniere position avant le tir (checkpoint)
            balle.transform.position = checkpoint;
        }

    }

    //la coroutine de la balle sortie qui attend juste 1s 
    IEnumerator coroutineBalleSortie()
    {
        yield return new WaitForSeconds(1f);
    }

    // La methode qui change de piste à la balle
    void changerDePiste()
    {
        
        balle.GetComponent<Rigidbody>().isKinematic = false;
        if (pisteCourante == lesPistes[0])
        {
            // si on est à la premiere piste, on va a la 2eme, et on sauvegarde le score de coups de la 1ere piste dans le tableau
            lesScores[0] = compteur;
            versPiste(2);
        }
        else if (pisteCourante == lesPistes[1])
        {
            // la meme chose avec la 2eme piste
            lesScores[1] = compteur;
            versPiste(3);
        }
        else
        {
            // si on est a la troisieme piste, on y va directement a la fin du jeu
            lesScores[2] = compteur;
            // en placant la camera à sa position au début du jeu 
            placerCameraAnimationDebut();
            gestionnaire.GetComponent<GestionAnim>().commencerAnimFin(lesScores[0], lesScores[1], lesScores[2]);
        }
    }

    // la methode qui place la camera selon sa position apres tir selon la piste
    void deplacerCameraAprTir()
    {
        laCamera.GetComponent<deplacementCamera>().enabled = false;
        laCamera.transform.position = posCameracourante.transform.position;
        laCamera.transform.rotation = posCameracourante.transform.rotation;
    }

    // la methode qui traite la balle quand elle est tirée
    void traiterBalleTiree()
    {
        GameObject leCube = balle.transform.Find("Cube").gameObject;
        GameObject crosshair = balle.transform.Find("crosshair").gameObject;
        // on appelle la methode qui deplace la position de la camera
        deplacerCameraAprTir();
        // on desactive le rectangle et le aim
        leCube.gameObject.SetActive(false);
        crosshair.SetActive(false);

        // on calcule la force qui est la grandeur du cube multiplie par une constante
        float force = leCube.transform.localScale.y * 330;
        // on applique la force a la balle
        rb.AddForce(balle.transform.forward * force);
        // on sauvegarde cette nouvelle position
        checkpoint = balle.transform.position;
        // on incremente le compteur
        incrementerCompteur();

    }

    // la methode qui debute le jeu
    void debuterJeu()
    {
        // quand le jeu commence, on active l'animation des numeros de pistes et coups
        gestionnaire.GetComponent<GestionAnim>().activerAnimationPisteEtCoups();
        // on active la balle
        balle.SetActive(true);
        balle.transform.Find("Cube").gameObject.SetActive(true);
        balle.transform.Find("crosshair").gameObject.SetActive(true);
        balle.transform.rotation = balle.GetComponent<DeplacementBalle>().rotationInitiale;

        // on commence a la premiere piste
        versPiste(1);
        // le jeu a commence
        jeuCommence = true;
        // on met le compteur a 0 et la piste 
        compteur = 0;
        
    }

    // la methode qui place la camera a la position du debut de l'animation du début

    void placerCameraAnimationDebut()
    {
        laCamera.transform.position = posCamera[3].transform.position;
        laCamera.transform.rotation = posCamera[3].transform.rotation;
        // desactiver le script de la camera qui la relie avec la balle
        laCamera.GetComponent<deplacementCamera>().enabled = false;
        // desactiver la balle
        balle.SetActive(false);
        // le jeu n'a pas encore commence
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
    
    // la methode qui desactive toutes les animations
    void desactiverToutesAnimations()
    {
        gestionnaire.GetComponent<GestionAnim>().animDebut.gameObject.SetActive(false);

        foreach(TextMeshProUGUI animation in gestionnaire.GetComponent<GestionAnim>().autresAnim)
        {
            animation.gameObject.SetActive(false);
        }

        foreach(TextMeshProUGUI animation in gestionnaire.GetComponent<GestionAnim>().animFin)
        {
            animation.gameObject.SetActive(false);
        }
        // appele la methode qui attend avant de redébuter le jeu
        StartCoroutine(attenteAvantRecommencer());

    }

    // une coroutine qui fait que attendre avant de recommencer le jeu
    IEnumerator attenteAvantRecommencer()
    {
        yield return new WaitForSeconds(3.0f);
    }

    // la methode qui met fin au jeu
    void procedureFinJeu()
    {
        // on desactive toutes les animations
        desactiverToutesAnimations();
        // et on recommence le jeu
        debuterJeu();
    }

    
        
    // methode qui traite la balle quand elle est a cote du muret
    void traiterBalleCoteMuret()
    {
        // le rayon de la balle
        float rayon = balle.GetComponent<SphereCollider>().radius;
        // tous les murs
        GameObject[] murs = GameObject.FindGameObjectsWithTag("Mur");

        foreach (GameObject mur in murs)
        {
            // s'il ya une balle a cote du murs 
            Collider murCol = mur.GetComponent<Collider>();
            Vector3 pointProche = murCol.ClosestPoint(balle.transform.position);
            float distance = (pointProche - balle.transform.position).magnitude;
            //et que la distance entre ces 2
            // est moins que 2fois le rayon de la balle 
            // on deplace la balle a une distance de 2fois le rayon
            if (distance < rayon * 2)
            {
                Vector3 directionDeplacement = (balle.transform.position - pointProche).normalized;
                balle.transform.position = pointProche + directionDeplacement * (rayon * 2);
            }
        }

    }

    // la methode qui gere les touches du raccourcis
    void faireTouches()
    {
        // lorsque le jeu commence
        // 1: vers la piste1 
        //2: vers la piste2 
        // 3: vers la piste 3
        // 0: on place la balle a cote d'un trou de la piste courante
        //9: on sort la balle de la piste courante
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
                // L'angle est entre 0 et 2pi
                float angle = Random.Range(0, 2 * Mathf.PI);
                // on prend le rayon de la balle
                float rayon = trouCourant.GetComponent<SphereCollider>().radius;
                // la distance d'un trou et la balle est 2fois son rayon
                float distanceDuTrou = 2 * rayon;
                float x = Mathf.Cos(angle) * distanceDuTrou;
                float z = Mathf.Sin(angle) * distanceDuTrou;
                // la nouvelle position de balle a cote du trou
                Vector3 nouvellePos = trouCourant.transform.position + new Vector3(x, 0.5f, z);

                balle.transform.position = nouvellePos;
            }
        }
    }

    // la methode qui envoie la balle vers une piste en prenant en parametre le numero de piste
    void versPiste(int num)
    {
        // si le compteur etait 1, ca veut dire que la balle a ete entree du premier coup
        if (compteur == 1)
        {
            gestionnaire.GetComponent<GestionAnim>().commencerUneAnimation(1);
        }
        // on remet le script de la camera
        laCamera.GetComponent<deplacementCamera>().enabled = true;
        // on place la balle a la position de depart de cette nouvelle piste
        balle.transform.position = lesPosDepart[num - 1].transform.position;
        // on sauvegarde la nouvelle postiion
        checkpoint = balle.transform.position;
        // la meme chose avec la camera, le trou, et la piste
        posCameracourante = posCamera[num - 1];
        trouCourant = lesTrous[(num - 1)];
        pisteCourante = lesPistes[(num - 1)];
        // on modifie le numero de piste et le compteur
        numPiste = num;
        compteur = 0;
        // on met a jour l'animation des pistes et coups
        gestionnaire.GetComponent<GestionAnim>().MettreAJourAnimationPiste(numPiste, compteur);

        // lorsqu'on est à la piste 3, on met le booleen a true 
        if(num == 3)
        {
            balle.GetComponent<DeplacementBalle>().piste3 = true;
        }
    }

    // la methode qui incremente le compteur 
    void incrementerCompteur()
    {
        
        compteur++;
        // on met a jour l'animation des pistes et coups
        gestionnaire.GetComponent<GestionAnim>().MettreAJourAnimationPiste(numPiste, compteur);
    }

}


