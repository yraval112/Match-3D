using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public Action onGameOver;
    void Awake()
    {
        Instance = this;
        onGameOver += GameOver;
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
        Debug.Log("GAME OVER!");
    }
}
