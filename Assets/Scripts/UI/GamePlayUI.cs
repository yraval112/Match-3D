using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GamePlayUI : MonoBehaviour
{
    [Header("Combo UI")]
    public Slider comboBar;
    public TextMeshProUGUI comboText;
    public Image comboBarFill;

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
        if (comboBar != null)
        {
            comboBar.maxValue = 1f;
        }
        UpdateUI();
    }

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        if (CoinSystem.Instance == null) return;

        UpdateComboUI();
        UpdateScoreUI();
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
                comboText.text = "COMBO x" + currentCombo + "\n(" + timeRemaining.ToString("F1") + "s)";
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

    public void OnComboActivated()
    {
        UpdateComboUI();
    }
}
