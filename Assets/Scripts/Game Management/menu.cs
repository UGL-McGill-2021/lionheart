using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public bool cursor;
    void Start()
    {
        if (!cursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
            Cursor.visible = true;
    }

    public void Play()
    {
        SceneManager.LoadScene(2);
    }
    public void Shut()
    {
        Application.Quit();
    }
    public void URL(string url)
    {
        Application.OpenURL(url);
    }
    public void Cred()
    {
        SceneManager.LoadScene(1);
    }
}
