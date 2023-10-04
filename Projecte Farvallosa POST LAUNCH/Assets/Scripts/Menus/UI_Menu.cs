using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UI_Menu : MonoBehaviour
{
    private bool alreadySetButtons = false;
    public bool pararDeltaTime = true;

    //El menú fisic com a GameObject
    public GameObject menuObject;
    //La tecla amb la qual es cridarà este menú
    public KeyCode keycode;

    //public MenuButton[] menuButtons;

    //Delegate per a tornar al joc
    public delegate void delBackToGame();
    public event delBackToGame backToGame;

    //Delegate per a canviar a un altre menu
    public delegate void delChangeMenu(UI_Menu menu);
    public event delChangeMenu changeMenu;

    //Grups d'elements (botons sobretot) que s'amagaran o mostraran segons
    // la manera en que s'obri el menú
    // (exemple: la bústia mostra el botó per accedir al MenuEscriureCartes en MenuCartes)
    public AlternativeElementGroup[] alternativeElements;

    //El valor actual que obri (o no) un dels AlternativeElementGroup;
    public string currentWordKey;

    //Delegate per a que la barra d'items sàpiga que està habilitat o deshabilitat
    public delegate void delSignal(bool intro);
    public event delSignal delIsActive;
    

    // Start is called before the first frame update
    void Start()
    {
        currentWordKey = "";
        SetButtons();

    }

    //Ho cridarà cada menú que necessite controlar la barra d'items (UI_InventariJugador, UI_MenuObjectius)
    public void AllowItemsBarControl()
    {
        //FindObjectOfType<ItemsBarScript>().SetMenuInventari(this);
        if (delIsActive != null) delIsActive(true);
    }

    private void OnEnable()
    {
        if (delIsActive != null) delIsActive(true);
    }

    private void OnDisable()
    {
        if (delIsActive != null) delIsActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void SetButtons()
    {
        if (alreadySetButtons) return;

        if(menuObject == null)
            menuObject = this.gameObject;

        if(keycode==null)
        {
            Debug.Log("UI_Menu SetButtons() NO KEYCODE!!!");
        }

        /*
        if(menuButtons.Length == 0)
        {
            Debug.Log("UI_Menu SetButtons() NO BUTTONS!!!");
            alreadySetButtons = true;
            return;
        }
        
        for(int i=0; i<menuButtons.Length; i++)
        {
            string infobutton = menuButtons[i].SetButton();
            if (infobutton == "nobtn_notag")
                Debug.Log("UI_Menu SetButtons() menuButtons[" + 1 + "] has no tag and no button!!!");
            else if(infobutton == "nobtn")
                Debug.Log("UI_Menu SetButtons() menuButtons[" + 1 + "] with tag ="+menuButtons[i].tag+" found no button!!!");
            else if (infobutton == "noevent")
                Debug.Log("UI_Menu SetButtons() menuButtons[" + 1 + "] has no Unity event!!!");

        }*/

        //Amagar tots els grups alternatius
        foreach (AlternativeElementGroup group in alternativeElements)
            foreach (GameObject obj in group.elements)
                obj.SetActive(false);

        alreadySetButtons = true;
    }

    public void BackToGame()
    {
        backToGame?.Invoke();
    }

    //public void ChangeMenu(MenuButton pressedButton)
    public void ChangeMenu(UI_Menu newMenu)
    {
        //Debug.Log("UI_Menu ChangeMenu()");
        if (changeMenu != null /*&& pressedButton != null*/ && newMenu != null)
            changeMenu(newMenu);
    }

    public void ShowAlternativeGroup(string wordKey)
    {
        if (alternativeElements == null || alternativeElements.Length == 0) return;

        if (wordKey == "") return;

        foreach(AlternativeElementGroup group in alternativeElements)
            if(group.wordKey == wordKey)
            {
                foreach (GameObject obj in group.elements)
                    obj.SetActive(true);

                currentWordKey = wordKey;
                break;
            }
    }

    public void HideAlternativeGroup()
    {
        if (currentWordKey == "") return;

        foreach (AlternativeElementGroup group in alternativeElements)
            foreach (GameObject obj in group.elements)
                obj.SetActive(false);

        currentWordKey = "";
    }
}
/*
[System.Serializable]
public struct MenuButton 
{
    public string tag;
    public Button btn;
    public UnityEvent unityEvent;
    //public UI_Menu menu;

    public string SetButton()
    {
        if (btn == null)
        {
            /*
            if(tag == null || tag == "")
            {
                return "nobtn_notag"; //This message is sent to the UI_Menu
            }

            btn = GameObject.FindGameObjectWithTag(tag).GetComponent<Button>();
            if (btn == null)
            * /
                return "nobtn";
        }

        if (unityEvent == null)
            return "noevent";

        btn.onClick.AddListener(unityEvent.Invoke);

        return "fullyset";
    }
}
*/
[System.Serializable]
public struct AlternativeElementGroup
{
    public string wordKey;  //El valor que desbloquetja els botons/elements
    public GameObject[] elements;
}
