using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejaCamares : MonoBehaviour
{
    // Start is called before the first frame update

    public string camInicial;
    public NombraCamera camActual;

    private FaceCameraPersonajes [] fcPersonatges;
    private FaceCameraElementos []  fcElements;
    private DetectiuStateManager dsm;

    public NombraCamera [] nomsCameresEscenari;
    public NombraCamera nomCameraJugador;

    void Start()
    {
        //nomCameraJugador = new NombraCamera("CameraJugador", Camera.main);
        nomCameraJugador = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<NombraCamera>();
        nomsCameresEscenari = FindObjectsOfType<NombraCamera>();
        camActual = null;
        fcPersonatges = FindObjectsOfType<FaceCameraPersonajes>();
        fcElements = FindObjectsOfType<FaceCameraElementos>();
        dsm = FindObjectOfType<DetectiuStateManager>();
        
        if(camInicial == null || camInicial == "")
            CanviarCamara("CameraJugador");
        else
            CanviarCamara(camInicial);
    }

    void BuscarNombraCameres()
    {

    }

  
    //En cas d activar ho en un timeline, es fa amb un Signal Track
    //REFERÈNCIA PER A TREBALLAR AMB SIGNAL TRACK:
    //https://www.youtube.com/watch?v=PlfSWUiBCoo&t=184s
    public void CanviarCamara(string nomCam)
    {
        Debug.Log("ManejaCamares CanviarCamara("+nomCam+")");
        //En cas de que cam i camActual siga la mateixa, s han de desactivar i activar, en aquest ordre
        if (nomCam==null)
        {
            Debug.Log("Cam is null");
            return;
        }
        //if(camActual!=null)     camActual.gameObject.SetActive(false);
        //cam.gameObject.SetActive(true);
        /*if(camActual!=null)
        {
            // ------  MAAAAAAAL!!! ------
            // Al deshabilitar el component de la càmera, al tratar de reenviarho per a canviar de càmera, en realitat
            // es torna un valor null, es produix un error i no es fa el canvi
            camActual.camera.enabled = false;
            camActual.camera.GetComponent<AudioListener>().enabled = false;

            // ESTRATÈGIA ALTERNATIVA DESCARTADA: El valor Camera.depth
            // Es mostra el punt de vista de la càmera amb major depth FAAALS!!!
            //  Lo que ocurrix es que es renderitzen abans les càmeres amb major depth, mostrant finalment la última en el joc
            //  Açó pareix utilitzar-se per a efectes de post processament, però por ser ineficient per a aquest ús

            // ALTRA ESTRATÈGIA: Linkear càmeres amb noms (strings) i recibir el nom de la càmera que es vol mostrar
        }*/

        //NombraCamera novaCam;
        /*if(nomCam == "CameraJugador")
        {
           camActual = nomCameraJugador;
        }
        else
        {*/
            bool decidit = false;
            foreach(NombraCamera nc in nomsCameresEscenari)
            {
                if (nc.nom == nomCam)
                {
                    camActual = nc;
                    nc.camera.enabled = true;
                Debug.Log("ManejaCamares CanviarCamara() camActual (" + camActual.nom + ") activada i nc("+nc.nom+").camera.enabled = true");
                    decidit = true;
                    Debug.Log("ManejaCamares CanviarCamara() Camera " + nc.nom + " activada");
                    //break;
                }
                else
                {
                    
                    nc.camera.enabled = false;
                    Debug.Log("ManejaCamares CanviarCamara() Camera " + nc.nom + " desactivada");
                }
            }
            if(!decidit)
            {
                Debug.Log("ERROR: Nom no corresponent a cap camera");
                return;
            }
        //}

        if (camActual.camera == null) Debug.Log("NO HAY CAMARA EN CamActual");
        if (camActual.camera != null) Debug.Log("PUES SÍ QUE HAY CAMARA EN CamActual");
        // Ix un missatge d'error (NullReferenceException), però no pareix que faça res

        camActual.camera.enabled = true;
        AudioListener al = camActual.camera.GetComponent<AudioListener>();
        if(al!= null)   al.enabled = true;

        //Debug.Log("Camera actual assignada");
        if (dsm == null) Debug.Log("ManejaCamares CanviarCamara() dsm==null");
        //if (camActual == null) Debug.Log("ManejaCamares CanviarCamara() camActual == null");
        //if (camActual.camera == null) Debug.Log("ManejaCamares CanviarCamara() camActual.camera == null");
        else dsm.CanviarCamara(camActual.camera);

        foreach(FaceCameraPersonajes pers in fcPersonatges)
            pers.CanviarCamara(camActual.camera);

        foreach(FaceCameraElementos elem in fcElements)
            elem.CanviarCamara(camActual.camera);
    }
}


