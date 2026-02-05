using UnityEngine;

public enum Difficulty { Easy, Hard }

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public ItemSlot[] itemSlots;
    public GameObject[] itemPrefabs;
    public ObjectPool objectPool;

    [Header("Difficulty Settings")]
    public Difficulty difficulty = Difficulty.Easy;
    public int itemsPerSlot_Easy = 5;
    public int itemsPerSlot_Hard = 10;

    [Header("Grid Layout Settings")]
    public int gridRows = 3;
    public int gridColumns = 4;
    public float slotSpacingX = 1.5f;
    public float slotSpacingY = 1.5f;
    public Vector3 gridStartPosition = Vector3.zero;

    private int currentItemsPerSlot;

    void Awake()
    {
        Instance = this;
        SetupGridLayout();
    }

    void Start()
    {
        currentItemsPerSlot = (difficulty == Difficulty.Easy) ? itemsPerSlot_Easy : itemsPerSlot_Hard;
        SpawnInitialItems();
    }

    void SetupGridLayout()
    {
        int totalSlots = gridRows * gridColumns;
        itemSlots = new ItemSlot[totalSlots];

        int slotIndex = 0;
        for (int row = 0; row < gridRows; row++)
        {
            for (int col = 0; col < gridColumns; col++)
            {
                // Calculate grid position
                Vector3 slotPosition = gridStartPosition + new Vector3(
                    col * slotSpacingX,
                    row * slotSpacingY,
                    0
                );

                // Create slot at position
                GameObject slotObject = new GameObject("ItemSlot_" + row + "_" + col);
                slotObject.transform.position = slotPosition;
                slotObject.transform.SetParent(transform);

                ItemSlot slot = slotObject.AddComponent<ItemSlot>();
                itemSlots[slotIndex] = slot;

                // Create spawn point for the slot
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
        ObjectType randomType = (ObjectType)Random.Range(0, System.Enum.GetNames(typeof(ObjectType)).Length);
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
