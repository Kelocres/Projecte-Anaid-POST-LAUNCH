using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextSelectIdioma : MonoBehaviour
{
    //Utilitza el ManejaPartida per a saber quin idioma serà l'indicat
    //ManejaPartida mp;

    //Strings dels textos, mateixa informació però en diferents idiomes
    [SerializeField][TextArea(3,10)] public string textValencia;
    [SerializeField][TextArea(3,10)] public string textCastellano;
    [SerializeField][TextArea(3,10)] public string textEnglish;

    //Variable pública, el text que es mostrarà al jugador
    //[HideInInspector]
    [HideInInspector] public string selectText;

    //Per si es només text en pantalla
    protected Text txtCanvas;

    void Awake()
    {
        //mp = FindObjectOfType<ManejaPartida>();
        Data_Idioma di = Data_Idioma.instance;
        if (di != null) di.delCanviarIdioma += DelActualitzaSelectText;

        txtCanvas = GetComponent<Text>();

        ActualitzaSelectText();
    }

    public void SetTexte(DataTextSelectIdioma data)
    {
        textValencia = data.textValencia;
        textCastellano = data.textCastellano;
        textEnglish = data.textEnglish;

        ActualitzaSelectText();
    }

    //Per a comprovar si el delegate funciona
    public virtual void DelActualitzaSelectText()
    {
        Debug.Log("TextSelectIdioma DelActualitzaSelectText()");
        ActualitzaSelectText();
    }

  
    //Aquesta funció es cridarà al començar el joc, i cada vegada que es canvie l'idioma
    public void ActualitzaSelectText()
    {
        //string idioma = mp.idiomaActual;
        //idioma = Data_Idioma.instance.idiomaActual;
        if (Data_Idioma.instance == null)
        {
            Debug.Log("TextSelectIdioma ActualitzaSelectText() Data_Idioma.instance == null");
            if (selectText == null || selectText == "")
            {
                Debug.Log("TextSelectIdioma ActualitzaSelectText() Canviar selectText a valencià per defecte");
                selectText = textValencia;
            }
        }
        else
        {
            string idioma = Data_Idioma.instance.IdiomaActual();
            //Debug.Log("TextSelectIdioma ActualitzaSelectText() idioma = "+idioma);
            switch (idioma)
            {
                case "valencià":
                    selectText = textValencia;
                    break;
                case "español":
                    selectText = textCastellano;
                    break;
                case "english":
                    selectText = textEnglish;
                    break;
                default:
                    selectText = "ERROR EN TextSelectIdioma";
                    break;
            }
        }

        if (txtCanvas != null) txtCanvas.text = selectText;
    }

    
}

public class DataTextSelectIdioma
{
    public string textValencia;
    public string textCastellano;
    public string textEnglish;
}
