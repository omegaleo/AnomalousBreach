using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Titlescreen : MonoBehaviour
{
    [SerializeField] private int _gameScene = 1;
    [SerializeField] private GameObject _exitBtn;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _exitBtn.SetActive(false);
        }
    }

    public void Play()
    {
        SceneManager.LoadScene(_gameScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
