using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;

public class ExplorarState : IStateJugador
{
    public float velocitat = 5f;

    //Input del moviment
    float ObtinInputX() {return Input.GetAxis("Horizontal");}
    float ObtinInputY() {return Input.GetAxis("Vertical");}

    //Input per a xarrar amb NPCs
    //bool ObtinInputXarrar() {return Input.GetKeyDown(KeyCode.K);}
    bool ObtinInputXarrar() { return Input.GetMouseButtonDown(1); }

    public void StateUpdate(DetectiuStateManager cj)
    {
        //Debug.Log("Soc Explorar");
        GestionarInputMoure(cj);
        if(ObtinInputXarrar())  GestionarInputXarrar(cj);
    }
    
    IStateJugador CanviaState(DetectiuStateManager cj)
    {
        return cj.xarrarState;
    }

    void GestionarInputXarrar(DetectiuStateManager cj)
    {
        Debug.Log("Input Xarrar pulsat");
        //Buscar un objecte amb MainTriggersDialog que puga ser activat
        MainTriggersDialog [] listaTriggers = GameObject.FindObjectsOfType<MainTriggersDialog>();
        bool triggerDetectat = false;
        foreach(MainTriggersDialog tri in listaTriggers)
        {
            if (tri.ActivarDialeg())
            {
                triggerDetectat = true;
                break;
            }
        }

        //Sols si s'ha trobat un trigger es canvia a XarrarState
        if(triggerDetectat)
            cj.CanviarState_Xarrar();
        
    }

    void GestionarInputMoure(DetectiuStateManager cj)
    {
        cj.inputMoure = new Vector3(ObtinInputX(), 0, ObtinInputY());
        //Usar input de velocidad para que el personaje se oriente 
        //según las coordenadas de la cámara
        cj.SnapAlignCharacterWithCamera();

        //Per a animar Idle o Caminar
        if (cj.inputMoure.x == 0 && cj.inputMoure.z == 0)
        {
            //Debug.Log("ExplorarState GestionarInputMoure() isCaminant == false");
            cj.animacion.SetBool("isCaminant", false);
        }
        else
        {
            //Debug.Log("ExplorarState GestionarInputMoure() isCaminant == true");
            cj.animacion.SetBool("isCaminant", true);
        }

        if(cj.inputMoure.x != 0 && cj.inputMoure.z != 0)
            cj.inputMoure = new Vector3(0,0,Mathf.Abs(cj.inputMoure.x * cj.inputMoure.z));
        else if (cj.inputMoure.x == 0)
            cj.inputMoure = new Vector3(0,0,Mathf.Abs(cj.inputMoure.z));
        else if (cj.inputMoure.z == 0)
            cj.inputMoure = new Vector3(0,0,Mathf.Abs(cj.inputMoure.x));

        cj.inputMoure.Normalize();

        //cambiar velocidad a "ir adelante"
        Vector3 velocidadActual = cj.transform.TransformDirection(cj.inputMoure);
        CharacterController controlador = cj.GetComponent<CharacterController>();
        velocidadActual = new Vector3(velocidadActual.x * velocitat, 0f, velocidadActual.z * velocitat);

        //Adició del moviment vertical
        //velocidadActual.y += Physics.gravity.y; //Cau molt ràpid
        velocidadActual.y += Physics.gravity.y * 0.7f;


        velocidadActual = velocidadActual * Time.smoothDeltaTime * 4;

        controlador.Move(velocidadActual);

        /*if (controlador.isGrounded)
            Debug.Log("DETECTIVIE ESTÁ GROUNDED");
        else
            Debug.Log("NO GROUNDED");*/

        //Dubte:
        //El codi del moviment deuria estar ací o en DetectiuStateManager, 
        //i ací simplement s'indicaria que el métode es pot executar mentre
        //s'està en ExplorarState?
        //
        //Possibilitat: definir ací el valor de inputMoure i la velocitat de
        //moviment, i passar els valors a DetectiuStateManager
    }

    
}
