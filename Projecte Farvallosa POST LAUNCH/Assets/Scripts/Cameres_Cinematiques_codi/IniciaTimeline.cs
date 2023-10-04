using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class IniciaTimeline : MonoBehaviour
{
    public PlayableDirector director;
    public ManejaDialeg md;

    //AQUEST OBJECTE NO S'UTILITZARÀ PER A COL·LISSIONAR AMB CINETRIGGERS!!!
    public BoxCollider collider;
    
    //Per a asociar durant la partida el director
    //NO ES NECESSARI: CADA TIMELINE TINDRA EL SEU DIRECTOR EN EL MATEIX GAMEOBJECT
    //public string tagDirector;
    // Per si l'animació es vol repetir més vegades o cap
    public bool repetir = false;

    public CineTriggerScript [] cineTriggers;
    
    //Per si, al acabar la cinemàtica, es passa a un diàleg
    public bool continuarAmbDialeg = false;
    public Dialeg dialeg;
    public FaceCameraPersonajes [] personatges;

    //Si es vol canviar l'anim de Detectiu al escomençar la cinematica
    public bool canviarAnimDetectiu = true;

    //Si es vol utilitzar la Pantalla Negra, es deurà activar abans de la cinemàtica i
    //desactivar quan s'acabe
    public GameObject PantallaFadeIn;
    public bool iniciFadeIn = false;
    public GameObject PantallaFadeOut;
    public bool iniciFadeOut = false;

    void Awake()
    //void Start()
    {
        //collider.GetComponent<BoxCollider>(); 
        director = GetComponent<PlayableDirector>();
        if (director != null) InitDirector();

        md = FindObjectOfType<ManejaDialeg>();

        foreach (CineTriggerScript ct in cineTriggers)
            ct.finalCinematica += TancarCTs;
    }

    private void InitDirector()
    {
        //Debug.Log()
        //NO ES NECESSARI: CADA TIMELINE TINDRA EL SEU DIRECTOR EN EL MATEIX GAMEOBJECT
        /*if (tagDirector != null && tagDirector != "")
            director = GameObject.FindGameObjectWithTag(tagDirector).GetComponent<PlayableDirector>();
        else
            director = FindObjectOfType<PlayableDirector>();*/

        director.played += Director_Played;
        director.stopped += Director_Stoped;
    }

    /*void Start()
    {
      collider.GetComponent<BoxCollider>();
    }*/

    //Director_Played will hide the UI panel used to start the Timeline once the Timeline has started playing
    private void Director_Played(PlayableDirector obj)
    {
        //controlPanel.SetActive(false);
        if(collider != null) collider.enabled = false;
        //Pantalla negra
        if (PantallaFadeIn != null && iniciFadeIn)
        {
            //Esta part es manejaria millor amb dos pantalles negres: una per a FadeIn i altra per a FadeOut
            PantallaFadeIn.SetActive(true);
            //if (iniciNegre) PantallaNegra.GetComponent<Animator>().SetTrigger("instaOpac");
            //else PantallaNegra.GetComponent<Animator>().SetTrigger("instaTrans");
        }
        else if (PantallaFadeOut != null && iniciFadeOut)
        {
            //Esta part es manejaria millor amb dos pantalles negres: una per a FadeIn i altra per a FadeOut
            PantallaFadeOut.SetActive(true);
            //if (iniciNegre) PantallaNegra.GetComponent<Animator>().SetTrigger("instaOpac");
            //else PantallaNegra.GetComponent<Animator>().SetTrigger("instaTrans");
        }

        DetectiuStateManager dsm = FindObjectOfType<DetectiuStateManager>();
        if(dsm!=null)
            dsm.CanviarState_Cinematica(canviarAnimDetectiu);
    }

    //Director_Stoped will show the UI panel after the Timeline has stopped
    private void Director_Stoped(PlayableDirector obj)
    {
        Debug.Log("IniciaTimeline Director_Stoped()");
        //Pantalla negra
        if (PantallaFadeIn != null) PantallaFadeIn.SetActive(false);
        if (PantallaFadeOut != null) PantallaFadeOut.SetActive(false);

        //controlPanel.SetActive(true);
        if (repetir)
        {
            if (collider != null) collider.enabled = true;
        }

        if (continuarAmbDialeg)// && dialeg!=null && personatges.Length != 0)
        {
            Debug.Log("IniciaTimeline Director_Stoped() Es passara a Dialeg");
            if (dialeg == null)
            {
                Debug.Log("IniciaTimeline Director_Stoped() es vol passar a XarrarState però el Dialeg es null");
                return;
            }
            if (personatges.Length == 0)
            {
                Debug.Log("IniciaTimeline Director_Stoped() es vol passar a XarrarState però no hi han personatges assignats");
                return;
            }
            //Enviar el dialeg al ManejaDialeg

            if (md == null)
            {
                Debug.Log("No s'ha trobat el manejaDialeg");
                return;
            }
            md.IniciaDialog(dialeg, personatges);
            //Canviar Detectiu de CinematicaState a XarrarState
            FindObjectOfType<DetectiuStateManager>().CanviarState_Xarrar();

            TancarCTs();

        }
      
        
        
    }

    public void AcabarTimeline()
    {
        Debug.Log("IniciaTimeline AcabarTimeline()");
        director.Stop();
        //Director_Stoped(director);
    }

    private void TancarCTs()
    {
        Debug.Log("IniciaTimeline TancarCTs()");
        if (cineTriggers.Length == 0) return;

        foreach (CineTriggerScript ct in cineTriggers)
            ct.activat = false;
    }

    public void StartTimeline()
    {
      //Activar tots els CineTriggers asociats al timeline
      foreach(CineTriggerScript ct in cineTriggers)
        if(ct!=null)
          ct.activat = true;
        if (director == null)
        {
            Debug.Log("IniciaTimeline StartTimeline() llamar InitDirector()");
            InitDirector();
        }
        director.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
      if(other.tag=="Player")
        StartTimeline();
    }
}
