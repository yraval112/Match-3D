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

    private int currentItemsPerSlot;

    void Awake()
    {
        Instance = this;

    }

    void Start()
    {
        currentItemsPerSlot = (difficulty == Difficulty.Easy) ? itemsPerSlot_Easy : itemsPerSlot_Hard;
        SpawnInitialItems();
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
