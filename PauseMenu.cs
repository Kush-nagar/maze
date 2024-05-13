using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] public GameObject PauseMenuPanel;

    public void Pause(){
        PauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume(){
        PauseMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Restart(){
        Time.timeScale = 1f;
        SceneManager.LoadScene("GameplaySceen");
    }

    public void Exit(){        
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
