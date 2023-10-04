using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaCamaraLliure : MonoBehaviour
{
    public Transform objectiu; //Personaje jugador
    //Posició de la càmara mentre el jugador està en ExplorarState
    public Transform posCamara;
    
    private Vector3 vectorMirar;

    //Posició respecte al jugador(variable obsoleta, s'utilitzarà si no s'ha
    //trobat a posCamaraExplorar)
    //private Vector3 diferenciaPosicion = new Vector3(-32,9,0); 
    private Vector3 puntoOptimo;//Punto donde se posicionará la cámara

    public float velocitatLerp = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        vectorMirar = objectiu.transform.position - transform.position;

        //Per a la rotació
        Quaternion giroOptimo = Quaternion.LookRotation(vectorMirar);
        transform.rotation = Quaternion.Lerp(transform.rotation, giroOptimo, 0.05f);

        //Per a la posició
        /*if(posCamara==null)
            puntoOptimo = objectiu.transform.position + diferenciaPosicion;
        else */
            puntoOptimo = posCamara.position;
        transform.position = Vector3.Lerp(transform.position, puntoOptimo, velocitatLerp);
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

    //COPIAT DIRECTAMENT DE CONTROLA_CAMARA

    /*public void NouEnfoque(Vector3 introObjectiu, Vector3 introPosCamara)
    {
        //Debug.Log("Controla_Camara NouEnfoque(Vector3)");

        if (introObjectiu == null || introPosCamara == null)
        {
            Debug.Log("Controla_Camara NouEnfoque(Vector3) introObjectiu==null || introPosCamara==null");
            return;
        }

        vuit1.transform.position = introObjectiu;
        objectiu = vuit1.transform;
        vuit2.transform.position = introPosCamara;
        posCamara = vuit2.transform;

    }*/

    public void NouPosCamera(Transform introPosCamara)
    {
        //Debug.Log("Controla_Camara NouPosCamera() ");
        if (introPosCamara != null)
            posCamara = introPosCamara;
    }

    public void NouObjectiu(Transform introObjectiu)
    {
        if (introObjectiu != null)
            objectiu = introObjectiu;
    }


}
