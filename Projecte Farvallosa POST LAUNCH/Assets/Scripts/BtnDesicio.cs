using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnDesicio : BtnSelectIdioma
{
    public TextAsset textJSON;
    public Dialeg dialeg;

    public void AnclarBoto(Button boto)
    {
        textBoto = boto.GetComponentInChildren<Text>();
        //Debug.Log("PROBA BtnSelectIdioma: "+textBoto.text);
        ActualitzaTextBoto();

        //Assignar funció al onClick() del botó
        boto.onClick.AddListener(delegate {Decidit();});
    }

    public void AnclarBoto(BtnSelectIdioma boto)
    {
        boto.textValencia = textValencia;
        boto.textCastellano = textCastellano;
        boto.textEnglish = textEnglish;
        //Debug.Log("PROBA BtnSelectIdioma: "+textBoto.text);
        boto.ActualitzaTextBoto();

        //Assignar funció al onClick() del botó
        boto.boto.onClick.AddListener(delegate { Decidit(); });
    }

    public void Decidit()
    {
        //Indicar al ManejaDialeg que la desició ha sigut presa
        //Enviarli també el Dialeg registrat en textJSON (codi similar al de ActivadorDialeg)
        if(textJSON!=null)
            dialeg = JsonUtility.FromJson<Dialeg>(textJSON.text);
        FindObjectOfType<ManejaDialeg>().IniciaDialog(dialeg);

    }

    //CODI DE PROVA, ARA INSERVIBLE
    /*public void DemostraExistencia(int intro)
    {
        Debug.Log(" El botó "+intro+" EXISTEIX!!!");
    }*/
}
