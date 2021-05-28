using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static Game Instance
    {
        get
        {
            return _instance;
        }
    }
    private static Game _instance;

    [SerializeField]
    private GameObject _pauseMenu;

    public bool IsPaused
    {
        get { return _isPaused; }
    }
    private bool _isPaused;

    private void Start()
    {
        _instance = this;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            _isPaused = !_isPaused;

            HandlePauseMenu();
        }
    }

    private void HandlePauseMenu()
    {
        _pauseMenu.SetActive(_isPaused);

        // Ternary Operator.
        Time.timeScale = _isPaused ? 0 : 1;

        if(_isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
