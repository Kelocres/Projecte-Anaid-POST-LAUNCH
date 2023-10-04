using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentoSpriteSheets : MonoBehaviour
{
    
    private Animator animador;
    
    //Codi de FaceCameraPersonajes
    Vector3 direccionCamara;

    // Start is called before the first frame update
    void Start()
    {
        animador = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Codi de FaceCameraPersonajes
        direccionCamara = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direccionCamara);
        transform.Rotate(0, 180, 0);

        if (Input.GetKeyDown(KeyCode.A)) animador.SetBool("decididor", true);
        if (Input.GetKeyDown(KeyCode.S)) animador.SetBool("decididor", false);
    }
}
