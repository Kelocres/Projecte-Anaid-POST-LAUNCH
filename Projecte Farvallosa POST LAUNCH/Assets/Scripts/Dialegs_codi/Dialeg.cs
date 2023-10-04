using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialeg : MonoBehaviour
{

    //Per a que es puguen editar millor en l'Inspector
    /*[TextArea(3,10)]
    public string[] oracions;*/

    public TextAsset dataDialegJSON;
    //PROVA DE COM SERIALITZAR EL DIALEG EN JSON
    //public string jsonMostra;

    [SerializeField]
    //public DataDialeg dataMostra;

    public LineaDialeg[] liniesDialeg;
    // Vector de respostes, cadascuna condueix a un Dialeg
    public bool decisioEsCadenat = false;
    public LiniaDecisio desicio;

    // Activar un event podrà ser seguit amb un nou Dialeg o cinemàtica
    public bool eventActivable = false;
    public ActivadorEvent activador;

    public string[] clausDespresDialeg;
    

    public Dialeg nextDialeg;

    public CadenatDialeg nextCadenat;

    public IniciaTimeline cinematica;

    //Com no sempre detecta com a null les variables desicio, activador i nextDialeg,
    //es preferible senyalar qué va despres de les línies de diàleg amb un enum
    //Creació de l'enum:
    public enum AccioDialeg : short {Acabar = 0, Decisio = 1, NextDialeg = 2, Cinematica = 4, Cadenat = 5, RetornarDecisio = 6};
    //Variable de l'enum creat (el valor per defecte és Acabar): 
    public AccioDialeg despresDialeg = AccioDialeg.Acabar;
    //Com altres scripts no poden treballar amb enums aliens, les comparacions es
    //realitzaràn en aquest script:
    public bool despresDialegEsAcabar()         {return despresDialeg == AccioDialeg.Acabar;}
    public bool despresDialegEsDecisio()        {return despresDialeg == AccioDialeg.Decisio;}
    public bool despresDialegEsNextDialeg()     {return despresDialeg == AccioDialeg.NextDialeg;}
    public bool despresDialegEsEvent()          { return eventActivable; }
    //public bool despresDialegEsEvent()          {return despresDialeg == AccioDialeg.Event;}
    public bool despresDialegEsCinematica()     {return despresDialeg == AccioDialeg.Cinematica;}
    public bool despresDialegEsCadenat()        { return despresDialeg == AccioDialeg.Cadenat; }
    public bool despresDialegEsRetornarDecisio() { return despresDialeg == AccioDialeg.RetornarDecisio; }

    public LineaDialeg liniaRetornarDecisio;


    //Per a convertir el JSON en liniesDialeg
    void Start()
    {
        //jsonMostra = JsonUtility.ToJson(liniesDialeg[0]);
        //ConvertirData();

        if(dataDialegJSON != null)
        {
            DataDialeg dataDialeg = JsonUtility.FromJson<DataDialeg>(dataDialegJSON.text);
            RecogerData(dataDialeg);

            //liniesDialeg = JsonUtility.FromJson<LineaDialeg[]>(liniesJSON.text);


            //AÇÒ NO FUNCIONA PERQUE LES INSTANCIES NO DEUEN SER DE CLASSES
            //QUE HEREDEN DE MONOBEHAVIOUR, SOLS DE DATA CLASSES
            /*
            Dialeg json = JsonUtility.FromJson<Dialeg>(liniesJSON.text);
            liniesDialeg = json.liniesDialeg;
            Debug.Log("Dialeg Start() JSON exportat");
            */
        }
    }

    //Métode per a saber com es deuen escriu-re els DataDialeg
    private void ConvertirData()
    {
        DataDialeg dataDialeg = new DataDialeg();
        //DataDialeg dataDialeg = ScriptableObject.CreateInstance("DataDialeg") as DataDialeg;
        dataDialeg.liniesDialeg = new DataLineaDialeg[liniesDialeg.Length];
        for (int i = 0; i < liniesDialeg.Length; i++)
        {
            dataDialeg.liniesDialeg[i] = liniesDialeg[i].ConvertirData();
            

        }

        //LA DESICIÓ SERÀ NULL EN AQUESTA PROVA
        dataDialeg.desicio = null;
        Debug.Log("Dialeg ConvertirData() Linies = " + dataDialeg.liniesDialeg.Length);
        Debug.Log("Dialeg ConvertirData() texte: " + dataDialeg.liniesDialeg[0].textValencia);

        //jsonMostra = JsonUtility.ToJson(liniesDialeg[0]);
        //dataMostra = dataDialeg;
        //jsonMostra = JsonUtility.ToJson(dataDialeg);
    }

    private void RecogerData(DataDialeg dataDialeg)
    {
        //Convertir DataLiniaDialeg en LiniaDialeg
        //Debug.Log("Dialeg RecogerData() dataDialeg.liniesDialeg.Length = " + dataDialeg.liniesDialeg.Length);
        liniesDialeg = new LineaDialeg[dataDialeg.liniesDialeg.Length];
        for(int i=0; i<liniesDialeg.Length; i++)
        {
            liniesDialeg[i] = new LineaDialeg(dataDialeg.liniesDialeg[i]);
            //Els anims es deuen crear amb AddComponent
            //Els AddComponent es deuen fer des d'una classe MonoBehaviour
            Personatge_Anim[] anims = new Personatge_Anim[dataDialeg.liniesDialeg[i].anims.Length];
            for(int j = 0; j < anims.Length; j++)
            {
                anims[j] = gameObject.AddComponent<Personatge_Anim>() as Personatge_Anim;
                anims[j].SetPersonatge_Anim(dataDialeg.liniesDialeg[i].anims[j]);
            }
            liniesDialeg[i].anims = anims;
            //Debug.Log("LineaDialeg() Número d'anims programats: " + anims.Length);
            /*for (int j = 0; j < anims.Length; j++)
            {
                //anims[j] = new Personatge_Anim(data.anims[j]);
                //anims[j] = GameObject.AddComponent<Personatge_Anim>() as Personatge_Anim;
                //this.gameObject.AddComponent<Personatge_Anim>();
            }*/
        }

        //gameObject.AddComponent<Personatge_Anim>();

        if (despresDialegEsDecisio() && desicio!=null && dataDialeg.desicio != null && !decisioEsCadenat)
        {
            Debug.Log("Definir LiniaDesicio");
            desicio.SetLiniaDecisio(dataDialeg.desicio);
            Personatge_Anim[] anims = new Personatge_Anim[dataDialeg.desicio.anims.Length];
            for (int j = 0; j < anims.Length; j++)
            {
                anims[j] = gameObject.AddComponent<Personatge_Anim>() as Personatge_Anim;
                anims[j].SetPersonatge_Anim(dataDialeg.desicio.anims[j]);
            }
            desicio.anims = anims;
        }

        if (despresDialegEsRetornarDecisio() && dataDialeg.liniaRetornarDecisio != null)
        {
            liniaRetornarDecisio = new LineaDialeg(dataDialeg.liniaRetornarDecisio);
            //Els anims es deuen crear amb AddComponent
            //Els AddComponent es deuen fer des d'una classe MonoBehaviour
            Personatge_Anim[] anims = new Personatge_Anim[dataDialeg.liniaRetornarDecisio.anims.Length];
            for (int j = 0; j < anims.Length; j++)
            {
                anims[j] = gameObject.AddComponent<Personatge_Anim>() as Personatge_Anim;
                anims[j].SetPersonatge_Anim(dataDialeg.liniaRetornarDecisio.anims[j]);
            }
            liniaRetornarDecisio.anims = anims;
        }
        
    }


}

//El dialeg serà interpretat pel JSON, però açó ha de ser Serializable
[System.Serializable]
public class LineaDialeg 
{
    //Nom del personatge que xarra
    public string nom;

    //Per a diferenciar el personatge segons el valor idPersonatge en FaceCameraPersonajes
    public string idPersonatge;

    //public string animEmissor;
    //public string animReceptor;
    //[SerializeField]
    public Personatge_Anim [] anims;


    //[TextArea(3,10)]
    //public string oracio;

    [TextArea(3,10)] public string textValencia;
    [TextArea(3,10)] public string textCastellano;
    [TextArea(3,10)] public string textEnglish;

    public LineaDialeg(DataLineaDialeg data)
    {
        nom = data.nom;
        idPersonatge = data.idPersonatge;
        //Els anims es deuen crear amb AddComponent
        //Els AddComponent es deuen fer des d'una classe MonoBehaviour

        //anims = new Personatge_Anim[data.anims.Length];
        //Debug.Log("LineaDialeg() Número d'anims programats: " + anims.Length);
        /*for (int j = 0; j < anims.Length; j++)
        {
            //anims[j] = new Personatge_Anim(data.anims[j]);
            //anims[j] = GameObject.AddComponent<Personatge_Anim>() as Personatge_Anim;
            //this.gameObject.AddComponent<Personatge_Anim>();
        }*/
        textValencia = data.textValencia;
        textCastellano = data.textCastellano;
        textEnglish = data.textEnglish;
    }

    public DataLineaDialeg ConvertirData()
    {
        DataLineaDialeg data = new DataLineaDialeg();
        data.nom = nom;
        data.idPersonatge = idPersonatge;
        data.anims = new DataPersonatge_Anim[anims.Length];
        Debug.Log("anims.Length ==" + anims.Length);
        Debug.Log("data.anims.Length == " + data.anims.Length);
        for (int j = 0; j < anims.Length; j++)
        {

            DataPersonatge_Anim newAnim = anims[j].ConvertirData();
            data.anims[j] = newAnim;
        }
        data.textValencia = textValencia;
        data.textCastellano = textCastellano;
        data.textEnglish = textEnglish;

        return data;
    }

    
    //Possibles addicions en el futur:
    // Variable que indique que, amb aquesta linia, el jugador té varies opcions a respondre
    // Variable que indique que, al acabar dialeg, X factor canvie en el nivell
}

//[System.Serializable]
public class LiniaDecisio : MonoBehaviour
{
    //Nom del personatge que xarra
    public string nom;

    //Per a diferenciar el personatge segons el valor idPersonatge en FaceCameraPersonajes
    public string idPersonatge;

    //public string animEmissor;
    //public string animReceptor;
    public Personatge_Anim [] anims;

    //[TextArea(3,10)]
    //public string oracio;

    [TextArea(3,10)] public string textValencia;
    [TextArea(3,10)] public string textCastellano;
    [TextArea(3,10)] public string textEnglish;

    [SerializeField]
    private BtnDesicio[] opcions;

    //public LiniaDecisio(DataLiniaDecisio data)
    public virtual void SetLiniaDecisio(DataLiniaDecisio data)
    {
        nom = data.nom;
        idPersonatge = data.idPersonatge;
        //Els anims es deuen crear amb AddComponent
        //Els AddComponent es deuen fer des d'una classe MonoBehaviour
        /*anims = new Personatge_Anim[data.anims.Length];
        for (int j = 0; j < anims.Length; j++)
        {
            anims[j] = new Personatge_Anim( data.anims[j]);
        }*/
        textValencia = data.textValencia;
        textCastellano = data.textCastellano;
        textEnglish = data.textEnglish;

        for (int j = 0; j < opcions.Length; j++)
        {
            opcions[j].SetTexte(data.opcions[j]);
        }
    }

    public virtual BtnDesicio[] GetOpcions()
    {
        return opcions;
    }

    
    //Possibles addicions en el futur:
    // Variable que indique que, amb aquesta linia, el jugador té varies opcions a respondre
    // Variable que indique que, al acabar dialeg, X factor canvie en el nivell
}

// EL SERIALIZABLE ES NECESSARI PER A LA CONVERSIÓ A JSON I VICEVERSA!!!
[System.Serializable]
public class DataDialeg
{
    public DataLineaDialeg[] liniesDialeg;
    // Vector de respostes, cadascuna condueix a un Dialeg

    public DataLiniaDecisio desicio;

    public DataLineaDialeg liniaRetornarDecisio;

    //public int despresDialeg;
}
[System.Serializable]
public class DataLineaDialeg
{
    //Nom del personatge que xarra
    public string nom;

    //Per a diferenciar el personatge segons el valor idPersonatge en FaceCameraPersonajes
    public string idPersonatge;

    //public string animEmissor;
    //public string animReceptor;
    //[SerializeField]
    public DataPersonatge_Anim[] anims;


    //[TextArea(3,10)]
    //public string oracio;

    [TextArea(3, 10)] public string textValencia;
    [TextArea(3, 10)] public string textCastellano;
    [TextArea(3, 10)] public string textEnglish;
}
[System.Serializable]
public class DataLiniaDecisio
{
    public string nom;

    //Per a diferenciar el personatge segons el valor idPersonatge en FaceCameraPersonajes
    public string idPersonatge;

    //public string animEmissor;
    //public string animReceptor;
    public DataPersonatge_Anim[] anims;

    public DataTextSelectIdioma[] opcions;

    [TextArea(3, 10)] public string textValencia;
    [TextArea(3, 10)] public string textCastellano;
    [TextArea(3, 10)] public string textEnglish;
}
/*
public class DataPersonatge_Anim
{
    public string idPersonatge;
    public string anim;
    public int canviaOrientacio;
}*/



