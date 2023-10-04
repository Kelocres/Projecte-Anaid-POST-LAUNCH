using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerReposicionar : MonoBehaviour
{
    //public Dialeg dialeg;
    //public IniciaTimeline cinematica;
    // Start is called before the first frame update

    private GameObject jugador;

    public Transform posicion;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            jugador = other.gameObject;
            IniciarDialeg();
        }
    }

    public void IniciarDialeg()
    {
        Debug.Log("IniciarActe3Script ComprovarClaus() Iniciar acte 3!!");
        FindObjectOfType<DetectiuStateManager>().CanviarState_Xarrar();
        FaceCameraPersonajes[] nomesProta = { GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FaceCameraPersonajes>() };
        GetComponent<ActivadorDialeg>().ObrirDialeg(nomesProta);
    }

    public void Reposicionar()
    {
        jugador.transform.position = posicion.position;
    }

}
