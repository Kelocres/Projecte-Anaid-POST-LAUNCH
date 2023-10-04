using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Acte3_ReposicionarPersonatges : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] personatges;
    public Transform[] novesPosicions;

    private bool potFuncionar;

    //Per a la CameraJugador (interactuar desde Controla_Camara)
    public Transform cameraNovaPosicio;
    public Transform cameraNouObjectiu;

    public Personatge_Anim[] anims;

    void Start()
    {
        potFuncionar = false;
        if (personatges == null || novesPosicions == null)
            Debug.Log("Acte3_ReposicionarPersonatges Start() arrays no inicialitzades!!!");
        else if (personatges.Length != novesPosicions.Length)
            Debug.Log("Acte3_ReposicionarPersonatges Start() arrays amb número d'elements diferent!!!");
        else
        {
            potFuncionar = true;
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Reposicionar()
    {
        if (potFuncionar)
        {
            Debug.Log("Acte3_ReposicionarPersonatges Reposicionar() " + personatges.Length + " elements en els arrays");
            for (int i=0; i<personatges.Length; i++)
            {
                if(personatges[i]==null || novesPosicions[i]==null)
                {
                    Debug.Log("Acte3_ReposicionarPersonatges Reposicionar() element "+i+" no existent!!!");
                    return;
                }

                //Si el personatge te un NavMeshAgent actiu, fara falta
                //deshabilitar-ho, canviar la posició i habilitar-ho de nou
                NavMeshAgent navme = personatges[i].GetComponentInChildren<NavMeshAgent>();

                if(navme!=null)
                {
                    navme.isStopped = true;
                    navme.enabled = false;
                }
                personatges[i].transform.position = novesPosicions[i].position;

                if (navme != null)
                    navme.enabled = true;
            }

            //Reajustar Controla_Camera
            if(cameraNouObjectiu!=null && cameraNovaPosicio!=null)
            {
                Controla_Camara[] ccs = FindObjectsOfType<Controla_Camara>();
                if (ccs.Length == 0)
                    Debug.Log("Acte3_ReposicionarPersonatges Reposicionar() No hi ha Controla_Camara!!");
                else if (ccs.Length > 1)
                    Debug.Log("Acte3_ReposicionarPersonatges Reposicionar() Hi han " + ccs.Length + " Controla_Camara, el programa no funcionarà");
                else
                    ccs[0].NouEnfoque(cameraNouObjectiu, cameraNovaPosicio);
            }
            else
                Debug.Log("Acte3_ReposicionarPersonatges Reposicionar() Transforms per a Controla_Camera no trobats!!!");

            if(anims != null && anims.Length!=0)
            {
                Debug.Log("Acte3_ReposicionarPersonatges Reposicionar() Ajustar FaceCameraPersonajes");
                FaceCameraPersonajes[] tots = FindObjectsOfType<FaceCameraPersonajes>();
                foreach(FaceCameraPersonajes fcp in tots)
                {
                    fcp.IniciDialeg();
                    fcp.CanviaDialegAnimacion(anims);
                }

            }
        }
    }
}
