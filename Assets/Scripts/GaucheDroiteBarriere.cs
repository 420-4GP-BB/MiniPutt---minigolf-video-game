using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Le script qui fait déplacere les barrières de la piste 3.
public class GaucheDroiteBarriere : MonoBehaviour
{
    // On entre la vitesse des barrières manuellement
    [SerializeField] float vitesse;
    // Le x est le l'axe dans le vecteur de déplacement
    float x;
    // Les barrières vont se déplacer avec le vecteur de déplacement
    private Vector3 vec_deplacement;

    void Start()
    {
        // Initialiser la vitesse de déplacement vers la gauche pour la premiere barriere
        if(gameObject.name == "Barriere")
        {
            x = vitesse;
        }
        else
        {
            // la vitesse de déplacement vers la droite pour l'autre barrière
            x = -vitesse;
        }

        vec_deplacement = new Vector3(x, 0, 0);
        vec_deplacement.Normalize();
    }

    void Update()
    {
        // Pour chaque frame, la barrière se déplace selon son nom
        transform.Translate(vec_deplacement * Time.deltaTime);
    }

    // Si la barrière touche le mur sur les cotés, elle change de direction
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







