using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePlayUI : MonoBehaviour
{
    [Header("Combo UI")]
    public Slider comboBar;
    public TextMeshProUGUI comboText;
    public TextMeshProUGUI levelTimeText;
    public Image comboBarFill;

    [Header("Level Timer")]
    public bool showLevelTimer = true;

    private float levelTimer = 0f;
    private bool timerRunning = false;

    [Header("Score UI")]
    public TextMeshProUGUI scoreText;

    [Header("Combo Settings")]
    public float comboResetTime = 5f;
    public Color normalComboColor = Color.white;
    public Color activeComboColor = Color.green;
    public Color maxComboColor = Color.yellow;

    private int displayedCombo = 0;

    void Start()
    {
        if (CoinSystem.Instance != null)
        {
            CoinSystem.Instance.SetCoins(0);
        }

        if (comboBar != null)
        {
            comboBar.maxValue = 1f;
        }
        // Initialize level timer from LevelManager if available
        if (LevelManager.Instance != null)
        {
            levelTimer = LevelManager.Instance.levelTimeLimit;
            timerRunning = levelTimer > 0f;
        }

        UpdateUI();
    }

    void Update()
    {
        // Stop timer if the game has ended elsewhere
        if (GameManager.Instance != null && GameManager.Instance.isGameOver)
        {
            timerRunning = false;
        }

        if (timerRunning)
        {
            levelTimer -= Time.deltaTime;
            if (levelTimer <= 0f)
            {
                levelTimer = 0f;
                timerRunning = false;
                if (GameManager.Instance != null)
                {
                    //GameManager.Instance.GameOver();
                    GameManager.onGameOver?.Invoke();
                }
            }
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (CoinSystem.Instance == null) return;

        UpdateComboUI();
        UpdateScoreUI();
        UpdateLevelTimerUI();
    }

    void UpdateComboUI()
    {
        if (CoinSystem.Instance == null || comboBar == null) return;

        int currentCombo = CoinSystem.Instance.GetCombo();
        float timeRemaining = CoinSystem.Instance.GetComboTimeRemaining();

        if (currentCombo != displayedCombo)
        {
            displayedCombo = currentCombo;

            if (comboText != null)
            {
                comboText.transform.DOScale(1.2f, 0.1f).OnComplete(() =>
                {
                    comboText.transform.DOScale(1f, 0.1f);
                });
            }
        }

        if (currentCombo > 0)
        {
            float fillAmount = Mathf.Clamp01(timeRemaining / comboResetTime);
            comboBar.value = fillAmount;

            if (comboBarFill != null)
            {
                if (currentCombo >= 5)
                {
                    comboBarFill.color = maxComboColor;
                }
                else if (currentCombo >= 2)
                {
                    comboBarFill.color = activeComboColor;
                }
                else
                {
                    comboBarFill.color = normalComboColor;
                }
            }

            if (comboText != null)
            {
                comboText.text = "COMBO x" + currentCombo; //+ "\n(" + timeRemaining.ToString("F1") + "s)";
                comboText.gameObject.SetActive(true);
            }
        }
        else
        {
            comboBar.value = 0;
            if (comboText != null)
            {
                comboText.gameObject.SetActive(false);
            }
        }
    }

    void UpdateScoreUI()
    {
        if (CoinSystem.Instance == null) return;

        int totalScore = CoinSystem.Instance.GetCoins();

        if (scoreText != null)
        {
            scoreText.text = "SCORE: " + totalScore;
        }
    }

    void UpdateLevelTimerUI()
    {
        if (!showLevelTimer || levelTimeText == null) return;

        if (levelTimer <= 0f)
        {
            levelTimeText.text = "TIME: 0s";
            return;
        }

        int totalSeconds = Mathf.CeilToInt(levelTimer);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        levelTimeText.text = string.Format("TIME: {0:D2}:{1:D2}", minutes, seconds);
    }

    public void OnComboActivated()
    {
        UpdateComboUI();
    }
}
