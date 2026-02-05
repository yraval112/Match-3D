using System.Collections.Generic;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public Transform spawnPoint;
    public float itemOffsetY = 0.25f;

    public List<SelectableItem> itemList = new List<SelectableItem>();
    private LevelManager levelManager;

    public void SetLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }

    public void AddItem(SelectableItem item)
    {
        item.transform.SetParent(transform);
        item.transform.position = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            spawnPoint.position.z + itemOffsetY * itemList.Count
        );

        item.SetSlot(this);
        item.SetClickable(false);

        itemList.Add(item);
        UpdateTopItem();
    }

    public void RemoveTopItem()
    {
        if (itemList.Count == 0) return;

        SelectableItem removed = itemList[0];
        itemList.RemoveAt(0);
        UpdateTopItem();

        if (itemList.Count == 0)
        {
            if (levelManager != null)
            {
                levelManager.OnSlotEmpty(this);
            }
        }
    }

    void UpdateTopItem()
    {
        foreach (var item in itemList)
        {
            item.SetClickable(false);
        }

        if (itemList.Count > 0)
        {
            SelectableItem topItem = itemList[0];
            topItem.SetClickable(true);
        }
    }
}
