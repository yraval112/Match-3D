using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ItemSlot : MonoBehaviour
{
    public Transform spawnPoint;
    public float itemOffsetY = 0.25f;
    public float itemMoveSpeed = 0.3f;

    public List<SelectableItem> itemList = new List<SelectableItem>();
    private LevelManager levelManager;

    public void SetLevelManager(LevelManager manager)
    {
        levelManager = manager;
    }

    public void AddItem(SelectableItem item)
    {
        item.SetSlot(this);
        item.SetClickable(false);

        itemList.Add(item);

        StartCoroutine(AnimateItemPosition(item, itemList.Count - 1));
        UpdateTopItem();
    }

    private IEnumerator AnimateItemPosition(SelectableItem item, int index)
    {
        Vector3 targetPosition = new Vector3(
            spawnPoint.position.x,
            spawnPoint.position.y,
            spawnPoint.position.z + itemOffsetY * index
        );

        item.transform.DOMove(targetPosition, itemMoveSpeed).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(itemMoveSpeed);

        item.transform.SetParent(transform);
        item.transform.position = targetPosition;
    }

    public void RemoveTopItem()
    {
        if (itemList.Count == 0) return;

        SelectableItem removed = itemList[0];
        itemList.RemoveAt(0);

        // Animate remaining items down
        StartCoroutine(AnimateItemsDown());
        UpdateTopItem();

        if (itemList.Count == 0)
        {
            if (levelManager != null)
            {
                levelManager.OnSlotEmpty(this);
            }
        }
    }

    private IEnumerator AnimateItemsDown()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Vector3 targetPosition = new Vector3(
                spawnPoint.position.x,
                spawnPoint.position.y,
                spawnPoint.position.z + itemOffsetY * i
            );
            itemList[i].transform.DOMove(targetPosition, itemMoveSpeed).SetEase(Ease.OutQuad);
        }
        yield return new WaitForSeconds(itemMoveSpeed);
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
