using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguixJugador : MonoBehaviour
{
    Transform protagonista; //Personatge jugador
    public Transform posCamaraExplorar; //Gameobject buit on es situarà la càmara
    public Transform objectiuCamaraExplorar;

    public Controla_Camara controlaCamara;

    //Ha de guardar la posició de camera i objectiu en ExplorarState quan es
    //canvia de Zona (VoreExplorar_Zona)

    void Start()
    {
        //protagonista = FindObjectOfType<Controlador_Jugador>().transform;
        protagonista = FindObjectOfType<DetectiuStateManager>().transform;
        if(posCamaraExplorar==null)
            posCamaraExplorar = transform.GetChild(0).transform;

        if(controlaCamara==null)
        {
            Controla_Camara[] ccs = FindObjectsOfType<Controla_Camara>();
            if (ccs.Length == 0)
                Debug.Log("SeguixJugador Start() No hi ha Controla_Camara!!");
            else if (ccs.Length > 1)
                Debug.Log("SeguixJugador Start() Hi han " + ccs.Length + " Controla_Camara, el programa no funcionarà");
            else
                controlaCamara = ccs[0];
        }

        if (objectiuCamaraExplorar == null)
            objectiuCamaraExplorar = protagonista;
    }

    // Update is called once per frame
    void Update()
    {
        //Posicionarse sempre on està el jugador
        transform.position = protagonista.position;
    }

    public void ReconfigurarCameraExplorar(Transform introPosCamera, Transform introObjectiu)
    {
        if(introObjectiu==null || introPosCamera==null)
        {
            Debug.Log("SeguixJugador ReconfigurarCameraExplorar(introPosCamera, introObjectiu) introPosCamera i/o introObjectiu no definits!!!");
            return;
        }

        posCamaraExplorar = introPosCamera;
        objectiuCamaraExplorar = introObjectiu;
    }

    public void NouPosCameraExplorar(Transform introPosCamera)
    {
        Debug.Log("SeguixJugador NouPosCameraExplorar(introPosCamera)");
        if (introPosCamera == null)
        {
            Debug.Log("SeguixJugador NouPosCameraExplorar(introPosCamera) introPosCamera no definit!!!");
            return;
        }

        posCamaraExplorar = introPosCamera;
        // Reajust de Projecte Bancalet
        // Es crida si l'estat del detectiu és explorar

        if(protagonista.GetComponent<DetectiuStateManager>().ActivatExploraState())
            ActivarCameraExplorar();
    }

    public void NouObjectiuExplorar(Transform introObjectiu)
    {
        if (introObjectiu == null)
        {
            Debug.Log("SeguixJugador NouPosCameraExplorar(introPosCamera) introObjectiu no definit!!!");
            return;
        }

        objectiuCamaraExplorar = introObjectiu;
    }

    public void MirarAlJugador()
    {
        objectiuCamaraExplorar = protagonista;
    }

    public void ActivarCameraExplorar()
    {
        Debug.Log("SeguixJugador ActivarCameraExplorar()");
        controlaCamara.NouEnfoque(objectiuCamaraExplorar, posCamaraExplorar);
    }
}
