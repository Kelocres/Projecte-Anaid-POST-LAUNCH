using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitolsMenuInici : MonoBehaviour
{
    public GameObject[] titols;
    private int titolActual = 0;
    // Si es un Awake, no arriba a arreplegar el Data_Idioma
    void Start()
    {
        Data_Idioma di = Data_Idioma.instance;
        if (di != null)
        {
            //Borrar previament, per si ja el tenia
            di.delCanviarIdioma -= ActualitzaTitol;
            //Tornar a afergir-ho
            di.delCanviarIdioma += ActualitzaTitol;
            Debug.Log("TitolsMenuInici Start() delegate aplicat!!");
        }

        ActualitzaTitol();
    }

    // Update is called once per frame
    public void ActualitzaTitol()
    {
        Debug.Log("TitolsMenuInici ActualitzaTitol()");
        if (titols == null || titols.Length <= 0) return;

        if(titols[titolActual]!=null)
            titols[titolActual].SetActive(false);
           
        if(Data_Idioma.instance != null)
        {
            Debug.Log("TitolsMenuInici ActualitzaTitol() idioma = " + Data_Idioma.instance.IdiomaActual());
            titolActual = Data_Idioma.instance.numIdiomaActual;
            Debug.Log("TitolsMenuInici ActualitzaTitol() titolActual = " + titolActual);
        }
            
        if (titols[titolActual] != null)
        {
            Debug.Log("TitolsMenuInici ActualitzaTitol() Activar titols["+titolActual+"]");
            titols[titolActual].SetActive(true);
        }
        

    }
}
