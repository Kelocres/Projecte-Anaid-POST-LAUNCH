using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;
using UnityEngine.AI;

public class CinematicaState : IStateJugador
{
    
    private bool activatNMA = false;
    //Per a saber si es deurà passar automàticament a Explorar_State després de NMA
    public bool explorarDespresNMA = true;

    //En cas de que, en una cinemàtica, es pida un Dialeg després d 'arribar 

    public void StateUpdate(DetectiuStateManager cj)
    {
        //Debug.Log("Soc Cinemàtica");
        if(activatNMA)
        {
            if (cj.agentNavegador.enabled == false)
            {
                Debug.Log("CinematicaState StateUpdate() ERROR El NavMeshAgent està desactivat!");
                return;
            }

            if (!cj.agentNavegador.pathPending)
            if(cj.agentNavegador.remainingDistance <= cj.agentNavegador.stoppingDistance)
                if(cj.agentNavegador.hasPath || cj.agentNavegador.velocity.sqrMagnitude == 0f)
                {
                    Debug.Log("NMA parat");
                    FiDelCami(cj);
                }
        }
        
    }

    //Per a comprovar si NavMeshAgent encara està funcionant o ja ha acabat
    public void FiDelCami(DetectiuStateManager cj)
    {
        Debug.Log("CinematicaState FiDelCami()");
        activatNMA = false;
        cj.agentNavegador.isStopped = true;
        // PROVISIONAL: Tornar a ExplorarState
        cj.animacion.SetBool("isCaminant",false);
        
        if(explorarDespresNMA)
            cj.CanviarState_Exlorar();
    }

    public bool isActivatNMA() { return activatNMA; }

    //Aquesta funció es crida des del DetectiuStateManager
    public void CaminarCapDesti(DetectiuStateManager cj, Transform desti, bool explorarDespres)
    {
        Debug.Log("Funció CaminarCapDestí de CinematicaState executada");

        if(cj.agentNavegador.enabled == false)
        {
            //Debug.Log("CinematicaState CaminarCapDestí() ERROR El NavMeshAgent està desactivat!");
            //return;
            cj.HabilitarNMA();
        }

        //AVÍS!!!
        //PER A TROBAR LA LOCALITZACIÓN D'UN OBJECTE PROP O DAMUNT DEL NAVMESH,
        //FAÇA FALTA UTILITZAR NAVMESH.SAMPLEPOSITION
        //REFERÈNCIA:
        //https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html

        NavMeshHit hit;
        if (NavMesh.SamplePosition(desti.position, out hit, 3.0f, NavMesh.AllAreas))
        {
            Debug.Log("Posició de SamplePosition vàlida");
            activatNMA = true;
            cj.animacion.SetBool("isCaminant", true);
            cj.agentNavegador.SetDestination(hit.position);
            cj.agentNavegador.isStopped = false;

            //Determinar si després es pasa a ExplorarState
            explorarDespresNMA = explorarDespres;
        }
        else
        {
            Debug.Log("Posició no trobada");
            FiDelCami(cj);
        }

    }


    
    IStateJugador CanviaState(DetectiuStateManager cj)
    {
        return cj.explorarState;
    }

    
}
