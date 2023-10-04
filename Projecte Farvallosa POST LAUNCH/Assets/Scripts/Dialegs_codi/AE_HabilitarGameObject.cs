using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AE_HabilitarGameObject : ActivadorEvent
{
    // Start is called before the first frame update
    public GameObject go;
    public override void Activar()
    {
        go.SetActive(true);

    }
}
