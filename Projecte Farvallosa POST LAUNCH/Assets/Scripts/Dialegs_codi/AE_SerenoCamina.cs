using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AE_SerenoCamina : ActivadorEvent
{
    //Llista de CTs:
    //  - CT davant de la porta, guia al sereno cap a fora del local
    //  - CT fora del local, ordena que es tanquen les portes
    //      i que es pare el sereno

    //public GameObject sereno;
    //private Animator sereno_Animator;
    //private NavMeshAgent sereno_NMA;

    public ControlaNPC controlaSereno;

    private CineTriggerScript[] cineTriggers;
    public CineTriggerScript primer_CT;

    //private bool activat;

    private void Start()
    {
        //activat = false;

        if(controlaSereno==null)
        {
            Debug.Log("AE_SerenoCamina Start() ControlaNPC del Sereno no registrat");
            return;
        }

        string tagNPC = controlaSereno.tag;

        cineTriggers = GetComponentsInChildren<CineTriggerScript>();
        Debug.Log("AE_SerenoCamina Start() cineTriggers.Lenght = " + cineTriggers.Length);
        if(cineTriggers.Length != 0)
        {
            foreach (CineTriggerScript ct in cineTriggers)
            {
                ct.activat = false;
                ct.tagColliderEnter = tagNPC;

            }
        }
    }

    /*private void Update()
    {
        if (!activat) return;


    }*/
    public override void Activar()
    {
        Debug.Log("AE_SerenoCamina Activar()");

        if(cineTriggers.Length == 0 || primer_CT == null)
        {
            Debug.Log("AE_SerenoCamina Activar() No hi han CineTriggers");
            return;
        }

        if (controlaSereno == null)
        {
            Debug.Log("AE_SerenoCamina Activar() El sereno no esta configurat");
            return;
        }

        

        foreach (CineTriggerScript ct in cineTriggers)
            ct.activat = true;

        //Començar corutina cap al primer ct
        Transform primer_transform = primer_CT.transform;

        // Ordenar al ControlaNPC
        controlaSereno.IniciarCamiNMA(primer_transform);
        

    }


}
