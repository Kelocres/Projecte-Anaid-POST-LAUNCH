using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public UI_Menu[] menus;
    private UI_Menu menuActivat;

    //Per a comprovar que la tecla d'un menú ha sigut pulsada
    private KeyCode[] keycodes;

    //private Animator animPantallaNegra;
    public IniciaTimeline cinematicaCarregarEscena;
    public GameObject pantallaNegra;
    private SoundManager soundManager;
    //public bool loadSceneAfterFadeOut = false;
    public string sceneToLoad = "";


    // Start is called before the first frame update
    void Start()
    {
        keycodes = new KeyCode[menus.Length];
        for(int i=0; i< menus.Length; i++)
        {
            menus[i].SetButtons();
            menus[i].backToGame += BackToGame;
            menus[i].changeMenu += ChangeMenu;

            keycodes[i] = menus[i].keycode;

            //En cas de que el menú està activat, serà el menú actiu
            if(menus[i].gameObject.activeSelf)
            {
                if (menuActivat == null) menuActivat = menus[i];
                //Si ja hi ha un activat, es desactivarà el gameobject
                else menus[i].gameObject.SetActive(false);
            }
        }

        // Obtindre Animator de la Pantalla Negra
        /*GameObject pantallaNegra = GameObject.FindGameObjectWithTag("UI_PantallaNegra");
        if (pantallaNegra != null)
            animPantallaNegra = pantallaNegra.GetComponent<Animator>();
        */

        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //Comprobar cada botó
        for(int i=0; i<keycodes.Length; i++)
        {
            if(Input.GetKeyDown(keycodes[i]))
            {
                Debug.Log("MenuManager Update() keycode[" + i + "] down");
                if (menuActivat == null)
                    ShowMenu(i);
                //DEGUT A UN BUG QUE ES PRODUIX AL EIXIR D'ALGUNS MENÚS AMB LA TECLA
                //NOMÉS ES PERMET FER LES EIXIDES O CANVIS DE MENÚ
                //AMB ELS BOTONS DEL CANVAS
                
                else if (menuActivat == menus[i])
                    BackToGame();
                else
                    ChangeMenu(i);
                
            }
        }
    }

    
    public void ShowMenu(int numMenu)
    {
        Debug.Log("MenuManager ShowMenu(int)");
        menuActivat = menus[numMenu];

        menuActivat.gameObject.SetActive(true);
        if (menuActivat.pararDeltaTime)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;

        //Bloquetjar CursorSelector mentre es mostren els menús
        //CursorSelector.BlockByMenu();
    }

    public void ShowMenu(UI_Menu menu)
    {
        int index = GetMenuNumber(menu);
        if (index == -1 || index >= menus.Length)
            Debug.Log("MenuManager ShowMenu(UI_Menu) menu not found in array!!!");
        else ShowMenu(index);
    }

    public void BackToGame()
    {
        if (menuActivat == null) return;

        menuActivat.gameObject.SetActive(false);
        menuActivat = null;

        foreach (UI_Menu menu in menus)
            menu.HideAlternativeGroup();

        Time.timeScale = 1f;

        //Desbloquetjar CursorSelector
        //CursorSelector.UnblockByMenu();
    }

    public void ChangeMenu(int numMenu)
    {
        Debug.Log("MenuManager ChangeMenu(" + numMenu + ")");
        menuActivat.gameObject.SetActive(false);

        menuActivat = menus[numMenu];
        menuActivat.gameObject.SetActive(true);
        if (menuActivat.pararDeltaTime)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    //Éste es crida des dels botons dels menus
    public void ChangeMenu(UI_Menu newMenu)
    {
        int index = GetMenuNumber(newMenu);
        if (index == -1 || index >= menus.Length)
            Debug.Log("MenuManager ShowMenu(UI_Menu) menu not found in array!!!");
        else ChangeMenu(index);
    }

    //Per a carregar escena després d'una cinemàtica (fadeIn)
    public void LoadScene()
    {
        //if(loadSceneAfterFadeOut && sceneToLoad!="")
        Debug.Log("MenuManager LoadScene()");
        SceneManager.LoadScene(sceneToLoad);
    }

    public void LoadSceneAfterFadeOut(string nomEscena)
    {
        Debug.Log("MenuManager LoadSceneAfterFadeOut()");
        //BackToGame();
        Time.timeScale = 1f;

        //Tancar el ManejaDialeg
        ManejaDialeg md = FindObjectOfType<ManejaDialeg>();
        if(md!=null)
        {
            md.AcabarDialeg_Cinematica();
        }

        if (nomEscena!=null && nomEscena!= "")
            sceneToLoad = nomEscena;
        else
        {
            Debug.Log("MenuManager LoadSceneAfterFadeOut() nomEscena no valid!!");
            return;
        }
        //loadSceneAfterFadeOut = true;


        if (pantallaNegra != null)
             pantallaNegra.SetActive(true);
         //else
         //    LoadScene();
        
        if (cinematicaCarregarEscena != null)
            cinematicaCarregarEscena.StartTimeline();
        else
            Debug.Log("MenuManager LoadSceneAfterFadeOut() cinematicaCarregarEscena no existix!!!");

        if (soundManager != null)
            soundManager.SoftStopMusic(null);
        else
            Debug.Log("MenuManager LoadSceneAfterFadeOut() no s'ha pogut canviar la musica");

    }

    private int GetMenuNumber(UI_Menu searchMenu)
    {
        for(int i=0; i<menus.Length; i++)
        {
            if (menus[i] == searchMenu)
                return i;
        }

        return -1;
    }
}
