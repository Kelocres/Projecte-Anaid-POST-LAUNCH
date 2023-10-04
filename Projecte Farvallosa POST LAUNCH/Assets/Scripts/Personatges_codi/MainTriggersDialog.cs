using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTriggersDialog : MonoBehaviour
{
    //Si el jugador elegix xarrar mentre està en el trigger central, es
    //a dir, en aquest, s'ignora si està també en algun dels altres dos
    private bool enTriggerCentral;
    private bool enTriggerCostats;

    //Per a arreplegar el Delegates dels triggers del costat
    TriggersDialog dtEsquerra;
    TriggersDialog dtDreta;

    //Per a la prova de Dialogue System
    //https://www.youtube.com/watch?v=_nRzoTzeyxU&list=WL&index=16
    //TriggerDialogProva tdp;
    ActivadorDialeg tdp;

    //Per a posicionar la càmara durant el diàleg
    public GameObject posCamara;

    FaceCameraPersonajes facecp;

    //Per a senyalar que es pot xarrar
    public MostraSenyalsDialeg senyals;
    
    void Start()
    {
        //Per a arreplegar el Delegates dels triggers del costat
        dtEsquerra = transform.GetChild(0).GetComponent<TriggersDialog>();
        dtEsquerra.dT += TriggerAccio;
        dtDreta = transform.GetChild(1).GetComponent<TriggersDialog>();
        dtDreta.dT += TriggerAccio;

        //Per als dialegs
        tdp = GetComponent<ActivadorDialeg>();
        facecp = GetComponentInParent<FaceCameraPersonajes>();
        
    }

    void Update()
    {/*
        if(Input.GetKeyDown(KeyCode.K)    && (enTriggerCentral || enTriggerCostats))
        {
            PosicionarCamara();
            tdp.ObrirDialog();
        }*/
    }

    // Métode accessible per a ExplorarState per activar el diàleg; si no es possible, es retorna false
    public bool ActivarDialeg()
    {
        if(enTriggerCentral || enTriggerCostats)
        {
            Debug.Log("Un MainTriggersDialeg activa el dialeg");
            PosicionarCamara();
            
            //Obtindre el FaceCameraPersonajes del jugador
            FaceCameraPersonajes facecpJugador = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FaceCameraPersonajes>();
            //Pasar els dos fcps al TriggerDialeg (que ho passarà al ManejaDialeg)
            tdp.ObrirDialeg(facecpJugador, facecp);

            return true;
        }

        return false;
    }

    void PosicionarCamara()
    {
        //Instanciar Dialeg_PosCamara enmitg dels personatges
        Transform posJugador = FindObjectOfType<DetectiuStateManager>().gameObject.transform;
        Vector3 posIntermedia = new Vector3((transform.position.x + posJugador.position.x)/2, (transform.position.y + posJugador.position.y)/2, (transform.position.z + posJugador.position.z)/2);
        Vector3 direccioJugador = posIntermedia - posJugador.position;
        GameObject instaPosCamara = Instantiate(posCamara, posIntermedia, Quaternion.LookRotation(direccioJugador));

        //Calcular quins dels dos fills del Dialeg_PosCamara és l'adecuat pel moment
        Vector3 newPosCamara;

        Transform posA = instaPosCamara.transform.GetChild(0).transform;
        float distanciaA = Vector3.Distance(Camera.main.transform.position, posA.position);
        Transform posB = instaPosCamara.transform.GetChild(1).transform;
        float distanciaB = Vector3.Distance(Camera.main.transform.position, posB.position);

        if(distanciaA < distanciaB) newPosCamara = posA.position;
        else                        newPosCamara = posB.position;

        //Cridar al métode de càmara per a canviar posició i enfoque
        FindObjectOfType<Controla_Camara>().NouEnfoque(posIntermedia, newPosCamara);        
        //Debug.Log("La camara s'ha canviat");
    }

    


    private void TriggerAccio(int num)
    {
        //El jugador ha ixit del trigger
        if(num == -1)
        {
            //Debug.Log("Jugador ha ixit dels triggers");
            enTriggerCostats = false;
        }
        //El jugador ha entrat en el trigger de l'esquerra
        else if(num == 0)
        {
            //dialogOrientació del jugador: 0 (dreta)
            //dialogOrientació del NPC: 1 (esquerra)
            //Debug.Log("Jugador ha entrat en trigger de la esquerra");
            facecp.CanviaDialegOrientacio(1);
            enTriggerCostats = true;

        }
        // Ha entrat en el trigger de la dreta
        else if(num == 1)
        {
            //dialogOrientació del jugador: 1 (esquerra)
            //dialogOrientació del NPC: 0 (dreta)
            //Debug.Log("Jugador ha entrat en trigger de la dreta");
            facecp.CanviaDialegOrientacio(0);
            enTriggerCostats = true;
        }

        //Activar senyals
        if (senyals != null) senyals.MostrarSenyal(num);
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")    enTriggerCentral = true;
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")    enTriggerCentral = false;
    }
}
