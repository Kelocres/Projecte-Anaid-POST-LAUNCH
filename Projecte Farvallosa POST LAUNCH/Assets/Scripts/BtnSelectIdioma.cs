using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class BtnSelectIdioma : TextSelectIdioma
{
    //El texte del botó, es canviarà el valor string pel de selectText
    protected Text textBoto;
    public Button boto;
    public Sprite spr_original;
    public Sprite spr_baixCursor;
    public Sprite spr_pulsat;

    private EventTrigger et;
    private SoundManager soundManager;
   
    void Start()
    {
        //txtCanvas = GetComponentInChildren<Text>();
        soundManager = FindObjectOfType<SoundManager>();

        if (GetComponent<Button>() != null)
        {
            boto = GetComponent<Button>();
            Canviar_Original();
            //Canviar_BaixCursor();
            //Canviar_Pulsat();
        }

        //Afegir un component EventTrigger per als canvis d'imatge
        et = GetComponent<EventTrigger>();
        if (et==null)
        {
            et = gameObject.AddComponent(typeof(EventTrigger)) as EventTrigger;
            //GameObject obj = this.gameObject;

            //Afegir events
            AddEvent(EventTriggerType.PointerEnter, delegate { Canviar_BaixCursor(); });
            AddEvent(EventTriggerType.PointerExit, delegate { Canviar_Original(); });
            AddEvent(EventTriggerType.PointerClick, delegate { Canviar_Pulsat(); });
        }

        //Per a quan BtnDecisio herede aquest codi
        if(GetComponentInChildren<Text>()!=null)
        {
            textBoto = GetComponentInChildren<Text>();
            //Debug.Log("PROBA BtnSelectIdioma: "+textBoto.text);
            ActualitzaTextBoto();
        }
    }

    protected void AddEvent(EventTriggerType type, UnityAction<BaseEventData> action)
    {
        //EventTrigger trigger = obj.GetComponent<EventTrigger>();

        //Crear un nou Trigger per afegir-lo al EventTrigger
        var eventTrigger = new EventTrigger.Entry();

        //Configurar el seu tipus
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        et.triggers.Add(eventTrigger);
    }

    // Es cridarà al començar el joc, i cada vegada que es canvie el idioma
    public void ActualitzaTextBoto()
    {
        ActualitzaSelectText();
        if (textBoto != null)
        {
            textBoto.text = selectText;
            textBoto.color = new Color(1, 1, 1);
        }
    }

    private void OnEnable()
    {
        //Tornar al color original quan es torne al menú
        Canviar_Original();
    }

    public override void DelActualitzaSelectText()
    {
        Debug.Log("BtnSelectIdioma DelActualitzaSelectText()");
        ActualitzaSelectText();
        if (textBoto != null) textBoto.text = selectText;
    }

    //Per a canviar la imatge del botó mentre el ratolí està damunt
    //TUTORIAL: https://www.youtube.com/watch?v=S5Lp8vCb6hU
    public void Canviar_BaixCursor()
    {
        if (spr_baixCursor != null && boto!=null && boto.image!=null)
            boto.image.sprite = spr_baixCursor;
        else
            Debug.Log("BtnSelectIdioma Canviar_BaixCursor() ERROR!!! spr_baixCursor es null");

        //if (txtCanvas != null)
        //    txtCanvas.color = new Color(1, 1, 0);
        if (textBoto != null)
            textBoto.color = new Color(1, 1, 0);
    }

    public void Canviar_Original()
    {
        if (spr_original != null && boto != null && boto.image != null)
            boto.image.sprite = spr_original;
        else
            Debug.Log("BtnSelectIdioma Canviar_Original() ERROR!!! spr_original es null");

        //if (txtCanvas != null)
        //    txtCanvas.color = new Color(1, 1, 1);
        if (textBoto != null)
            textBoto.color = new Color(1, 1, 1);

    }

    public void Canviar_Pulsat()
    {
        if (spr_pulsat != null && boto != null && boto.image != null)
            boto.image.sprite = spr_pulsat;
        else
            Debug.Log("BtnSelectIdioma Canviar_Pulsat() ERROR!!! spr_pulsat es null");

        //if (txtCanvas != null)
        //    txtCanvas.color = new Color(1, 1, 0.5f);
        if (textBoto != null)
            textBoto.color = new Color(1, 1, 0.5f);

        if (soundManager != null)
            soundManager.PlayButtonSound();
    }

    public void CopiarEstetica(BtnSelectIdioma estetica)
    {
        if (boto != null && boto.image != null)
        {
            spr_original = estetica.spr_original;
            Canviar_Original();
            spr_baixCursor = estetica.spr_baixCursor;
            Canviar_BaixCursor();
            spr_pulsat = estetica.spr_pulsat;
            Canviar_Pulsat();
        }
    }
}
