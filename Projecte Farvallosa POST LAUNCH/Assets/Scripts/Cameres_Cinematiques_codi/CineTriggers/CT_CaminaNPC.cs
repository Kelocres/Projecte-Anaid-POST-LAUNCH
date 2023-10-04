using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_CaminaNPC : CineTriggerScript
{
    //Hi ha que ordenar coses al NPC que es captura
    public ControlaNPC controlaNPC;
    public Transform nouDesti;
    public override void ActivarAlEntrar()
    {
        Debug.Log("CT_CaminaNPC ActivarAlEntrar()");
        if(controlaNPC==null)
        {
            Debug.Log("CT_CaminaNPC ActivarAlEntrar() ControlaNPC no registrat");
            return;
        }

        if(nouDesti == null)
        {
            Debug.Log("CT_CaminaNPC ActivarAlEntrar() nouDesti no registrat");
            return;
        }

        //controlaNPC.IniciarCamiNMA(nouDesti);
        controlaNPC.IniciarCamiAnimat(nouDesti);

        //Desactivar CT
        //activat = false;
    }
}
