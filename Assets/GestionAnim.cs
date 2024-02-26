using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class GestionAnim : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI animDebut;
    [SerializeField] TextMeshProUGUI[] autresAnim; //Piste et essais : pos 0, Trou en un coup : pos1, Balle hors terrain: pos2, Beaucoup de coups : pos3
    [SerializeField] TextMeshProUGUI animFin;
    private float dureeApparition = 2.0f;
    private int pisteActuelle;
    private int coupActuel;

    private void Start()
    {
        StartCoroutine(animerDebut());
        animDebut.color = new Color(animDebut.color.r, animDebut.color.g, animDebut.color.b, 0);
    }


    public void arreterAnimationDebut()
    {
        StopCoroutine(animerDebut());
        animDebut.color = new Color(animDebut.color.r, animDebut.color.g, animDebut.color.b, 0);

    }

    IEnumerator animerDebut()
    {
        float temps = 0f;        
        while (temps < dureeApparition)
        {
            temps += Time.deltaTime;
            float alpha = Mathf.Clamp01(temps / dureeApparition);
            animDebut.color = new Color(animDebut.color.r, animDebut.color.g, animDebut.color.b, alpha);
            yield return null;
        }

        animDebut.color = new Color(animDebut.color.r, animDebut.color.g, animDebut.color.b, 1);
    }


    public void commencerUneAnimation(int pos)
    {
        autresAnim[pos].gameObject.SetActive(true);
        StartCoroutine(animer(autresAnim[pos], 2.0f));
        
        
    }

    IEnumerator animer(TextMeshProUGUI texte, float nbDeRepos)
    {
        print("Animation du texte" + texte.ToString());

        texte.color = new Color(texte.color.r, texte.color.g, texte.color.b, 1);
        yield return new WaitForSeconds(nbDeRepos);

        texte.gameObject.SetActive(false);
    }

    public void MettreAJourAnimationPiste(int piste, int coups)
    {
        pisteActuelle = piste;
        coupActuel = coups;
        autresAnim[0].text = $"Piste: {pisteActuelle}     {coupActuel + 1}e coup";        
    }

    public void MettreAJourAnimationPiste()
    {
        autresAnim[0].text = $"Piste: {0}  {1}e coup";
    }

    public void activerAnimationPisteEtCoups()
    {
        autresAnim[0].gameObject.SetActive(true);
        MettreAJourAnimationPiste();
    }

    public void commencerAnimFin(int scorePiste1, int scorePiste2, int scorePiste3)
    {
        animFin.gameObject.SetActive(true);
        StartCoroutine(animerFin(scorePiste1, scorePiste2, scorePiste3));
    }

    private IEnumerator animerFin(int scorePiste1, int scorePiste2, int scorePiste3)
    {
        TextMeshProUGUI scoreFinalText = animFin;

        scoreFinalText.text = "Score final";
        yield return animer(scoreFinalText, 1.0f);
        //yield return new WaitForSeconds(1f);

        scoreFinalText.text += $"Piste 1: {scorePiste1}";
        yield return animer(scoreFinalText, 1.0f);
        //yield return new WaitForSeconds(1f);

        scoreFinalText.text += $"Piste 2: {scorePiste2}";
        yield return animer(scoreFinalText, 1.0f);
        //yield return new WaitForSeconds(1f);

        scoreFinalText.text += $"Piste 3: {scorePiste3}";
        yield return animer(scoreFinalText, 1.0f);
        //yield return new WaitForSeconds(1f);

        
        string performanceMessage = msgAfficher(scorePiste1 + scorePiste2 + scorePiste3);
        scoreFinalText.text = performanceMessage;
        yield return animer(scoreFinalText, 1.0f);
    }

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


