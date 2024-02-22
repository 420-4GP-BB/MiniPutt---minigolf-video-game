using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject[] lesPosDepart;
    [SerializeField] GameObject[] posCamera;
    [SerializeField] Camera laCamera;
    [SerializeField] GameObject balle;
    private bool jeuCommence;

    // Start is called before the first frame update
    void Start()
    {
        laCamera.transform.position = posCamera[3].transform.position;
        laCamera.transform.rotation = posCamera[3].transform.rotation;
        balle.SetActive(false);
        laCamera.GetComponent<deplacementCamera>().enabled = false;
       
        jeuCommence = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && jeuCommence == false)
        {
            print("on appuie sur n'impote quoi ");
            // camera attaché au joueur
            //laCamera.transform.position = posCamera[0].transform.position;
            //laCamera.transform.rotation = posCamera[0].transform.rotation;
            // suivre la balle
            balle.SetActive(true);
            balle.transform.position = lesPosDepart[0].transform.position;
            laCamera.GetComponent<deplacementCamera>().enabled = true;
            jeuCommence = true;
        }

     
    }
}
