using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraElementos : MonoBehaviour
{
    //Siempre se revertirá para tener todos los sprites bien
    //public bool revertir;
    Vector3 direccionCamara;

    //float lockPos = 0f;
    //La càmara a la qual s'encaren no serà sempre la Camera.main
    private Camera camaraActual;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        //Versió 1: Rotan constatement per a mirar sempre cap a la càmara
        //A partir de cert número, comença a maretjar
        /*
        direccionCamara = Camera.main.transform.position - transform.position;
        transform.rotation = Quaternion.LookRotation(direccionCamara);

        //if (revertir==true) 
            //transform.rotation.y += 180; ASI NO VALE PORQUE NO PUEDES SUMAR A UN QUATERNIÓN
            //CON transform.rotate CONSIGUES UNA ROTACIÓN RELATIVA
            transform.Rotate(0,180,0);

        //Con lockPos se bloquea la variable
        transform.rotation = Quaternion.Euler(lockPos, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        */

        //Versió 2: El sprite manté la mateixa rotació que la càmara
        if(camaraActual!=null)
            transform.rotation = camaraActual.transform.rotation;
        else if(Camera.main != null)
            transform.rotation = Camera.main.transform.rotation;

    }

    public void CanviarCamara(Camera novaCamara)
    {
        camaraActual = novaCamara;
        transform.rotation = camaraActual.transform.rotation;
    }
}
