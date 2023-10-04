using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejaDialeg : MonoBehaviour
{
    //Crear singleton
    public static ManejaDialeg instance;

    //Per a guardar les frases i mostrar-les una a una
    //private Queue<string> oracions;
    private Queue<LineaDialeg> liniesPersonatge;

    //Per a mostrar el diàlegs en la pantalla
    //Si et marca lo de UI en roig, ni cas
    public Text txtNom;     // Tag: CaixaDialeg_txtNom
    public Text txtDialog;  // Tag: CaixaDialeg_txtDialog

    //Per a treballar amb els textos en valencià, castellà i anglés:
    string frase;
    string fraseValencia;
    string fraseCastellano;
    string fraseEnglish;

    //La linea actual en la caixa de dialeg (es reescriurà si el canvia l'idioma)
    LineaDialeg liniaActual;

    //Els botons de les decisions (als quals s'anclaràn els BtnDesicio)
    //public Button [] canvas_btnDesicions;
    public BtnSelectIdioma[] canvas_btnDesicions;

    //Per a mostrar o llevar la caixa del diàleg
    public Animator animCaixaDialeg;

    //Els facecp dels personatges que dialoguen
    //En el futur es canviarà per un vector de FaceCPs:
    //FaceCameraPersonajes facecpPerso1;
    //FaceCameraPersonajes facecpPerso2;
    private FaceCameraPersonajes [] personatges;
    private string [] ids;
    
    Personatge_Anim [] dialegAnims;

    //Per a registrar el tipusPersonatge del Dialeg
    string emissor;

    //Per a obtindre el idioma en que es mostraràn els textos
    ManejaPartida mp;
    string idioma;

    //El dialeg que s'està llegint, es comprovarà si té lineaDecisio o ActivadorEvent
    Dialeg dialegActual;

    //Corutina per a escriu-re el text
    private IEnumerator dialeg_escriure;
    //Bool que indica si la corutina està en marxa o no
    private bool escrivint;

    public BtnDesicio esteticaOpcions;



    private void Awake()
    {

        
        if (instance != null & instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
    }
    
    
    void Start()
    {
        liniesPersonatge = new Queue<LineaDialeg>();

        mp = GetComponent<ManejaPartida>();
        //idioma = mp.idiomaActual;
        //idioma = Data_Idioma.instance.idiomaActual;
        Data_Idioma di = Data_Idioma.instance;
        if(di==null)
        {
            idioma = "valencià";
        }
        else
        {
            idioma = Data_Idioma.instance.IdiomaActual();
            di.delCanviarIdioma += ActualitzarIdioma;
        }
        

        //Trobar i reconeixer la caixa de dialeg
        GameObject getObjecte = GameObject.FindGameObjectWithTag("CaixaDialeg");
        if (getObjecte != null)
        {
            animCaixaDialeg = getObjecte.GetComponent<Animator>();
            //Debug.Log("Caixa del dialeg trobada");
        }
        else Debug.Log("ManejaDialeg Start() CaixaDialeg no trobat!!!");

        getObjecte = GameObject.FindGameObjectWithTag("CaixaDialeg_txtNom");
        if(getObjecte != null)
        {
            txtNom = getObjecte.GetComponent<Text>();
            //Debug.Log("Caixa del dialeg trobada");
        }
        getObjecte = GameObject.FindGameObjectWithTag("CaixaDialeg_txtDialog");
        if(getObjecte != null)
        {
            txtDialog = getObjecte.GetComponent<Text>();
            //Debug.Log("Caixa del dialeg trobada");
        }

        //Assignar funció del botó Següent 
        getObjecte = GameObject.FindGameObjectWithTag("CaixaDialeg_Next");
        if(getObjecte != null)
        {
            Button next = getObjecte.GetComponent<Button>();
            //SI ESTA LINEA S'UTILITZA, ASEGURAT DE QUE EL BOTÓ NO TINGA POSSAT EL MATEIX
            //ADD LISTENER DES DE L'EDITOR
            next.onClick.AddListener(delegate {LiniaContinua();});
            //next.onClick.AddListener(delegate {Debug.Log("Botó NEXT pulsat"); });
            //Debug.Log("Caixa del dialeg trobada");
        }

        //Trobar i reconeixer els botons de les decisions
        GameObject [] getObjectes = GameObject.FindGameObjectsWithTag("BotonsDecisio");
        if(getObjectes.Length == 3)
        {
            canvas_btnDesicions = new BtnSelectIdioma [3];
            for(int i=0; i<3; i++)
                canvas_btnDesicions[i] = getObjectes[i].GetComponent<BtnSelectIdioma>();
        }

        escrivint = false;
            
    }

    public void ActualitzarIdioma()
    {
        idioma = Data_Idioma.instance.IdiomaActual();
        if (liniaActual != null) MostrarOracio();
    }

    //En el cas de que se li passe un CadenatDialeg
    public void IniciaDialog(CadenatDialeg cadenat)
    {
        Dialeg nouDialeg = cadenat.DonarDialeg();
        if (nouDialeg != null)
            IniciaDialog(nouDialeg);
        else
        {
            Debug.Log("ManejaDialeg IniciaDialog(CadenatDialeg) El dialeg del cadenat és null");
            AcabarDialeg_Explorar();
        }
    }
    public void IniciaDialog(CadenatDialeg cadenat, FaceCameraPersonajes[] introPersonatges)
    {
        Dialeg nouDialeg = cadenat.DonarDialeg();
        if (nouDialeg != null)
            IniciaDialog(nouDialeg, introPersonatges);
        else
        {
            Debug.Log("ManejaDialeg IniciaDialog(CadenatDialeg, FaceCameraPersonajes[]) El dialeg del cadenat és null");
            AcabarDialeg_Explorar();
        }
    }

    //public void IniciaDialog(Dialeg dialog, FaceCameraPersonajes perso1, FaceCameraPersonajes perso2)
    public void IniciaDialog(Dialeg dialog, FaceCameraPersonajes [] introPersonatges)
    {
        Debug.Log("ManejaDialeg IniciaDialog(Dialeg, FaceCameraPersonajes[])");
        //Els personatges es preparen per a dialogar
        /*facecpPerso1 = perso1;
        facecpPerso1.IniciDialeg();
        facecpPerso2 = perso2;
        facecpPerso2.IniciDialeg();*/

        //Per a llevar les funcions onClick dels botons de decisió:
        BuidarBotonsDecisio();

        dialegActual = dialog;

        Debug.Log("RECIBITS " + introPersonatges.Length+" FCPs");

        personatges = new FaceCameraPersonajes[introPersonatges.Length];
        ids = new string[introPersonatges.Length];

        for(int i=0; i<introPersonatges.Length; i++)
        {
            personatges[i] = introPersonatges[i];
            ids[i] = introPersonatges[i].idPersonatge;
            personatges[i].IniciDialeg();
        }

        animCaixaDialeg.SetBool("isObert", true);
        animCaixaDialeg.SetBool("isDecisio", false);
        
        //Eliminar de la qua les oracions de dialogs anteriors
        liniesPersonatge.Clear();

        //Posar oracions del DialogProva en la cua
        foreach (LineaDialeg oracio in dialog.liniesDialeg)
            liniesPersonatge.Enqueue(oracio);

        //Debug.Log(oracions.Count);
        Debug.Log("ManejaDialeg IniciaDialog(Dialeg, FaceCameraPersonajes[]) crida a MostrarOracioActual()");
        LiniaContinua();
    }

    public void IniciaDialog(Dialeg dialog)
    {
        //Per a continuar un Dialeg després d'una desició; si el vector personatges està biut, es cancela
        if(personatges.Length == 0) return;

        //Per a llevar les funcions onClick dels botons de decisió:
        BuidarBotonsDecisio();

        dialegActual = dialog;
        animCaixaDialeg.SetBool("isObert", true);
        animCaixaDialeg.SetBool("isDecisio", false);

        //Eliminar de la qua les oracions de dialogs anteriors
        liniesPersonatge.Clear();

         //Posar oracions del DialogProva en la cua
        foreach (LineaDialeg oracio in dialog.liniesDialeg)
            liniesPersonatge.Enqueue(oracio);

        //Debug.Log(oracions.Count);
        Debug.Log("ManejaDialeg IniciaDialog(Dialeg) crida a MostrarOracioActual()");
        LiniaContinua();

    }

    //He decidit ignorar este métode
    /*
    public void PulsarNext()
    {
        if (escrivint)
        {
            Debug.Log("ManejaDialeg PulsarNext()");
            if(frase!=null)
            {
                StopCoroutine(EscriuFrase(frase));
                txtDialog.text = frase;
                Debug.Log("ManejaDialeg PulsarNext() tota la frase");
            }
            escrivint = false;
        }
        else LiniaContinua();
    }*/


    public void LiniaContinua()
    {
        //Debug.Log("ManejaDialeg MostrarOracioActual() liniesPersonatge.Count ="+ liniesPersonatge.Count);
        //Si ja no hi han més oracions, acabar dialog
        if(liniesPersonatge.Count == 0) 
        {
            //// Activar un event podrà ser seguit amb un nou Dialeg o cinemàtica
            if (dialegActual.despresDialegEsEvent())
            {
                //Debug.Log("Activar event");
                dialegActual.activador.Activar();
               
            }
            //else if(dialegActual.desicio != null && dialegActual.activador==null)
            
            // Si hi han claus disponibles, s'arrepleguen i es donen al ManejaClaus
            if(dialegActual.clausDespresDialeg.Length>0)
            {
                string[] claus = dialegActual.clausDespresDialeg;
                foreach (string clau in claus)
                    ManejaClaus.instance.NovaClau(clau);
            }

            if(dialegActual.despresDialegEsDecisio())
            {

                DecisioEnDialeg();
                return;
            }
            else if(dialegActual.despresDialegEsNextDialeg())
            {
                IniciaDialog(dialegActual.nextDialeg);

                return;
            }
            else if(dialegActual.despresDialegEsCinematica())
            {
                AcabarDialeg_Cinematica();
                //Reproduir cinemàtica
                if (dialegActual.cinematica != null)
                {
                    FindObjectOfType<DetectiuStateManager>().CanviarState_Cinematica(false);
                    dialegActual.cinematica.StartTimeline();
                    return;
                }
                else
                {
                    Debug.Log("ManejaDialeg MostrarOracioActual() ES DEVIA CONTINUAR AMB CINEMÁTICA, PERÒ ÉS NULL");
                }
                return;
            }
            else if(dialegActual.despresDialegEsCadenat())
            {
                if(dialegActual.nextCadenat == null)
                {
                    Debug.Log("ManejaDialeg MostraOracio() ES DEVIA CONTINUAR AMB CADENAT PERÒ ÉS NULL");
                    AcabarDialeg_Explorar();
                }
                else
                    IniciaDialog(dialegActual.nextCadenat);
                return;
            }
            else if(dialegActual.despresDialegEsRetornarDecisio())
            {
                RetornarDecisio();
                return;
            }
            else //if(dialegActual.despresDialegEsAcabar())
            {
                //Debug.Log("Ordenar acabar dialeg");
                AcabarDialeg_Explorar();
                return;
            }
        }

        //Extraure text
        //string frase = oracions.Dequeue();
        liniaActual = liniesPersonatge.Dequeue();
        MostrarOracio();
        
    }

    public void MostrarOracio()
    {
        if(liniaActual==null)
        {
            Debug.Log("ManejaDialeg MostrarOracio() liniaActual==null");
            return;
        }
        //string animEmissor = liniaActual.animEmissor;
        //string animReceptor = liniaActual.animReceptor;

        //Utilitzar la linia indicada segons l'idioma
        fraseValencia = liniaActual.textValencia;
        fraseCastellano = liniaActual.textCastellano;
        fraseEnglish = liniaActual.textEnglish;

        switch (idioma)
        {
            case "valencià":
                frase = fraseValencia;
                break;
            case "español":
                frase = fraseCastellano;
                break;
            case "english":
                frase = fraseEnglish;
                break;
            default:
                frase = "ERROR EN TextSelectIdioma";
                break;
        }

        //Mostrar nom del dialogant
        txtNom.text = liniaActual.nom;
        //emissor = liniaActual.tipusPersonatge;

        //Segons el tipusPersonatge indicat en Dialeg, s'identificaran emissor i receptor
        /*if(emissor=="npc")
        {
            facecpPerso2.CanviaDialegAnimacion(animEmissor);
            facecpPerso1.CanviaDialegAnimacion(animReceptor);
        }
        else if(emissor=="jugador")
        {
            facecpPerso1.CanviaDialegAnimacion(animEmissor);
            facecpPerso2.CanviaDialegAnimacion(animReceptor);
        }*/

        dialegAnims = liniaActual.anims;
        /*foreach(Personatge_Anim pa in dialegAnims)
        {
            foreach(FaceCameraPersonajes fcp in personatges)
            {
                if(pa.idPersonatge == fcp.idPersonatge)
                {
                    fcp.CanviaDialegAnimacion(pa.anim);
                    break;
                }
            }
        }*/
        foreach (FaceCameraPersonajes fcp in personatges)
            fcp.CanviaDialegAnimacion(dialegAnims);


        //Debug.Log("ManejaDialeg MostrarOracioActual() frase = "+frase);
        //txtDialog.text = frase;
        //StopAllCoroutines();
        //StartCoroutine(EscriuFrase(frase));
        if (dialeg_escriure != null) StopCoroutine(dialeg_escriure);
        dialeg_escriure = EscriuFrase(frase);
        StartCoroutine(dialeg_escriure);
    }

    public void BuidarBotonsDecisio()
    {
        //foreach(Button boto in canvas_btnDesicions)
        //    boto.onClick.RemoveAllListeners();
        foreach(BtnSelectIdioma boto in canvas_btnDesicions)
            boto.boto.onClick.RemoveAllListeners();
    }

    private void DecisioEnDialeg()
    {
        //Llegir el text de LiniaDecisió com si fora LiniaDialeg
        LiniaDecisio decisioActual = dialegActual.desicio;
        //Indicar al CANVAS que es canvie a mode decisió
        animCaixaDialeg.SetBool("isDecisio", true);

        //Enllaçar cada botó del CANVAS amb les opcions de desició. La quantitat de botons
        //que apareixen en la pantalla ha de ser acord amb la talla d'array dels botons de decisió
        BtnDesicio[] opcions = decisioActual.GetOpcions();

        if(opcions.Length== 0)
        {
            AcabarDialeg_Explorar();
            return;
        }
        for (int i = 0; i < canvas_btnDesicions.Length; i++)
        {
            if (i >= opcions.Length)
            {
                canvas_btnDesicions[i].gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Anclar botó " + i);
                //Debug.Log("Botó del canvas: "+canvas_btnDesicions[i]!=null);
                //Debug.Log("Opció de la decisió: "+decisioActual.opcions[i]!=null);
                canvas_btnDesicions[i].gameObject.SetActive(true);
                //decisioActual.opcions[i].DemostraExistencia(i);
                if (opcions[i] == null) Debug.Log("ManejaDialeg DecisioEnDialeg() opcions[" + i + "] == null");
                if (canvas_btnDesicions[i] == null) Debug.Log("ManejaDialeg DecisioEnDialeg() canvas_btnDesicions[" + i + "] == null");
                opcions[i].AnclarBoto(canvas_btnDesicions[i]);

                if (esteticaOpcions != null)
                    opcions[i].CopiarEstetica(esteticaOpcions);
            }
        }
        
        //Utilitzar la linia indicada segons l'idioma
        fraseValencia = decisioActual.textValencia;
        fraseCastellano = decisioActual.textCastellano;
        fraseEnglish = decisioActual.textEnglish;

        switch (idioma)
        {
            case "valencià":
                frase = fraseValencia;
                break;
            case "español":
                frase = fraseCastellano;
                break;
            case "english":
                frase = fraseEnglish;
                break;
            default:
                frase = "ERROR EN TextSelectIdioma";
                break;
        }

        //Mostrar nom del dialogant
        txtNom.text = decisioActual.nom;

        dialegAnims = decisioActual.anims;
        foreach (FaceCameraPersonajes fcp in personatges)
            fcp.CanviaDialegAnimacion(dialegAnims);

        Debug.Log(frase);

        if (dialeg_escriure != null) StopCoroutine(dialeg_escriure);
        dialeg_escriure = EscriuFrase(frase);
        StartCoroutine(dialeg_escriure);

        
    }

    private void RetornarDecisio()
    {
        if (dialegActual.liniaRetornarDecisio == null)
        {
            DecisioEnDialeg();
            return;
        }
        //Indicar al CANVAS que es canvie a mode decisió
        animCaixaDialeg.SetBool("isDecisio", true);

        //Llegir el text de LiniaDecisió com si fora LiniaDialeg
        LiniaDecisio decisioActual = dialegActual.desicio;

        //Enllaçar cada botó del CANVAS amb les opcions de desició. La quantitat de botons
        //que apareixen en la pantalla ha de ser acord amb la talla d'array dels botons de decisió
        BtnDesicio[] opcions = decisioActual.GetOpcions();
        if (opcions.Length == 0)
        {
            AcabarDialeg_Explorar();
            return;
        }

        for (int i = 0; i < canvas_btnDesicions.Length; i++)
        {
            if (i >= opcions.Length)
            {
                canvas_btnDesicions[i].gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Anclar botó " + i);
                //Debug.Log("Botó del canvas: "+canvas_btnDesicions[i]!=null);
                //Debug.Log("Opció de la decisió: "+decisioActual.opcions[i]!=null);
                canvas_btnDesicions[i].gameObject.SetActive(true);
                //decisioActual.opcions[i].DemostraExistencia(i);
                opcions[i].AnclarBoto(canvas_btnDesicions[i]);

                if (esteticaOpcions != null)
                    opcions[i].CopiarEstetica(esteticaOpcions);
            }
        }

        LineaDialeg linia = dialegActual.liniaRetornarDecisio;

        //Utilitzar la linia indicada segons l'idioma
        fraseValencia = linia.textValencia;
        fraseCastellano = linia.textCastellano;
        fraseEnglish = linia.textEnglish;

        switch (idioma)
        {
            case "valencià":
                frase = fraseValencia;
                break;
            case "español":
                frase = fraseCastellano;
                break;
            case "english":
                frase = fraseEnglish;
                break;
            default:
                frase = "ERROR EN TextSelectIdioma";
                break;
        }

        //Mostrar nom del dialogant
        txtNom.text = linia.nom;

        dialegAnims = linia.anims;
        foreach (FaceCameraPersonajes fcp in personatges)
            fcp.CanviaDialegAnimacion(dialegAnims);

        Debug.Log(frase);

        if (dialeg_escriure != null) StopCoroutine(dialeg_escriure);
        dialeg_escriure = EscriuFrase(frase);
        StartCoroutine(dialeg_escriure);

        
    }

    //Manera optativa: les lletres apareixen una a una
    IEnumerator EscriuFrase(string frase)
    {
        txtDialog.text = "";

        //escrivint = true;

        //Contador de temps total, per a previndre que tarde massa
        float tempsTotal = 0f;
        float tempsMaxim = 2f;

        

        for(int i=0; i<frase.Length; i++)
        {
            txtDialog.text += frase[i];

            tempsTotal += Time.deltaTime;
            if (tempsTotal >= tempsMaxim)
            {
                txtDialog.text = frase;
                yield break;
            }

            yield return null;
        }

        escrivint = false;
    }

    public void AcabarDialeg_Explorar()
    {
        if (animCaixaDialeg == null)
            animCaixaDialeg = GameObject.FindGameObjectWithTag("CaixaDialeg").GetComponent<Animator>();

        animCaixaDialeg.SetBool("isObert", false);
        //facecpPerso1.AcabarDialeg();
        //facecpPerso2.AcabarDialeg();
        foreach(FaceCameraPersonajes fcp in personatges)
            fcp.AcabarDialeg();
        //Destruir o deshabilitar Dialeg_PosCamara
        Destroy(GameObject.FindGameObjectWithTag("Dialeg_PosCamara"));
        //Canviar de ExplorarState a XarrarState
        // NOTA FUTURA
        // MODIFICAR PER A QUE ES CANVIE A EXPLORARSTATE O CINEMATICASTATE SEGONS EL DIALEG
        FindObjectOfType<DetectiuStateManager>().CanviarState_Exlorar();

        //Buidar el vector de FaceCameraPersonatges
        personatges = new FaceCameraPersonajes[0];
        liniaActual = null;

        //Debug.Log("Fi de la conversació");
    }

    public void AcabarDialeg_Cinematica()
    {
        animCaixaDialeg.SetBool("isObert", false);
        //facecpPerso1.AcabarDialeg();
        //facecpPerso2.AcabarDialeg();
        if(personatges!=null && personatges.Length!=0)
            foreach (FaceCameraPersonajes fcp in personatges)
                fcp.AcabarDialeg();
        //Destruir o deshabilitar Dialeg_PosCamara
        //Destroy(GameObject.FindGameObjectWithTag("Dialeg_PosCamara"));
        //Canviar de ExplorarState a XarrarState
        // NOTA FUTURA
        // MODIFICAR PER A QUE ES CANVIE A EXPLORARSTATE O CINEMATICASTATE SEGONS EL DIALEG
        //FindObjectOfType<DetectiuStateManager>().CanviarState_Exlorar();

        //Buidar el vector de FaceCameraPersonatges
        personatges = new FaceCameraPersonajes[0];
        liniaActual = null;

        //Debug.Log("Fi de la conversació");
    }
}
