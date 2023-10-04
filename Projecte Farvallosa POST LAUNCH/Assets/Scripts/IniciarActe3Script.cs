using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IniciarActe3Script : MonoBehaviour
{
    // Start is called before the first frame update
    string[] claus_iniciaActe3 = { "Acte2DialegLorenei1", "Acte2DialegXema1", "Acte2DialegAnaid1" };
    private bool dialegEnviat;

    public void Start()
    {
        dialegEnviat = false;
    }

    public void ComprovarClaus()
    {
        if (!dialegEnviat)
        {
            //Debug.Log("IniciarActe3Script ComprovarClaus()");
            bool totesLesClaus = true;
            int numeroClaus = 0;
            foreach (string clau in claus_iniciaActe3)
            {
                if (!ManejaClaus.instance.ComprobarClau(clau))
                    totesLesClaus = false;
                else
                {
                    numeroClaus++;
                }
            }

            if (totesLesClaus)
            {
                dialegEnviat = true;

                //Obrir dialeg que inicia la cinematica
                IniciarActe3();
            }
            else
            {
                Debug.Log("IniciarActe3Script ComprovarClaus() Claus obtingudes: " + numeroClaus);
            }
        }
    }

    public void IniciarActe3()
    {
        Debug.Log("IniciarActe3Script ComprovarClaus() Iniciar acte 3!!");
        FindObjectOfType<DetectiuStateManager>().CanviarState_Xarrar();
        FaceCameraPersonajes[] nomesProta = { GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<FaceCameraPersonajes>() };
        GetComponent<ActivadorDialeg>().ObrirDialeg(nomesProta);
    }
}
