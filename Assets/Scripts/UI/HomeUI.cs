using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeUI : MonoBehaviour
{
    public void OnLevelBtnClick(int levelNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlayLevel" + levelNumber);
    }
}
