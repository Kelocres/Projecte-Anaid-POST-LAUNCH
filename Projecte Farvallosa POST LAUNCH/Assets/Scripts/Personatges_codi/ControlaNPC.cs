using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControlaNPC : MonoBehaviour
{
    // Start is called before the first frame update
    bool enMovimentNMA;
    bool enMovimentAnimat;
    private Animator animator;
    private NavMeshAgent NMA;

    public Transform objectiu;
    public float velocitatLerp;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        NMA = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!enMovimentNMA && !enMovimentAnimat) return;
        if (enMovimentNMA && enMovimentAnimat)
        {
            Debug.Log("ControlaNPC Update() enMovimentNMA:true && enMovimentAnimat:true");
            return;
        }

        if (enMovimentNMA)
        {
            if (!NMA.pathPending)
            {
                //Debug.Log("ControlaNPC Update() NMANMA.remainingDistance: "+ NMA.remainingDistance+ ", NMA.stoppingDistance: "+ NMA.stoppingDistance);
                if (NMA.remainingDistance <= NMA.stoppingDistance)
                    if (NMA.hasPath || NMA.velocity.sqrMagnitude == 0f)
                    {
                        Debug.Log("ControlaNPC Update() NMA parat");
                        FiDelCami();
                    }
            }
        }
        else if(enMovimentAnimat)
        {
            Quaternion giroOptimo = Quaternion.LookRotation(objectiu.position - transform.position);
            transform.rotation = Quaternion.Lerp(transform.rotation, giroOptimo, 0.1f);
            transform.position = Vector3.Lerp(transform.position, objectiu.position, velocitatLerp);
        }
    }

    public void IniciarCamiNMA(Transform nouObjectiu)
    {
        Debug.Log("ControlaNPC IniciarCamiNMA()");
        objectiu = nouObjectiu;
        enMovimentAnimat = false;

        if (NMA.enabled == false) NMA.enabled = true;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(objectiu.position, out hit, 3.0f, NavMesh.AllAreas))
        {
            enMovimentNMA = true;
            animator.SetBool("isXarrant", false);
            animator.SetBool("isCaminant", true);
            NMA.SetDestination(hit.position);
            NMA.isStopped = false;
        }

        else
        {
            Debug.Log("ControlaNPC IniciarCami() Posició no trobada");
            FiDelCami();
        }

    }

    public void IniciarCamiAnimat(Transform nouObjectiu)
    {
        //Iniciar corutina ( o processos en Update())
        objectiu = nouObjectiu;
        enMovimentNMA = false;
        NMA.enabled = false;

        enMovimentAnimat = true;


    }

    public void FiDelCami()
    {
        Debug.Log("ControlaNPC FiDelCami()");
        
            NMA.isStopped = true;
            enMovimentNMA = false;
        NMA.enabled = false;
        
            enMovimentAnimat = false;
        
        animator.SetBool("isCaminant", false);
        
    }

    
}
