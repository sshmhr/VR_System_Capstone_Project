using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//whenever we change the scenes in unity, we need to use this package

using UnityEngine.UI;
//we need since we will be dealing iwth Ui elements


public class MainMenu : MonoBehaviour
{
    public DataManager dataManager;
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        dataManager.saveGameState();
    }

    public void QuitGame()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


}