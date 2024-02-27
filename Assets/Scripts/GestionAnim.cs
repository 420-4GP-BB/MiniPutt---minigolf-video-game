using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

// Ce script est le gestionnaire de toutes les animations
public class GestionAnim : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI animDebut; // L'animation du début
    [SerializeField] public TextMeshProUGUI[] autresAnim; //Piste et essais : pos 0, Trou en un coup : pos1, Balle hors terrain: pos2, Beaucoup de coups : pos3
    [SerializeField] public TextMeshProUGUI[] animFin; // L'animation de la fin sous forme de plusieurs animations
    public event Action finJeu; // Une action pour déclencher que le jeu est fini, elle permet de relancer le jeu depuis le game manager
    private float dureeApparition = 2.0f;// Un float sur la durée d'apparition de animation pour devenir graduellement opaque 
    private int pisteActuelle; // la piste actuelle pour faire le compteur des coups et pistes
    private int coupActuel; // le coup actuel pour faire les compteur des coups et pistes


    private void Start()
    {
        // Quand on commence, on active l'animation du début
        animDebut.gameObject.SetActive(true);
        StartCoroutine(animerDebut());
        // Le texte de l'animation du début commence transparent
        animDebut.color = new Color(animDebut.color.r, animDebut.color.g, animDebut.color.b, 0);
    }

    // La coroutine qui fait l'animation du début
    IEnumerator animerDebut()
    {
        // le temps qui est changeable à chaque frame, ce qui désigne l'arrivée du texte entre 0s et 2s  
        float temps = 0f;
        while (temps < dureeApparition)
        {
            temps += Time.deltaTime;
            // Le alpha est l'opacité entre 0 et 1
            float alpha = Mathf.Clamp01(temps / dureeApparition);
            animDebut.color = new Color(animDebut.color.r, animDebut.color.g, animDebut.color.b, alpha);
            yield return null;
        }

        // à la fin de la coroutine, on affiche le texte de façon claire
        animDebut.color = new Color(animDebut.color.r, animDebut.color.g, animDebut.color.b, 1);
    }

    // Une methode qui arrete l'animation du début
    public void arreterAnimationDebut()
    {
        StopCoroutine(animerDebut());
        // on désactive l'animation de l'écran
        animDebut.gameObject.SetActive(false);
    }

    // Une méthode qui commence les autres animations
    // Cette methode peut commencer les animations : coup d'un coup- dépassement de coups limité- balle sortie
    public void commencerUneAnimation(int pos)
    {
        autresAnim[pos].gameObject.SetActive(true);
        // On commence la coroutine qui fait l'animation qui dure 2s
        StartCoroutine(animer(autresAnim[pos], 2.0f));
        
        
    }

    // une coroutine qui anime une animation
    // cette coroutine peut animer les animations : coup d'un coup- dépassement de coups limité- balle sortie
    IEnumerator animer(TextMeshProUGUI texte, float nbDeRepos)
    {
        texte.color = new Color(texte.color.r, texte.color.g, texte.color.b, 1);
        yield return new WaitForSeconds(nbDeRepos);

        // après avoir fait l'animation, on la désactive de l'écran
        texte.gameObject.SetActive(false);
    }

    // la méthode qui active l'animation des pistes et coups
    // Cette animation est mise à jour à chaque fois que le joueur effectue un nouveau coup ou passe à une autre piste
    public void activerAnimationPisteEtCoups()
    {
        autresAnim[0].gameObject.SetActive(true);
        MettreAJourAnimationPiste();
    }

    // La méthode pour mettre à jour le numero de piste et numero de coups
    public void MettreAJourAnimationPiste(int piste, int coups)
    {
        pisteActuelle = piste;
        coupActuel = coups;
        // on ajoute le numero de piste et le numero de coups à l'animation 
        autresAnim[0].text = $"Piste: {pisteActuelle}     {coupActuel + 1}e coup";        
    }

    // La methode qui sert à donner une valeur du début pour la piste et les coups
    public void MettreAJourAnimationPiste()
    {
        autresAnim[0].text = $"Piste: {0}  {1}e coup";
    }

    
    // Une méthode qui commence l'animation de la fin 
    // Cette méthode prend en parametre le score dans la piste1, piste2 et piste 3
    public void commencerAnimFin(int scorePiste1, int scorePiste2, int scorePiste3)
    {
        // On a dit que l'animation de fin contient plusieurs mini animations, on les active toutes
        foreach(TextMeshProUGUI text in animFin)
        {
            text.gameObject.SetActive(true);
            text.color = new Color(text.color.r, text.color.g, text.color.b, 0);
        }
        // On commence la coroutine qui anime la fin
        StartCoroutine(animerFin(scorePiste1, scorePiste2, scorePiste3));
    }

    // La coroutine qui anime la fin et affiche les résultats
    private IEnumerator animerFin(int scorePiste1, int scorePiste2, int scorePiste3)
    {
        // Afficher score final
        animFin[0].text = "Score final";
        yield return animerTxt(animFin[0], 1.0f);

        // afficher le score de la piste1
        animFin[1].text = scorePiste1.ToString();
        yield return animerTxt(animFin[1], 1.0f);

        // afficher le score de la piste2
        animFin[2].text = scorePiste2.ToString();
        yield return animerTxt(animFin[2], 1.0f);

        // afficher le score de la piste3
        animFin[3].text = scorePiste3.ToString();
        yield return animerTxt(animFin[3], 1.0f);

        //afficher le message selon les scores
        // on appelle la methode qui retourne le message à afficher, et on l'affiche.
        string performanceMessage = msgAfficher(scorePiste1 + scorePiste2 + scorePiste3);
        animFin[4].text = performanceMessage;
        yield return animer(animFin[4], 1.0f);

        // Lorsque toutes les résultats sont affichés, on désactive toutes les animations afin de recommencer le jeu
        desactiverAnimFin();
    }

    // Une coroutine qui anime le texte affiché, puis un temps de repos avant de passer au prochain texte
    IEnumerator animerTxt(TextMeshProUGUI texte, float nbDeRepos)
    {
        texte.color = new Color(texte.color.r, texte.color.g, texte.color.b, 1);
        yield return new WaitForSeconds(nbDeRepos);
    }

    
    // Une méthode pour désactiver les animations de fin, lorsque toutes les animations de fin sont désactivé
    // on déclenche la fin du jeu
    void desactiverAnimFin()
    {
        foreach (TextMeshProUGUI text in animFin)
        {
            text.gameObject.SetActive(false);
        }
        finJeu?.Invoke();
    }

    // La methode qui prend le score des 3 pistes en parametres et retourne ce qu'on affiche à l'écran
    private string msgAfficher(int scoreTotal)
    {
        if (scoreTotal <= 6)
        {
            return "Excellent !";
        }
        else if (scoreTotal <= 9)
        {
            return "Très bien!";
        }
        else if (scoreTotal <= 12)
        {
            return "Pas mal";
        }
        else
        {
            return "Pas terrible...";
        }
    }






}


