using System;
using System.Collections;
using UnityEngine;

public enum Difficulty { Easy, Hard }

public class LevelManager : MonoBehaviour
{
    public static event Action<int> onLevelChanged;
    public static LevelManager Instance { get; private set; }

    public int levelNumber = 1;
    public LevelDataSO levelData;
    public ItemSlot[] itemSlots;
    public GameObject[] itemPrefabs;
    public ObjectPool objectPool;

    [Header("Difficulty Settings")]
    public Difficulty difficulty = Difficulty.Easy;

    [Header("Grid Layout Settings")]
    public int gridRows = 3;
    public int gridColumns = 4;
    public float slotSpacingX = 1.5f;
    public float slotSpacingY = 1.5f;
    public float gridZDistance = 10f;
    public Vector3 gridStartPosition = Vector3.zero;
    public float levelTimeLimit = 60f;

    private int currentItemsPerSlot;

    // Call this to change levels at runtime. Invokes `onLevelChanged`.
    public void SetLevel(int newLevel)
    {
        if (newLevel == levelNumber) return;
        levelNumber = newLevel;
        LoadLevelData();
        onLevelChanged?.Invoke(levelNumber);
    }

    public void NextLevel()
    {
        SetLevel(levelNumber + 1);
    }

    void Awake()
    {
        Instance = this;
        LoadLevelData();
        SetupGridLayout();
    }

    void Start()
    {
        if (currentItemsPerSlot == 0)
        {
            currentItemsPerSlot = (difficulty == Difficulty.Easy) ? 5 : 10;
        }
        SpawnInitialItems();
    }

    void LoadLevelData()
    {
        if (levelData == null)
        {
            Debug.LogError("LevelDataSO is not assigned!");
            return;
        }

        LevelData levelConfig = null;
        foreach (var level in levelData.levels)
        {
            if (level.levelNumber == levelNumber)
            {
                levelConfig = level;
                break;
            }
        }

        if (levelConfig == null)
        {
            Debug.LogWarning($"Level {levelNumber} not found! Using first level.");
            if (levelData.levels.Count > 0)
            {
                levelConfig = levelData.levels[0];
            }
            else
            {
                Debug.LogError("No levels defined in LevelDataSO!");
                return;
            }
        }

        gridRows = levelConfig.gridRows;
        gridColumns = levelConfig.gridColumns;
        slotSpacingX = levelConfig.slotSpacingX;
        slotSpacingY = levelConfig.slotSpacingY;
        gridZDistance = levelConfig.gridZDistance;
        gridStartPosition = levelConfig.gridStartPosition;
        // levelTimeLimit stored in LevelData is minutes — convert to seconds for runtime
        levelTimeLimit = levelConfig.levelTimeLimit * 60f;

        Debug.Log($"Level {levelNumber} Loaded: {gridRows}x{gridColumns} grid, Spacing: ({slotSpacingX}, {slotSpacingY})");
    }

    void SetupGridLayout()
    {
        int totalSlots = gridRows * gridColumns;
        itemSlots = new ItemSlot[totalSlots];

        float gridWidth = (gridColumns - 1) * slotSpacingX;
        float gridHeight = (gridRows - 1) * slotSpacingY;
        Vector3 centeredStartPosition = new Vector3(-gridWidth / 2, -gridHeight / 2, gridZDistance);

        int slotIndex = 0;
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridColumns; col++)
            {
                Vector3 slotPosition = centeredStartPosition + new Vector3(
                    col * slotSpacingX,
                    row * slotSpacingY,
                    0
                );

                GameObject slotObject = new GameObject("ItemSlot_" + row + "_" + col);
                slotObject.transform.position = slotPosition;
                slotObject.transform.SetParent(transform);

                ItemSlot slot = slotObject.AddComponent<ItemSlot>();
                itemSlots[slotIndex] = slot;

                GameObject spawnPointObject = new GameObject("SpawnPoint");
                spawnPointObject.transform.SetParent(slotObject.transform);
                spawnPointObject.transform.localPosition = Vector3.zero;
                slot.spawnPoint = spawnPointObject.transform;

                slotIndex++;
                Debug.Log("Created ItemSlot at position: " + slotPosition);
            }
        }

        Debug.Log("Grid Layout Setup Complete! Total Slots: " + totalSlots);
    }



    void SpawnInitialItems()
    {
        foreach (var slot in itemSlots)
        {
            slot.SetLevelManager(this);
            for (int i = 0; i < currentItemsPerSlot; i++)
            {
                SpawnItemInSlot(slot);
            }
        }
    }

    public void SpawnItemInSlot(ItemSlot slot)
    {
        ObjectType randomType = (ObjectType)UnityEngine.Random.Range(0, System.Enum.GetNames(typeof(ObjectType)).Length);
        SelectableItem item = objectPool.SpawnFromPool(randomType);

        if (item != null)
        {
            slot.AddItem(item);
        }
        else
        {
            Debug.LogError("Failed to spawn item of type: " + randomType);
        }
    }

    public void OnSlotEmpty(ItemSlot slot)
    {
        for (int i = 0; i < currentItemsPerSlot; i++)
        {
            SpawnItemInSlot(slot);
        }
    }
}
