using UnityEngine;

public class SelectableItem : MonoBehaviour
{
    public ObjectType objectType;

    public bool isClickable;
    private ItemSlot parentSlot;
    public SelectableItem()
    {
        isClickable = false;
    }
    public void SetSlot(ItemSlot slot)
    {
        parentSlot = slot;
    }

    public void SetClickable(bool value)
    {
        isClickable = value;
    }

    public void Select()
    {
        if (!isClickable) return;

        parentSlot.RemoveTopItem();
        MatchManager.Instance.AddToTray(this);
    }
}
public enum ObjectType
{
    Apple,
    Banana,
    Shoe,
    Toy,
    Cake
}