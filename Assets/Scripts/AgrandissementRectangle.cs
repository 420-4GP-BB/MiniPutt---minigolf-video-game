using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgrandissementRectangle : MonoBehaviour
{
    private bool _agrandissementActif;  
    private Vector3 _vecteurCroissance = new Vector3(0, 0.005f, 0); // Le taux de croissance du vecteur
                                                                           // Si on change ces valeurs, on change la vitesse d'acroissement.
                                                                           // Sera fait autrement dans l'exercice 2

    void Start()
    {
        _agrandissementActif = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_agrandissementActif)
        {
            transform.localScale += _vecteurCroissance;
        }
        else
        {
            transform.localScale -= _vecteurCroissance;
        }

        // On regarde s'il faut agrandir ou diminuer la taille pour la prochain itération

        if (transform.localScale.y >= 2.0f)
        {
            _agrandissementActif = false;
        }

        if (transform.localScale.y <= 0.1f)
        {
            _agrandissementActif = true;
        }
    }
}
