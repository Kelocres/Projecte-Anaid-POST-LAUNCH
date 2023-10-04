using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Personatge_Anim : MonoBehaviour
{
    public string idPersonatge;
    public string anim;
    //public int canviaOrientacio = -1; //NoCanviar = -1, CapDreta = 0, CapEsquerra = 1, InvertirActual = 2

    //TOT LO DE BAIX SOLS ES PODRIA FER SI LA CLASS ES MONOBEHAVIOUR
    //PER A CONVERTIR UN JSON A PERSONATGE_ANIM, EMPRAREM A DATAPERSONATGE_ANIM
    
    
    public enum AnimOrientacio : short { NoCanviar = 0, CapDreta = -1, CapEsquerra = 1, InvertirActual = 2};
    //Variable de l'enum creat (el valor per defecte és NoCanviar):
    public AnimOrientacio canviOrientacio = AnimOrientacio.NoCanviar;
    //Com altres scripts no poden treballar amb enums aliens, les comparacions es
    //realitzaràn en aquest script:
    public bool CO_EsNoCanviar() { return canviOrientacio == AnimOrientacio.NoCanviar; }
    public bool CO_EsCapDreta() { return canviOrientacio == AnimOrientacio.CapDreta; }
    public bool CO_EsCapEsquerra() { return canviOrientacio == AnimOrientacio.CapEsquerra; }
    public bool CO_EsInvertirActual() { return canviOrientacio == AnimOrientacio.InvertirActual; }
    /*
    public Personatge_Anim(DataPersonatge_Anim data)
    {
        //Debug.Log("Personatge_Anim() Crear anim");
        idPersonatge = data.idPersonatge;
        anim = data.anim;
        canviOrientacio = (AnimOrientacio)data.canviaOrientacio;
    }*/

    public void SetPersonatge_Anim(DataPersonatge_Anim data)
    {
        //Debug.Log("Personatge_Anim() Crear anim");
        idPersonatge = data.idPersonatge;
        anim = data.anim;
        canviOrientacio = (AnimOrientacio)data.canviaOrientacio;
    }

    public DataPersonatge_Anim ConvertirData()
    {
        Debug.Log("Personatge_Anim ConvertirData()");
        DataPersonatge_Anim data = new DataPersonatge_Anim();
        data.idPersonatge = idPersonatge;
        data.anim = anim;
        data.canviaOrientacio = (int)canviOrientacio;

        Debug.Log("Personatge_Anim ConvertirData() acabat amd CO="+data.canviaOrientacio);

        return data;
    }

    public Personatge_Anim(string dataId, string dataAnim, int dataCanvi)
    {
        idPersonatge = dataId;
        anim = dataAnim;
        canviOrientacio = (AnimOrientacio)dataCanvi;
    }

    public int GetCanviaOrientacio()
    {
        return (int)canviOrientacio;
    }



    /*public void FerAnim(FaceCameraPersonajes fcp)
    {
        if (fcp == null || fcp.idPersonatge != idPersonatge)
        {
            //Buscar el FCP segons el id
            FaceCameraPersonajes[] personatges = FindObjectsOfType<FaceCameraPersonajes>();
            bool trobat = false;
            foreach (FaceCameraPersonajes personatge in personatges)
            {
                if (fcp.idPersonatge == idPersonatge)
                {
                    fcp = personatge;
                    trobat = true;
                    break;
                }
            }

            //Si no s'ha trobat, no es fa res
            if (!trobat)
            {
                Debug.Log("");
            }
        }
    }*/

}

[System.Serializable]
public class DataPersonatge_Anim
{
    public string idPersonatge;
    public string anim;
    public int canviaOrientacio;
}
