using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiraCapJugador : MonoBehaviour
{
    Transform protagonista; //Personatge jugador
    Vector3 direccionCamara;
    // Start is called before the first frame update
    void Start()
    {
        protagonista = FindObjectOfType<DetectiuStateManager>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        direccionCamara = transform.position - protagonista.position;
        transform.rotation = Quaternion.LookRotation(direccionCamara);
    }
}
