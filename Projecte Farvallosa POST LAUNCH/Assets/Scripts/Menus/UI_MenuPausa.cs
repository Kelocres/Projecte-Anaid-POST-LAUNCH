using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MenuPausa : UI_Menu
{
    // Start is called before the first frame update
    public void ReturnStartMenu()
    {
        Debug.Log("UI_MenuPausa ReturnStartMenu()");
        BackToGame();

        //LevelManager.Instance.LoadScene("PROVA_MenuInici");
        SceneManager.LoadScene("EscenaMenuInici");
    }

    public void QuitGame()
    {
        Debug.Log("UI_MenuPausa QuitGame()");
        FindObjectOfType<SoundManager>().StopMusic();
        Application.Quit();
    }
}
