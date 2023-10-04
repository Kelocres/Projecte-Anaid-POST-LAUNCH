using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_IdiomaGameplay : UI_Menu
{
    // Start is called before the first frame update
    public UI_Menu tornarAMenu;
    void Start()
    {
        
        if (tornarAMenu == null) Debug.Log("UI_IdiomaStart Start() No hi ha tornarAMenu!!!");
        
    }

    // Update is called once per frame
    public void SeleccionarIdioma(int numIdioma)
    {
        Data_Idioma.instance.CambiarIdioma(numIdioma);
        //Pasar al menu de inicio
        ChangeMenu(tornarAMenu);
    }
}
