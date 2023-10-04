using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_CanviaCamera : ActivadorEvent
{
    private ManejaCamares mc;
    //public Camera camera;
    public NombraCamera nombraCamera;

    //En el cas opcional de que es busque canviar l'objectiu o la posició
    public ControlaCamaraLliure controlaCamaraLliure;
    public Controla_Camara controlaCamara;

    public Transform nouPosCamara;
    public Transform nouObjectiu;

    // Start is called before the first frame update
    void Start()
    {
        mc = FindObjectOfType<ManejaCamares>();
    }

    // Update is called once per frame
    public override void Activar()
    {
        if (mc == null)
            Debug.Log("AE_CanviaCamera Activar() mc == null");
        else if (nombraCamera == null)
            Debug.Log("AE_CanviaCamera Activar() nombraCamera == null");
        else if (nombraCamera.camera == null)
            Debug.Log("AE_CanviaCamera Activar() nombraCamera.camera == null");
        else if (nombraCamera.nom == null || nombraCamera.nom == "")
            Debug.Log("AE_CanviaCamera Activar() nombraCamera.nom no valid");
        else
        {
            Debug.Log("AE_CanviaCamera Activar() En marxa!!");
            mc.CanviarCamara(nombraCamera.nom);
        }

        //Si hi ha Controla_Camara o ControlaCamaraLliure, es tractarà de canviar
        //de posició o objectiu
        if (controlaCamara != null)
        {
            controlaCamara.NouPosCamera(nouPosCamara);
            controlaCamara.NouObjectiu(nouObjectiu);
        }
        else if (controlaCamaraLliure != null)
        {
            controlaCamaraLliure.NouPosCamera(nouPosCamara);
            controlaCamaraLliure.NouObjectiu(nouObjectiu);
        }
    }
}
