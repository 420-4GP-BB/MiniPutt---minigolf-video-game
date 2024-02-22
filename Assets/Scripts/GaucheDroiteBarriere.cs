using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaucheDroiteBarriere : MonoBehaviour
{
    [SerializeField] float vitesse;
    float x;
    private Vector3 vec_deplacement;

    void Start()
    {
        // Initialiser la vitesse de déplacement vers la gauche
        if(gameObject.name == "Barriere")
        {
            x = vitesse;
        }
        else
        {
            x = -vitesse;
        }
        vec_deplacement = new Vector3(x, 0, 0);
        vec_deplacement.Normalize();
    }

    void Update()
    {
        transform.Translate(vec_deplacement * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == ("Mur"))
        {
            vec_deplacement.x = -vec_deplacement.x;
            vec_deplacement.Normalize();
        }
    }
}







