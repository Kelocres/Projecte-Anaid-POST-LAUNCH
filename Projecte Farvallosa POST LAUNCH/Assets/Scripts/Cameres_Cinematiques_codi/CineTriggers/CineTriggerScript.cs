using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CineTriggerScript : MonoBehaviour
{
    //En el cas que un mateix GameObject tinga varios CineTriggerScripts i es necessiten
    //activar o desactivar en moments concrets
    public bool activat = true;

    //RECORDA QUE ELS OBJECTES QUE TENEN QUE ENTRAR EN EL TRIGGER HAN DE TINDRE
    //TAMBÉ UN RIGIDBODY
    public string tagColliderEnter;
    public string tagColliderExit;

    public bool ordreCaminarCapDesti = false;

    //Per a indicar al IniciaTimeline que es deuen desactivar tots els seus CTs
    public delegate void del_IT();
    public event del_IT finalCinematica;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
      if(activat && other.tag==tagColliderEnter)
        ActivarAlEntrar();
    }

    private void OnTriggerExit(Collider other)
    {
      if(activat && other.tag==tagColliderExit)
        ActivarAlEixir();
    }

    public virtual void ActivarAlEntrar()
    {

    }

    public virtual void ActivarAlEixir()
    {

    }

    protected void FinalCinematica() 
    { 
        //El delegate sols es pot cridar des de la classe que el té, no s'hereda
        finalCinematica(); 
    }
}
