using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCameraPersonajes : MonoBehaviour
{
    //Siempre se revertirá para tener todos los sprites bien
    //public bool revertir;
    Vector3 direccionCamara;

    Animator animacion;

    //Per identificar al personatge en ManejaDialeg, més enllà de si és npc o jugador
    //Ha de ser únic per al personatge i no ha de canviar mai
    public string idPersonatge;

    //Per a poder relacionar el nom d'una animació de diàleg i el seu valor numéric en dialogNumAnimació
    //Els cadenes s'escriuen en el Inspector
    //MILLOR ESCRIU-RE TOT EN MINÚSCULA COM: "xarrar" o "escoltar_preocupat"
    public string [] dialegAnims;

    //Animació per defecte, té que estar inclosa en dialegAnims(per exemple: "escoltar")
    public string animDefecte;

    //La càmara a la qual s'encaren no serà sempre la Camera.main
    private Camera camaraActual;
    private ColisionadorCamera_Script colcam;

    void Start()
    {
        animacion = GetComponent<Animator>();
        colcam = transform.parent.GetComponentInChildren<ColisionadorCamera_Script>();
        if(colcam != null)
        {
            colcam.camaraActual = camaraActual;
            colcam.transformPersonatge = transform.parent;
        }
        //camaraActual = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        //Versió anterior
        //direccionCamara =  transform.position - Camera.main.transform.position;
        //transform.rotation = Quaternion.LookRotation(direccionCamara);

        //Versió similar a FaceCameraElementos
        transform.rotation = camaraActual.transform.rotation;
        
        //if (revertir==true) 
            //transform.rotation.y += 180; ASI NO VALE PORQUE NO PUEDES SUMAR A UN QUATERNIÓN
            //CON transform.rotate CONSIGUES UNA ROTACIÓN RELATIVA
        if(colcam!= null)
        {
            animacion.SetFloat(colcam.stringParameter, colcam.floatParameter);
        }

    }

    public void CanviarCamara(Camera novaCamara)
    {
        camaraActual = novaCamara;
        transform.rotation = camaraActual.transform.rotation;
        if (colcam != null)
            colcam.camaraActual = camaraActual;
    }

    //Per a canviar l'orientació de les animacions de diàleg, encara que no estiga en XarrarState
    public void CanviaDialegOrientacio(int orientacio)
    {
        if(orientacio==0 || orientacio == 1)
            animacion.SetFloat("dialegOrientacio",orientacio);
    }

    public void CanviaDialegOrientacio(Personatge_Anim anim)
    {
        if (anim.CO_EsNoCanviar()) return;

        if (anim.CO_EsCapDreta()) animacion.SetFloat("dialegOrientacio", 0);
        else if(anim.CO_EsCapEsquerra()) animacion.SetFloat("dialegOrientacio", 1);
        else if(anim.CO_EsInvertirActual())
        {
            float actual = animacion.GetFloat("dialegOrientacio");
            animacion.SetFloat("dialegOrientacio", (actual + 1) % 2);
        }
    }

    //Per a canviar l'animació de dialeg
    public void CanviaDialegAnimacion(string anim)
    {

        //Si el string aportat està buit, es manté l'anim actual
        if (anim == null || anim == "") return;

        for (int i = 0; i < dialegAnims.Length; i++)
        {
            if (dialegAnims[i] == anim)
            {
                animacion.SetFloat("dialegNumAnimacio", i);
                return;
            }
        }

        Debug.Log("No s'ha canviat l'animació per alguna rao");
    }
    public void CanviaDialegAnimacion(Personatge_Anim anim)
    {

        //Si el string aportat està buit, es manté l'anim actual
        if(anim==null || anim.anim=="")   return;

        for(int i=0; i<dialegAnims.Length; i++)
        {
            if(dialegAnims[i] == anim.anim)
            {
                animacion.SetFloat("dialegNumAnimacio",i);
                CanviaDialegOrientacio(anim);
                return;
            }
        }

        Debug.Log("No s'ha canviat l'animació per alguna rao");
    }

    public void CanviaDialegAnimacion(Personatge_Anim [] anims)
    {
        //Es canviarà l'animació al primer anim trobat amb el id del personatge
        //Si no es troba cap, es manté l'anim actual
        foreach(Personatge_Anim a in anims)
        {
            if(a!=null && a.idPersonatge==idPersonatge)
            {
                if (a.anim == null || a.anim == "") continue;
                for (int i = 0; i < dialegAnims.Length; i++)
                {
                    if (dialegAnims[i] == a.anim)
                    {
                        animacion.SetFloat("dialegNumAnimacio", i);
                        CanviaDialegOrientacio(a);
                        return;
                    }
                }
            }
        }
    }

    public void IniciDialeg()
    {
        animacion.SetBool("isXarrant",true);
        //Animació per defecte; si dona problemes en el futur, es canviarà
        //CanviaDialegAnimacion(new Personatge_Anim(idPersonatge, animDefecte));
        CanviaDialegAnimacion(animDefecte);
    }

    public void AcabarDialeg()
    {
        animacion.SetBool("isXarrant",false);
    }
}
