using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le script qui fait d�placere les barri�res de la piste 3.
public class GaucheDroiteBarriere : MonoBehaviour
{
    // On entre la vitesse des barri�res manuellement
    [SerializeField] float vitesse;
    // Le x est le l'axe dans le vecteur de d�placement
    float x;
    // Les barri�res vont se d�placer avec le vecteur de d�placement
    private Vector3 vec_deplacement;

    void Start()
    {
        // Initialiser la vitesse de d�placement vers la gauche pour la premiere barriere
        if(gameObject.name == "Barriere")
        {
            x = vitesse;
        }
        else
        {
            // la vitesse de d�placement vers la droite pour l'autre barri�re
            x = -vitesse;
        }

        vec_deplacement = new Vector3(x, 0, 0);
        vec_deplacement.Normalize();
    }

    void Update()
    {
        // Pour chaque frame, la barri�re se d�place selon son nom
        transform.Translate(vec_deplacement * Time.deltaTime);
    }

    // Si la barri�re touche le mur sur les cot�s, elle change de direction
    // EX: barriere qui va vers la gauche verra vers la droite si elle tape un mur, et vice-versa
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == ("Mur"))
        {
            vec_deplacement.x = -vec_deplacement.x;
            vec_deplacement.Normalize();
        }
    }
}







