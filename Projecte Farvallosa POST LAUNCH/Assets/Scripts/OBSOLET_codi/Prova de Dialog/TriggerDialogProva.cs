using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogProva : MonoBehaviour
{
    public DialogProva prova;

    public void ObrirDialog()
    {
        if(prova!=null)
            FindObjectOfType<ManejaDialogProva>().IniciaDialog(prova);
    }
}
