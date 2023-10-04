using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_IdiomaStart : UI_Menu
{
    // Start is called before the first frame update
    public UI_MenuInici menuInici;
    public IniciaTimeline cinematicaMenuInici;
    void Start()
    {
        if(menuInici==null)
        {
            menuInici = FindObjectOfType<UI_MenuInici>();
            if (menuInici == null) Debug.Log("UI_IdiomaStart Start() No hi ha UI_MenuInici!!!");
        }
    }

    // Update is called once per frame
    public void SeleccionarIdioma(int numIdioma)
    {
        Data_Idioma.instance.CambiarIdioma(numIdioma);
        //Pasar al menu de inicio
        if (cinematicaMenuInici == null)
            ChangeMenu(menuInici);
        else
        {
            BackToGame();
            cinematicaMenuInici.StartTimeline();
        }
    }
}
