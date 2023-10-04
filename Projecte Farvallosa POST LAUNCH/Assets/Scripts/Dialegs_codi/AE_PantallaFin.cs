using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_PantallaFin : ActivadorEvent
{
    public UI_Menu menu;
    private MenuManager menuManager;

    public bool pararMusica = false;
    private SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = FindObjectOfType<MenuManager>();
        if (pararMusica)
            soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    public override void Activar()
    {
        if(menuManager==null)
        {
            Debug.Log("AE_PantallaFin Activar() menuManager==null");
            return;
        }
        if(menu == null)
        {
            Debug.Log("AE_PantallaFin Activar() menu==null");
            return;
        }

        //Parar la música
        if (pararMusica && soundManager != null)
            soundManager.StopMusic();
        //Carregar el menú
        menuManager.ShowMenu(menu);

    }
}
