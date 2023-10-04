using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CT_ParaJugadorIniciaDialeg : CineTriggerScript
{
    // Start is called before the first frame update
    public Dialeg dialeg;
    public List<FaceCameraPersonajes> personatgesDialeg;
    public bool dosPosCamaraNecessari = false;
    public bool posCamaraFixaNecessari = false;
    public GameObject dosPosCamara;
    public Transform posCamaraFixa;

    public int orientacioProta = 0;
    public int orientacioAltres = 1;
    ActivadorDialeg tdp;

    //Per als calculs de posCamara
    public Transform posAltrePersonatge;

    private void Start()
    {
        if (GetComponent<ActivadorDialeg>())
            tdp = GetComponent<ActivadorDialeg>();
        else if (tdp == null)
            Debug.Log("CT_ParaJugadorIniciaDialeg Start() ACTIVADOR DIALEG NO TROBAT!!");
    }

    public override void ActivarAlEntrar()
    {


        if ((dosPosCamaraNecessari && dosPosCamara != null)||(posCamaraFixaNecessari && posCamaraFixa!=null))
            PosicionarCamara();

        //Canviar orientació de les animacions del diàleg
        foreach (FaceCameraPersonajes personatge in personatgesDialeg)
            personatge.CanviaDialegOrientacio(orientacioAltres);

        FaceCameraPersonajes facecpJugador = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FaceCameraPersonajes>();
        facecpJugador.CanviaDialegOrientacio(orientacioProta);
        personatgesDialeg.Add(facecpJugador);

        FindObjectOfType<DetectiuStateManager>().CanviarState_Xarrar();
        tdp.dialeg = dialeg;
        tdp.ObrirDialeg(personatgesDialeg.ToArray());
    }

    void PosicionarCamara()
    {
        Debug.Log("CT_ParaJugadorIniciaDialeg PosicionarCamara()");
        Vector3 newPosCamara;
        Transform posJugador = FindObjectOfType<DetectiuStateManager>().gameObject.transform;
        Vector3 posIntermedia = new Vector3((posAltrePersonatge.position.x + posJugador.position.x) / 2,
                (posAltrePersonatge.position.y + posJugador.position.y) / 2,
                (posAltrePersonatge.position.z + posJugador.position.z) / 2);
        Vector3 direccioJugador = posIntermedia - posJugador.position;


        if (dosPosCamaraNecessari && dosPosCamara != null)
        {
            Debug.Log("CT_ParaJugadorIniciaDialeg PosicionarCamara() utilitzar dosPosCamara");
            //Instanciar Dialeg_PosCamara enmitg dels personatges

            GameObject instaPosCamara = Instantiate(dosPosCamara, posIntermedia, Quaternion.LookRotation(direccioJugador));

            //Calcular quins dels dos fills del Dialeg_PosCamara és l'adecuat pel moment
            Transform posA = instaPosCamara.transform.GetChild(0).transform;
            float distanciaA = Vector3.Distance(Camera.main.transform.position, posA.position);
            Transform posB = instaPosCamara.transform.GetChild(1).transform;
            float distanciaB = Vector3.Distance(Camera.main.transform.position, posB.position);

            if (distanciaA < distanciaB) newPosCamara = posA.position;
            else newPosCamara = posB.position;
        }
        else
        {
            Debug.Log("CT_ParaJugadorIniciaDialeg PosicionarCamara() utilitzar posCamaraFixa");
            newPosCamara = posCamaraFixa.position;
        }

        //Cridar al métode de càmara per a canviar posició i enfoque
        FindObjectOfType<Controla_Camara>().NouEnfoque(posIntermedia, newPosCamara);
        //Debug.Log("La camara s'ha canviat");

        //FINAL DE LA CINEMÁTICA
        FinalCinematica();
    }
}
