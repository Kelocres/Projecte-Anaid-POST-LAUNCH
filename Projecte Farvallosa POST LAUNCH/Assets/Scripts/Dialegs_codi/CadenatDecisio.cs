using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadenatDecisio : LiniaDecisio
{
    // Start is called before the first frame update
    public Clau_BtnDecisio[] opcionsBloquetjades;
    public BtnDesicio[] opcionsEixida;
    public Dialeg dialegEixida;

    public override void SetLiniaDecisio(DataLiniaDecisio data)
    {
        //Funció idéntica a la que hereda, sols que les opcions bloquejades deuran editar-se
        //sense utilitzar JSON
        nom = data.nom;
        idPersonatge = data.idPersonatge;
        //Els anims es deuen crear amb AddComponent
        //Els AddComponent es deuen fer des d'una classe MonoBehaviour
        /*anims = new Personatge_Anim[data.anims.Length];
        for (int j = 0; j < anims.Length; j++)
        {
            anims[j] = new Personatge_Anim(data.anims[j]);
        }*/
        textValencia = data.textValencia;
        textCastellano = data.textCastellano;
        textEnglish = data.textEnglish;

        if(opcionsEixida.Length > 0)
            for (int j = 0; j < opcionsEixida.Length; j++)
            {
                opcionsEixida[j].SetTexte(data.opcions[j]);
            }
        else if(dialegEixida != null)
        {
            Debug.Log("CadenatDecisio SetLiniaDecisio() No hi han opcions, es passa al dialegEixida");
            ManejaDialeg.instance.IniciaDialog(dialegEixida);
        }
        else
        {
            Debug.Log("CadenatDecisio SetLiniaDecisio() No hi ha dialegEixida i es pasa a ExplorarState");
            ManejaDialeg.instance.AcabarDialeg_Explorar();
        }
    }

    public override BtnDesicio[] GetOpcions()
    {
        List<BtnDesicio> opcions = new List<BtnDesicio>(opcionsEixida);

        //Afegir opcions que s'hagen desbloquetjat
        for(int i=0; i<opcionsBloquetjades.Length; i++)
        {
            Clau_BtnDecisio actual = opcionsBloquetjades[i];
            bool totesLesClaus = true;
            for (int j = 0; j < actual.claus_condicions.Length; j++)
            {
                string clauActual = actual.claus_condicions[j].clau;
                bool obrir = actual.claus_condicions[j].obrir;
                if (obrir)
                {
                    if (!ManejaClaus.instance.ComprobarClau(clauActual))
                    {
                        totesLesClaus = false;
                        break;
                    }
                }
                else
                {
                    if (ManejaClaus.instance.ComprobarClau(clauActual))
                    {
                        totesLesClaus = false;
                        break;
                    }
                }
            }
            if (totesLesClaus)
                opcions.Add(actual.opcio);
        }

        //Retornem la llista convertida en array
        return opcions.ToArray();
    }
}
[System.Serializable]
public class Clau_BtnDecisio
{
    public Clau_Condicio[] claus_condicions;
    public BtnDesicio opcio;
}
