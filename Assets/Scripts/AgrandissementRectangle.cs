using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgrandissementRectangle : MonoBehaviour
{
    // Declaration de variables 
    private bool _agrandissementActif;// un booleen qui detecte si le rectangle est dans les marges d'agrandissements
                                      
    private Vector3 _vecteurCroissance = new Vector3(0, 0.01f, 0); // Le taux de croissance du vecteur
                                                                   // Si on change ces valeurs, on change la vitesse d'acroissement.
                                                                           

    void Start()
    {
        // Dans la methode start, on initialise l'agrandissement actif à true
        _agrandissementActif = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        // si l'agrandissement est actif donc on ajoute le au rectangle le vecteur de croissance
        // sinon, on baisse
        if (_agrandissementActif)
        {
            transform.localScale += _vecteurCroissance;
        }
        else
        {
            transform.localScale -= _vecteurCroissance;
        }

        // On regarde s'il faut agrandir ou diminuer la taille pour la prochaine itération

        if (transform.localScale.y >= 2.0f)
        {
            // si la forme du rectangle est plus grande que 2.0, l'objet dépasse les limites d'agrandissement donc 
            // il n'est pas actif et ne peut pas s'agrandir
            _agrandissementActif = false;
        }

        if (transform.localScale.y <= 0.1f)
        {
            // si ce dernier est plus petit que 0.1, on remet l'agrandissement comme actif parce que l'objet est trop petit. 
            _agrandissementActif = true;
        }
    }
}
