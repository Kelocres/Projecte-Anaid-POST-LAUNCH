using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Com no és un MonoBehaviour, per a poder-ho editar des de
// l'Inspector s'ha de marcar com Serializable
[System.Serializable]
public class DialogProva
{
    public string nom;

    //Per a que es puguen editar millor en l'Inspector
    [TextArea(3,10)]
    public string[] oracions;
}
