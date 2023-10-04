using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CadenatDialeg : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Clau_Dialeg[] dialegsBloquetjats;
    
    public Dialeg dialegEixida;
    

    public Dialeg DonarDialeg()
    {
        for(int i=0; i<dialegsBloquetjats.Length; i++)
        {
            Clau_Dialeg actual = dialegsBloquetjats[i];
            bool totesLesClaus = true;
            //Comprobar cada clau
            for (int j=0; j<actual.claus_condicions.Length; j++)
            {
                string clauActual = actual.claus_condicions[j].clau;
                bool obrir = actual.claus_condicions[j].obrir;
                Debug.Log("Cadenat Dialeg DonarDialeg() Comprobar clau: " + clauActual);
                //if (actual.claus_condicions[j].obrir)
                if (obrir)
                {
                    //if (!ManejaClaus.instance.ComprobarClau(actual.claus_condicions[j].clau))
                    if (!ManejaClaus.instance.ComprobarClau(clauActual))
                    {
                        totesLesClaus = false;
                        break;
                    }
                }
                else
                {

                    //if (ManejaClaus.instance.ComprobarClau(actual.claus_condicions[j].clau))
                    if (ManejaClaus.instance.ComprobarClau(clauActual))
                    {
                        totesLesClaus = false;
                        break;
                    }
                }

            }
            if (totesLesClaus)
                return actual.dialeg;
        }

        //Com no es pot ningún dels bloquetjats, es trau el dialeg d'eixida
        return dialegEixida;
    }
}

[System.Serializable]
public class Clau_Dialeg
{
    public Clau_Condicio[] claus_condicions;
    public Dialeg dialeg;
}

[System.Serializable]
public class Clau_Condicio
{
    public string clau;
    public bool obrir;
}
