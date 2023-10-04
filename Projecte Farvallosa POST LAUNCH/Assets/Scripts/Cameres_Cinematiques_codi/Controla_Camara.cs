using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controla_Camara : MonoBehaviour
{
    public Transform objectiu; //Personaje jugador
    //Posició de la càmara mentre el jugador està en ExplorarState
    public Transform posCamara;
    
    private Vector3 vectorMirar;

    //Posició respecte al jugador(variable obsoleta, s'utilitzarà si no s'ha
    //trobat a posCamaraExplorar)
    private Vector3 diferenciaPosicion = new Vector3(-32,9,0); 
    private Vector3 puntoOptimo;//Punto donde se posicionará la cámara

    //En cas de que escride a la funció NouEnfoque amb Vector3, s'utlitzaràn aquestos gameobjects vuits:
    public GameObject vuit1;
    public GameObject vuit2;

    // Start is called before the first frame update
    void Start()
    {
        //objectiu = FindObjectOfType<Controlador_Jugador>().transform;
        //objectiu = FindObjectOfType<DetectiuStateManager>().transform;
        DetectiuStateManager detectiu = FindObjectOfType<DetectiuStateManager>();
        if(detectiu!=null)
        {
            objectiu = detectiu.transform;
        }


        SeguixJugador sj = FindObjectOfType<SeguixJugador>();
        if (sj==null)
            Debug.Log("PosCamaraExplorar no encontrado");
        else
        {
            posCamara = sj.posCamaraExplorar;
        }

        if(posCamara!=null)
        {
            transform.position = posCamara.position;
        }

        if (objectiu != null)
        {
            vectorMirar = objectiu.transform.position - transform.position;

            //Per a la rotació
            Quaternion giroOptimo = Quaternion.LookRotation(vectorMirar);
            Debug.Log("Controla_Camara Start() transform.rotation = giroOptimo;");
            transform.rotation = giroOptimo;
        }

        //RECORDA QUE EL GAMEOBJECT AMB EL COMPONENT ManejaPartida HA DE TINDRE EL TAG ManejaPartida
        GameObject mp = GameObject.FindGameObjectWithTag("ManejaPartida");
        if (mp != null)
        {
            vuit1 = mp.transform.Find("SolsTransform1").gameObject;
            vuit2 = mp.transform.Find("SolsTransform2").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectiu != null)
        {
            vectorMirar = objectiu.transform.position - transform.position;

            //Per a la rotació
            Quaternion giroOptimo = Quaternion.LookRotation(vectorMirar);
            transform.rotation = Quaternion.Lerp(transform.rotation, giroOptimo, 0.05f);
        }
        //Per a la posició
        if(posCamara==null && objectiu!=null)
            puntoOptimo = objectiu.transform.position + diferenciaPosicion;
        else 
            puntoOptimo = posCamara.position;
        transform.position = Vector3.Lerp(transform.position, puntoOptimo, 0.1f);
    }

    //Versió que treballa amb Transforms
    public void NouEnfoque(Transform introObjectiu, Transform introPosCamara)
    {
        //Debug.Log("Controla_Camara NouEnfoque(Transform)");

        if (introObjectiu == null || introPosCamara == null)
        {
            Debug.Log("Controla_Camara NouEnfoque(Vector3) introObjectiu==null || introPosCamara==null");
            return;
        }

        objectiu = introObjectiu;
        posCamara = introPosCamara;
    }

    public void NouEnfoque(Vector3 introObjectiu, Vector3 introPosCamara)
    {
        //Debug.Log("Controla_Camara NouEnfoque(Vector3)");

        if(introObjectiu==null || introPosCamara==null)
        {
            Debug.Log("Controla_Camara NouEnfoque(Vector3) introObjectiu==null || introPosCamara==null");
            return;
        }

        vuit1.transform.position = introObjectiu;
        objectiu = vuit1.transform;
        vuit2.transform.position = introPosCamara;
        posCamara = vuit2.transform;

    }

    public void NouPosCamera(Transform introPosCamara)
    {
        //Debug.Log("Controla_Camara NouPosCamera() ");
        if(introPosCamara!=null)
        posCamara = introPosCamara;
    }

    public void NouObjectiu(Transform introObjectiu)
    {
        if (introObjectiu != null)
            objectiu = introObjectiu;
    }
}
