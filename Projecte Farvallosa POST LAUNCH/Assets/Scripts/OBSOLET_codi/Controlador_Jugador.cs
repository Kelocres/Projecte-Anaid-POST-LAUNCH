using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;
//using Assets.Scripts.States;

public class Controlador_Jugador : MonoBehaviour
{
    //Per al State Machine
    private IStateJugador stateActual;

    public ExplorarState explorarState  = new ExplorarState();
    public XarrarState xarrarState      = new XarrarState();

    Vector3 inputMover;
    public float velocitat = 5f;
    Animator animacion;
    
    void OnEnable()
    {
        animacion = transform.GetChild(1).GetComponent<Animator>();
        stateActual = explorarState;
    }

    // Update is called once per frame
    void Update()
    {
        //Activitat de l'State
        /*if(stateActual!=null)   
            stateActual.StateUpdate(this);*/

        ControlMovimientoOrientacion();
        IdentificarQuad();

    }

    float ObtenInputX() {return Input.GetAxis("Horizontal");}
    float ObtenInputY() {return Input.GetAxis("Vertical");}

    void ControlMovimientoOrientacion()
    {
        if(this.GetComponent<CharacterController>() == null)    return;

        //Obtener inputs de movimiento
        inputMover = new Vector3(ObtenInputX(), 0, ObtenInputY());

        //Usar input de velocidad para que el personaje se oriente 
        //según las coordenadas de la cámara
        SnapAlignCharacterWithCamera();

        //Per a animar Idle o Caminar
        if(inputMover.x == 0 && inputMover.z == 0)
            animacion.SetBool("isCaminant",false);
        else animacion.SetBool("isCaminant",true);

        if(inputMover.x != 0 && inputMover.z != 0)
            inputMover = new Vector3(0,0,Mathf.Abs(inputMover.x * inputMover.z));
        else if (inputMover.x == 0)
            inputMover = new Vector3(0,0,Mathf.Abs(inputMover.z));
        else if (inputMover.z == 0)
            inputMover = new Vector3(0,0,Mathf.Abs(inputMover.x));

        inputMover.Normalize();

        //cambiar velocidad a "ir adelante"
        Vector3 velocidadActual = transform.TransformDirection(inputMover);
        CharacterController controlador = GetComponent<CharacterController>();
        velocidadActual = new Vector3(velocidadActual.x * velocitat, 0f, velocidadActual.z * velocitat);
        velocidadActual = velocidadActual * Time.smoothDeltaTime * 4;

        controlador.Move(velocidadActual);

    }

    void SnapAlignCharacterWithCamera()
    {
        if(inputMover.z != 0 || inputMover.x != 0)
        {
            transform.rotation =    Quaternion.Euler(transform.eulerAngles.x,
                                    Camera.main.transform.eulerAngles.y,
                                    transform.eulerAngles.z);

            float rot = 0;
            //z = delante, x= a los lados

            // si se va en la dirección opuesta de la actual, 
            //rotar inmediatamente; si no, rotación gradual    
            
            //si se va hacia atrás, rotar 180
            if (inputMover.z < 0) rot = 180;

            //si se va hacia los lados, rotar 90 según el vector horizontal
            if(inputMover.z == 0) 
                rot += (inputMover.x / Mathf.Abs (inputMover.x)) * 90f;
            //en caso contrario, se asumirán horizontal y vertical
            else    
                rot += (Mathf.Atan (inputMover.x/inputMover.z)) * 45f;
            
            transform.rotation = Quaternion.Euler(  transform.eulerAngles.x,
                                                    transform.eulerAngles.y + rot,
                                                    transform.eulerAngles.z);

        }
    }

    void IdentificarQuad()
    {
        RaycastHit idenColisionador;
        if(Physics.Linecast(transform.position, Camera.main.transform.position, out idenColisionador))
        {
            //Debug.Log("Colisió");
            //Comprobar gameobject colisionado con los cuadros hijos
            for(int i = 0; i<4; i++)
                if(idenColisionador.collider.gameObject == transform.GetChild(0).GetChild(i).gameObject)
                    //Debug.Log((float)i/3f);
                    animacion.SetFloat("idenColisionadorCamara", (float)i/3f);
        }

    }

}
