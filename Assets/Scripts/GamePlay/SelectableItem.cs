using UnityEngine;
using DG.Tweening;
using System.Collections;

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

        StartCoroutine(SelectAnimation());
    }

    private IEnumerator SelectAnimation()
    {
        // Scale up animation
        transform.DOScale(1.1f, 0.1f).SetEase(Ease.OutBack);
        yield return new WaitForSeconds(0.1f);

        // Scale back
        transform.DOScale(1f, 0.1f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.1f);

        // Move to tray with fade out
        transform.DOScale(0.5f, 0.3f);
        //transform.DOMove(transform.position + Vector3.up * 2f, 0.3f).SetEase(Ease.InQuad);
        yield return new WaitForSeconds(0.3f);

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
    Cake,
    Drum,
    Cherry,
    Pumpkin
}