using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.IO;
using UnityEngine.Events;

[System.Serializable]
public class PlayerCoinData
{
    public int totalCoins;
}

public class CoinChangeEvent : UnityEvent<int> { }
public class ComboChangeEvent : UnityEvent<int> { }

public class CoinSystem : MonoBehaviour
{
    public static CoinSystem Instance { get; private set; }

    [Header("Coin System")]
    public int totalCoins = 0;
    public int coinsPerMatch = 1;

    [Header("Combo System")]
    public int currentCombo = 0;
    public float comboResetTime = 5f;
    private float lastMatchTime;

    [Header("Events")]
    public CoinChangeEvent onCoinsChanged = new CoinChangeEvent();
    public ComboChangeEvent onComboChanged = new ComboChangeEvent();

    private string coinDataPath;
    private const string COIN_FILE_NAME = "player_coins.json";
    public int GetCoins() => totalCoins;
    public int GetCombo() => currentCombo;
    public float GetComboTimeRemaining() => Mathf.Max(0, comboResetTime - (Time.time - lastMatchTime));

    public void SetCoins(int amount) => totalCoins = amount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            coinDataPath = Path.Combine(Application.persistentDataPath, COIN_FILE_NAME);
            Debug.Log("Coin data path: " + coinDataPath);
            LoadCoinsFromStorage();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnApplicationQuit()
    {
        SaveCoinsToStorage();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveCoinsToStorage();
        }
    }

    public void SaveCoinsToStorage()
    {
        PlayerCoinData data = new PlayerCoinData
        {
            totalCoins = totalCoins
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(coinDataPath, json);
    }

    public void LoadCoinsFromStorage()
    {
        if (File.Exists(coinDataPath))
        {
            string json = File.ReadAllText(coinDataPath);
            PlayerCoinData data = JsonUtility.FromJson<PlayerCoinData>(json);
            totalCoins = data.totalCoins;
        }

    }

    public void DeleteCoinData()
    {
        if (File.Exists(coinDataPath))
        {
            File.Delete(coinDataPath);
            totalCoins = 0;
        }
    }



    public void AddCoins(int amount)
    {
        UpdateCombo();


        int baseCoins = 1;
        int comboBonus = currentCombo > 1 ? currentCombo : 0;
        int totalCoinsAdded = baseCoins + comboBonus;
        totalCoins += totalCoinsAdded;

        onCoinsChanged.Invoke(totalCoinsAdded);
        onComboChanged.Invoke(currentCombo);

    }

    private void UpdateCombo()
    {
        if (Time.time - lastMatchTime <= comboResetTime && currentCombo > 0)
        {
            currentCombo++;
        }
        else
        {
            currentCombo = 1;
        }

        lastMatchTime = Time.time;
    }

    public void ResetComboIfExpired()
    {
        if (currentCombo > 0 && Time.time - lastMatchTime > comboResetTime)
        {
            currentCombo = 0;
        }
    }

    public void ResetCombo()
    {
        currentCombo = 0;
        lastMatchTime = 0;
    }
}
