using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManejaDialogProva : MonoBehaviour
{
    //Per a guardar les frases i mostrar-les una a una
    private Queue<string> oracions;

    //Per a mostrar el dialogs en la pantalla
    //Si et marca lo de UI en roig, ni cas
    public Text txtNom;
    public Text txtDialog;

    //Per a mostrar o llevar la caixa del dialog
    public Animator animator;

    // Update is called once per frame
    void Start()
    {
        oracions = new Queue<string>();
    }

    public void IniciaDialog(DialogProva dialog)
    {
        //Debug.Log ("Començar conversació amb "+ dialog.nom);

        animator.SetBool("isObert", true);
        //Mostrar nom del dialogant
        txtNom.text = dialog.nom;

        //Eliminar de la qua les oracions de dialogs anteriors
        oracions.Clear();

        //Posar oracions del DialogProva en la qua
        foreach (string oracio in dialog.oracions)
            oracions.Enqueue(oracio);

        //Debug.Log(oracions.Count);
        MostrarOracioActual();
    }

    public void MostrarOracioActual()
    {
        //Si ja no hi han més oracions, acabar dialog
        if(oracions.Count == 0) 
        {
            AcabarDialog();
            return;
        }

        //Extraure text
        string frase = oracions.Dequeue();
        //Debug.Log(frase);
        //txtDialog.text = frase;
        StopAllCoroutines();
        StartCoroutine(EscriuFrase(frase));
    }

    //Manera optativa: les lletres apareixen una a una
    IEnumerator EscriuFrase(string frase)
    {
        txtDialog.text = "";
        foreach(char lletra in frase.ToCharArray())
        {
            txtDialog.text += lletra;
            yield return null;
        }
    }

    public void AcabarDialog()
    {
        animator.SetBool("isObert", false);
        //Debug.Log("Fi de la conversació");
    }
}
