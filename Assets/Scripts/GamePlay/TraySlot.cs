using DG.Tweening;
using UnityEngine;

public class TraySlot : MonoBehaviour
{
    public SelectableItem currentItem;

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public void SetItem(SelectableItem item)
    {
        currentItem = item;
        item.transform.position = transform.position;
        // item.transform.localScale = Vector3.one * 0.8f;
        item.transform.DOScale(2f, 0.2f);
        item.SetClickable(false);
        item.transform.SetParent(transform);
    }

    public void Clear()
    {
        currentItem = null;
    }
}
