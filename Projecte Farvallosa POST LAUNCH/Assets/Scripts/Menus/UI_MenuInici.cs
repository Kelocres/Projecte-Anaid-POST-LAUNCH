using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MenuInici : UI_Menu
{
    //public GameObject[] titols;
    //private int titolActual = -1;
    // Start is called before the first frame update
    void Start()
    {
        /*if (titols != null && titols.Length > 0)
        {
            //titols[titolActual].SetActive(false);
            titolActual = Data_Idioma.instance.numIdiomaActual;
            titols[titolActual].SetActive(true);
        }*/
    }
    /*
    private void OnEnable()
    {
        if(titols!=null && titols.Length>0)
        {
            //titols[titolActual].SetActive(false);
            if (Data_Idioma.instance == null)
            {
                if (titolActual == -1)
                {
                    Debug.Log("UI_MenuInici OnEnable() Data_Idioma.instance == null");
                    titolActual = 0;
                }
            }
            else    titolActual = Data_Idioma.instance.numIdiomaActual;

            titols[titolActual].SetActive(true);
        }
    }

    private void OnDisable()
    {
        if (titols != null && titols.Length > 0)
        {
            titols[titolActual].SetActive(false);
        }
    }
    */
    public void CarregarJoc()
    {
        SceneManager.LoadScene("EscenaBarReggae");
    }

    public void EixirJoc()
    {
        Application.Quit();
    }
}
