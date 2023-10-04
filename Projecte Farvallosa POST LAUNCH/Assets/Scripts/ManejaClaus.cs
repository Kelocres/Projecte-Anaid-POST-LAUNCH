using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManejaClaus : MonoBehaviour
{
    // PER A PROVAR LES VARIABLES STATIC SINGLETON
    public static ManejaClaus instance;
    private List<string> claus;
    


    private void Awake()
    {
        
        if (instance != null && instance != this)
            Destroy(this);
        else
        {
            instance = this;
            //DontDestroyOnLoad(this);

            claus = new List<string>();
        }
        
    }

    public void NovaClau(string nova)
    {
        if (!claus.Contains(nova)) claus.Add(nova);
        else Debug.Log("CLAU JA OBTINGUDA ANTERIORMENT");
    }

    public bool ComprobarClau(string intro)
    {
        if (claus.Contains(intro)) return true;
        return false;
    }

}
