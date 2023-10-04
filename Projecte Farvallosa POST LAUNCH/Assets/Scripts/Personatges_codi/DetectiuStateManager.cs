using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Interfaces;
using UnityEngine.AI;
using UnityEngine.Events;

public class DetectiuStateManager : MonoBehaviour
{
    //Per al State Machine
    private IStateJugador stateActual;

    public ExplorarState explorarState  = new ExplorarState();
    public XarrarState xarrarState      = new XarrarState();
    public CinematicaState cineState    = new CinematicaState();

    public Vector3 inputMoure;
    public float velocitat = 5f;
    public Animator animacion; //És public per a ser accecible pels States
    public NavMeshAgent agentNavegador;
    private ColisionadorCamera_Script colcam;

    //La càmara a la qual s'encaren no serà sempre la Camera.main
    private Camera camaraActual;
    Controla_Camara cc;
    SeguixJugador sj;

    //REF: https://www.youtube.com/watch?v=oc3sQamIh-Q
    // Els delegates es deuen gastar per a coses que deuen funcionar sempre en tot el projecte
    // Els UnityEvents es deuen gastar per a coses que només ocorren en l'escena actual
    [SerializeField] UnityEvent enExplorarEvent;

    //Delegates per a mostrar o amagar les senyals de dialeg
    public delegate void delStateManager();
    public event delStateManager delExplorar;
    public event delStateManager delXarrar;
    public event delStateManager delCinematica;
    
    void Start()
    {
        //VA I AGARRA EL ANIMATOR PER A LES ANIMACIONS TIMELINE
        //animacion = GetComponentInChildren<Animator>();
        if(animacion == null)
            animacion = transform.GetChild(0).GetComponent<Animator>();
        agentNavegador = GetComponent<NavMeshAgent>();
        agentNavegador.speed = velocitat * 3f;
        //stateActual = explorarState;
        
        if(stateActual==null)   CanviarState_Exlorar();

        colcam = GetComponentInChildren<ColisionadorCamera_Script>();

        cc = FindObjectOfType<Controla_Camara>();
        sj = FindObjectOfType<SeguixJugador>();

        //camaraActual = Camera.main;
    }

    //Input del moviment
    //public float ObtinInputX() {return Input.GetAxis("Horizontal");}
    //public float ObtinInputY() {return Input.GetAxis("Vertical");}
    
    //Input per a xarrar amb NPC
    //bool ObtinInputXarrar() {return Input.GetKeyDown(KeyCode.K);}

    void Update()
    {
        //Activitat de l'State
        if(stateActual!=null)   
            stateActual.StateUpdate(this);

        //ControlMovimientoOrientacion();
        //IdentificarQuad();
        //inputMoure = new Vector3(ObtenInputX(), 0, ObtenInputY());

        /*
        //Usar input de velocidad para que el personaje se oriente 
        //según las coordenadas de la cámara
        SnapAlignCharacterWithCamera();

        //Per a animar Idle o Caminar
        if(inputMoure.x == 0 && inputMoure.z == 0)
            animacion.SetBool("isCaminant",false);
        else animacion.SetBool("isCaminant",true);

        if(inputMoure.x != 0 && inputMoure.z != 0)
            inputMoure = new Vector3(0,0,Mathf.Abs(inputMoure.x * inputMoure.z));
        else if (inputMoure.x == 0)
            inputMoure = new Vector3(0,0,Mathf.Abs(inputMoure.z));
        else if (inputMoure.z == 0)
            inputMoure = new Vector3(0,0,Mathf.Abs(inputMoure.x));

        inputMoure.Normalize();

        //cambiar velocidad a "ir adelante"
        Vector3 velocidadActual = transform.TransformDirection(inputMoure);
        CharacterController controlador = GetComponent<CharacterController>();
        velocidadActual = new Vector3(velocidadActual.x * velocitat, 0f, velocidadActual.z * velocitat);
        velocidadActual = velocidadActual * Time.smoothDeltaTime * 4;

        controlador.Move(velocidadActual);
        */
    }

    //LES FUNCIONS DE FORZAR FLOAT PARAMETER NO ES DEUEN CRIDAR, EXCEPTE SI ES TOTALMENT NECESSARI I NO HI HA ALTRA SOLUCIÓ
    public void ForzarFloatParameter(bool validar_bloqueo, float valor)
    {
        if (colcam != null) colcam.ForzarFloatParameter(validar_bloqueo, valor);
    }

    public void ForzarFloatParameter_True(float valor)
    {
        if (colcam != null) colcam.ForzarFloatParameter(true, valor);
    }

    public void ForzarFloatParameter_False()
    {
        if (colcam != null) colcam.ForzarFloatParameter(false, 0f);
    }

    //EN CAS DE QUE EL NAV MESH AGENT NO PERMETA ANAR A UN LLOC PERQUE NO HI HA NAV MESH SURFACE
    //I BLOQUEJE LES FUNCIONABILITATS DEL COL·LISIONADOR CAMERA O DE LES CAMERES:
    public void DeshabilitarNMA()
    {
        agentNavegador.enabled = false;
    }

    public void HabilitarNMA()
    {
        agentNavegador.enabled = true;
    }

    public void CanviarCamara(Camera novaCamara)
    {
        camaraActual = novaCamara;
    }
    

    //Se accede a este código desde ExplorarState
    public void SnapAlignCharacterWithCamera()
    {
        if(inputMoure.z != 0 || inputMoure.x != 0)
        {
            transform.rotation =    Quaternion.Euler(transform.eulerAngles.x,
                                    camaraActual.transform.eulerAngles.y,
                                    transform.eulerAngles.z);

            float rot = 0;
            //z = delante, x= a los lados

            // si se va en la dirección opuesta de la actual, 
            //rotar inmediatamente; si no, rotación gradual    
            
            //si se va hacia atrás, rotar 180
            if (inputMoure.z < 0) rot = 180;

            //si se va hacia los lados, rotar 90 según el vector horizontal
            if(inputMoure.z == 0) 
                rot += (inputMoure.x / Mathf.Abs (inputMoure.x)) * 90f;
            //en caso contrario, se asumirán horizontal y vertical
            else    
                rot += (Mathf.Atan (inputMoure.x/inputMoure.z)) * 45f;
            
            transform.rotation = Quaternion.Euler(  transform.eulerAngles.x,
                                                    transform.eulerAngles.y + rot,
                                                    transform.eulerAngles.z);

        }
    }
    /*
    void IdentificarQuad()
    {
        RaycastHit idenColisionador;
        if(Physics.Linecast(transform.position, camaraActual.transform.position, out idenColisionador))
        {
            //Debug.Log("Colisió");
            //Comprobar gameobject colisionado con los cuadros hijos
            for(int i = 0; i<4; i++)
                if(idenColisionador.collider.gameObject == transform.GetChild(0).GetChild(i).gameObject)
                    //Debug.Log((float)i/3f);
                    animacion.SetFloat("idenColisionadorCamara", (float)i/3f);
        }

    }*/

    //Funció que executarà la mateixa funció en CinematicaState
    public void CaminarCapDesti_DespresExplorar(Transform desti)
    {
        Debug.Log("Funció CaminarCapDestí de DetectiuStateManager executada");
        if(stateActual != cineState)    CanviarState_Cinematica(true);

        cineState.CaminarCapDesti(this, desti, true);
    }

    public void CaminarCapDesti_NoExplorar(Transform desti)
    {
        Debug.Log("Funció CaminarCapDestí de DetectiuStateManager executada");
        if (stateActual != cineState) CanviarState_Cinematica(true);

        cineState.CaminarCapDesti(this, desti, false);
    }

    public void CanviarState_Exlorar()  
    {
        Debug.Log("Canviar a ExplorarState");
        stateActual = explorarState;
        //Configurar càmara
        FindObjectOfType<ManejaCamares>().CanviarCamara("CameraJugador");
        if(cc == null)
            cc = FindObjectOfType<Controla_Camara>();
        if(sj == null)
             sj = FindObjectOfType<SeguixJugador>();

        //cc.NouEnfoque(this.gameObject.transform, sj.posCamara);
        sj.ActivarCameraExplorar();

        //Fer les comprovacions de si es tenen les claus dels tres dialegs
        enExplorarEvent.Invoke();

        //Delegate
        if (delExplorar != null) delExplorar();
    }
    public void CanviarState_Xarrar()   
    {
        Debug.Log("Canviar a XarrarState");
        if (stateActual == cineState && cineState.isActivatNMA())
            cineState.FiDelCami(this);
        stateActual = xarrarState;
        //La configuració de càmara ho realitzarà el sistema determinat, entre els possibles: 
        //      MainTriggersDialog, per exemple

        //El personatge te que parar de caminar
        //Debug.Log("El personatge deixa de caminar");
        if (animacion == null)
            animacion = transform.GetChild(0).GetComponent<Animator>();
        animacion.SetBool("isCaminant",false);

        //Delegate
        if (delXarrar != null) delXarrar();

    }

    public void CanviarState_Cinematica(bool canviarAnim)
    {
        Debug.Log("Canviar a CinematicaState");
        stateActual = cineState;

        //El personatge te que parar de caminar
        //Debug.Log("El personatge deixa de caminar");
        if (animacion == null)
            animacion = transform.GetChild(0).GetComponent<Animator>();

        // Si es mana false, no es canvien els anims
        if(canviarAnim)
            animacion.SetBool("isCaminant",false);

        //Delegate
        if (delCinematica != null) delCinematica();
    }

    //FUNCIONS PER A CANVIAR PARÀMETRES AMB EL SIGNAL RECEIVER
    //NOMÉS SI ESTÀ EN CINEMATICA STATE !!
    public void SetIsCaminant(bool intro)
    {
        //Debug.Log("SetIsCaminant()");
        if (stateActual == cineState)
        {
            Debug.Log("SetIsCaminant() Se cambia el parámetro");
            if (animacion == null)
                animacion = transform.GetChild(0).GetComponent<Animator>();
            animacion.SetBool("isCaminant", intro);
        }
    }

    public void SetIsXarrant(bool intro)
    {
        Debug.Log("SetIsXarrant()");
        if (stateActual == cineState)
        {
            Debug.Log("SetIsXarrant() Se cambia el parámatro");
            if (animacion == null)
                animacion = transform.GetChild(0).GetComponent<Animator>();
            animacion.SetBool("isXarrant", intro);
        }
    }

    //Per anunciar a altres classes en quin estat està el DetectiuStateManager
    public bool ActivatExploraState() { return stateActual == explorarState; }
    public bool ActivatXarrarState() { return stateActual == xarrarState; }
    public bool ActivatCinematicaState() { return stateActual == cineState; }


}
