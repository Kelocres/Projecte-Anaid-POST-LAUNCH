using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data_Idioma : MonoBehaviour
{
    public static Data_Idioma instance;
    
    [HideInInspector]
    public static readonly string[] idiomesPossibles = { "valencià", "español", "english" };
    //public string idiomaActual;
    public int numIdiomaActual = 0;

    //Delegate per a canviar els textos a l'idioma adequat
    public delegate void delCanviarTextos();
    public event delCanviarTextos delCanviarIdioma;

    private void Awake()
    {
        if (instance != null & instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    // Update is called once per frame
    public void CambiarIdioma(int numIdioma)
    {
        //idiomaActual = idiomesPossibles[numIdioma];
        numIdiomaActual = numIdioma;
        if (delCanviarIdioma != null) delCanviarIdioma();
    }

    public string IdiomaActual() { return idiomesPossibles[numIdiomaActual]; }
}
