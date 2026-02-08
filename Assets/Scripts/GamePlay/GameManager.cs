using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Action onGameOver;
    public bool isGameOver = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        onGameOver += GameOver;
    }
    void OnDestroy()
    {
        onGameOver -= GameOver;
    }

    public void CheckWin()
    {
        if (FindObjectsOfType<SelectableItem>().Length == 0)
        {
            Win();
        }
    }

    public void Win()
    {
        Debug.Log("LEVEL COMPLETE!");
    }

    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("GAME OVER!");


    }
}
