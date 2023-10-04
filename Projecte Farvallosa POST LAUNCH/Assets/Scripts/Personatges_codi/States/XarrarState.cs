using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class XarrarState : IStateJugador
{
    //Input per a passar
    bool ObtinNextLinia() {return Input.GetKeyDown(KeyCode.K);}

    public void StateUpdate(DetectiuStateManager cj)
    {
        //Debug.Log("Soc Xarrar");
        //Si el jugador pulsa el botó, se li envia ordre a ManejaDialeg de passar a la següent linia
        if (ObtinNextLinia())
        {
            Debug.Log("XarrarState StateUpdate() crida a ManejaDialeg MostrarOracioActual()");
            GameObject.FindObjectOfType<ManejaDialeg>().LiniaContinua();
        }
    }


    
    IStateJugador CanviaState(DetectiuStateManager cj)
    {
        return cj.explorarState;
    }
}
