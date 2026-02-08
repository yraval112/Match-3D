using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GamePlayUI gamePlayUI;
    public GameOverUI gameOverUI;

    void Awake()
    {
        Instance = this;
    }
}
