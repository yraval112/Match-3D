using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject screen;
    void Awake()
    {
        GameManager.onGameOver += ShowGameOverScreen;
        screen = gameObject.transform.GetChild(0).gameObject;
    }
    public void ShowGameOverScreen()
    {
        Debug.Log("Showing Game Over Screen");
        if (screen != null)
            screen.SetActive(true);
    }

    public void HideGameOverScreen()
    {
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        GameManager.onGameOver -= ShowGameOverScreen;
    }
    public void OnRestartBtnClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("HomeScene");

    }
}
