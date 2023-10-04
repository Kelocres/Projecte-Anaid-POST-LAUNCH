using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PROVA_MostrarLinia : MonoBehaviour
{

    public Text text;
    public float interval = 0.01f;

    private string textFinal = "1234567890123456789012345678901234567890123456789012345678901234567890";


    void Start()
    {
        text = GetComponent<Text>();
    }

    public void MostrarLinea()
    {
        StartCoroutine(EscriuFraseTemps());
    }

    IEnumerator EscriuFrase()
    {
        text.text = "";
        //foreach (char lletra in frase.ToCharArray())
        for(int i = 0; i < 1000; i++)
        {
            text.text += "x";
            yield return null;
        }
    }

    IEnumerator EscriuFraseTemps()
    {
        text.text = "";
        float temps = 0f;
        int pos = 0;
        //foreach (char lletra in frase.ToCharArray())
        while(pos < textFinal.Length -1)
        {
            temps += Time.deltaTime;
            if(temps >= interval)
            {
                int numLletres = (int)(temps / interval);
                text.text += textFinal.Substring(pos, numLletres);
                pos += numLletres;
                temps %= interval;
            }

            yield return null;
        }
    }


}
