using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public FileManager fileManager;
    public AudioSource musicAudio;
    // Start is called before the first frame update
    void Start()
    {
        float rawValue = fileManager.readMusic();
        musicAudio.volume = (rawValue / 100);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void loadGameScene()
    {
        SceneManager.LoadScene(2);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void loadOptionsScene()
    {
        SceneManager.LoadScene(1);
    }

    public void loadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
