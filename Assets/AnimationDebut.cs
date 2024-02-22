using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class AnimationDebut : MonoBehaviour
{
    public event Action debutAnimation;
    public event Action finAnimation;

    [SerializeField] TextMeshProUGUI titre; 
    [SerializeField] float dureeApparition = 2.0f; 

    private void Start()
    {
        debutAnimation?.Invoke();
        titre.color = new Color(titre.color.r, titre.color.g, titre.color.b, 0);
        StartCoroutine(texteApparu());
    }

    IEnumerator texteApparu()
    {
        float temps = 0;

        while (temps < dureeApparition)
        {
            temps += Time.deltaTime;
            float col = Mathf.Clamp01(temps / dureeApparition);
            titre.color = new Color(titre.color.r, titre.color.g, titre.color.b, col);
            yield return null;
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            StopAllCoroutines();            
            finAnimation?.Invoke();
        }
    }
}

