using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MostraSenyalsDialeg : MonoBehaviour
{
    public SpriteRenderer senyalDreta;
    public SpriteRenderer senyalEsquerra;

    private SpriteRenderer senyalActual;
    private bool habilitat;
    private int numSenyal;
    // Start is called before the first frame update
    void Start()
    {
        if (senyalEsquerra != null)
        {
            senyalEsquerra.color = new Color(1, 1, 1, 0);
            senyalActual = senyalEsquerra;
        }
        if (senyalDreta != null)
            senyalDreta.color = new Color(1, 1, 1, 0);

        DetectiuStateManager dsm = FindObjectOfType<DetectiuStateManager>();
        if(dsm!=null)
        {
            dsm.delExplorar += HabilitarSenyal;
            dsm.delXarrar += InhabilitarSenyal;
            dsm.delCinematica += InhabilitarSenyal;
        }

        habilitat = true;
        numSenyal = -1;
    }

    // Update is called once per frame
    public void MostrarSenyal(int num)
    {
        numSenyal = num;
        senyalActual.color = new Color(1, 1, 1, 0);
        //El jugador està en el costat esquerre i el personatge en el dret
        if(numSenyal == 0)
        {
            if (senyalDreta != null)
                senyalActual = senyalDreta;
        }

        //El jugador està en el costat dret i el personatge en l'esquerre
        else if (numSenyal == 1)
        {
            if (senyalEsquerra != null)
                senyalActual = senyalEsquerra;
        }

        if(habilitat && numSenyal!=-1)
            senyalActual.color = new Color(1, 1, 1, 1);
    }

    public void HabilitarSenyal()
    {
        habilitat = true;
        MostrarSenyal(numSenyal);
    }

    public void InhabilitarSenyal()
    {
        habilitat = false;
        senyalActual.color = new Color(1, 1, 1, 0);
    }
}
