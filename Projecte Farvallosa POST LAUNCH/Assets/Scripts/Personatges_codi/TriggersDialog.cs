using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggersDialog : MonoBehaviour
{

    //Codi per a posar en TD_Esquerra i TD_Dreta
    //Em fa falta obtindre més informació del Delegates
    public delegate void delTrigger(int num);
    public event delTrigger dT;

    //Número per a identificar el trigger
    //TD_Esquerra: 0
    //TD_Dreta: 1
    public int numTrigger;

    void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            //Amb el Delegate, manar senyal al Codi Principal per a
            //que identifique que el jugador està en aquest trigger
            if(dT!=null)
            {    //Canviar el valor d orientacio del dialeg del personatge jugador
                other.gameObject.GetComponentInChildren<FaceCameraPersonajes>().CanviaDialegOrientacio(numTrigger);
                //dT(numTrigger, other.gameObject);
                dT(numTrigger);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag=="Player")
        {
            //Amb el Delegate, manar senyal de que el jugador s'ha ixit del trigger
            if(dT!=null)
            {    
                //dT(-1, null);
                dT(-1);
            }
        }
    }

}
