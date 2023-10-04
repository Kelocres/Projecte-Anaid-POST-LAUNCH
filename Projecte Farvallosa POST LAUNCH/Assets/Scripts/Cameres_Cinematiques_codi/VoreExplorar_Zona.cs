using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoreExplorar_Zona : MonoBehaviour
{
    public Transform posCameraZona;
    private Controla_Camara controlaCamara;

    //Si existix un SeguixJugador en l'escena, els canvis de posCamera i objectiu
    //s'enviaran a ell
    // Si no existix, els canvis es faran en Controla_Camara
    public SeguixJugador seguixJugador;

    private bool realitzable;
    // Start is called before the first frame update
    void Start()
    {
        realitzable = true;
        Controla_Camara[] ccs = FindObjectsOfType<Controla_Camara>();
        if (ccs.Length == 0)
            Debug.Log("VoreExplorar_Zona Start() No hi ha Controla_Camara!!");
        else if (ccs.Length > 1)
            Debug.Log("VoreExplorar_Zona Start() Hi han "+ccs.Length+" Controla_Camara, el programa no funcionarà");
        else
            controlaCamara = ccs[0];
        
        if(seguixJugador==null)
        {
            SeguixJugador[] sjs = FindObjectsOfType<SeguixJugador>();
            if (sjs.Length == 0)
                Debug.Log("VoreExplorar_Zona Start() No hi ha SeguixJugador!!");
            else if (sjs.Length > 1)
                Debug.Log("VoreExplorar_Zona Start() Hi han " + ccs.Length + " SeguixJugador, el programa no funcionarà");
            else
                seguixJugador = sjs[0];
        }

        if(posCameraZona == null)
            Debug.Log("VoreExplorar_Zona Start() No hi ha posCameraZona!!");

        if (seguixJugador == null && controlaCamara == null)
            realitzable = false;
    }

    // Update is called once per frame
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("VoreExplorar_Zona OnTriggerEnter() Has entrat en la zona");
            if (realitzable)
            {
                if (posCameraZona != null && controlaCamara != null)
                {
                    if (seguixJugador != null)
                        seguixJugador.NouPosCameraExplorar(posCameraZona);
                    else
                        controlaCamara.NouPosCamera(posCameraZona);
                }
            }
            else
                Debug.Log("VoreExplorar_Zona OnTriggerEnter() No hi ha Controla_Camara ni SeguixJugador, no es realitza canvi!!!");
        }
    }
}
