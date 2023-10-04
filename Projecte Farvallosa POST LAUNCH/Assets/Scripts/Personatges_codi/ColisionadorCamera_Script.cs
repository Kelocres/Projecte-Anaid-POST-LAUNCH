using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisionadorCamera_Script : MonoBehaviour
{
    // Start is called before the first frame update
    public string stringParameter = "idenColisionadorCamara";
    public Transform transformPersonatge;
    public Camera camaraActual;
    public float floatParameter;
    private bool bloquear;
    void Start()
    {
        floatParameter = 0;
        bloquear = false;
        if (camaraActual == null)
            camaraActual = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        IdentificarQuad();
    }

    void IdentificarQuad()
    {
        if (!bloquear)
        {
            RaycastHit idenColisionador;
            if (Physics.Linecast(transformPersonatge.position, camaraActual.transform.position, out idenColisionador))
            {
                //Debug.Log("ColisionadorCamera_Script IdentificarQuad() Colisió");
                //Comprobar gameobject colisionado con los cuadros hijos
                for (int i = 0; i < 4; i++)
                    if (idenColisionador.collider.gameObject == transform.GetChild(i).gameObject)
                        //Debug.Log((float)i/3f);
                        //animacion.SetFloat("idenColisionadorCamara", (float)i / 3f);
                        floatParameter = (float)i / 3f;

                //Debug.Log("IdentificarQuad() floatParameter = " + floatParameter);
            }
        }

    }

    public void ForzarFloatParameter(bool validar_bloqueo, float valor)
    {
        bloquear = validar_bloqueo;
        if (bloquear) floatParameter = valor;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if(camaraActual!=null)
            Gizmos.DrawLine(camaraActual.transform.position, transform.position);
    }
}
