using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_ParaNPCTancaPorta : CineTriggerScript
{
    public ControlaNPC controlaNPC;
    public Animator animatorPortes;

    public override void ActivarAlEntrar()
    {
        Debug.Log("CT_ParaNPCTancaPorta ActivarAlEntrar()");
        if (controlaNPC == null)
        {
            Debug.Log("CT_ParaNPCTancaPorta ActivarAlEntrar() ControlaNPC no registrat");
            return;
        }

        if (animatorPortes == null)
        {
            Debug.Log("CT_ParaNPCTancaPorta ActivarAlEntrar() animatorPortes no registrat");
            return;
        }

        controlaNPC.FiDelCami();
        animatorPortes.SetBool("isObert", false);

        //Desactivar CT
        //activat = false;
    }
}
