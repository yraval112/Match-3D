using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance;

    public List<TraySlot> traySlots = new List<TraySlot>();
    private List<SelectableItem> trayItems = new List<SelectableItem>();

    void Awake()
    {
        Instance = this;
    }

    public void AddToTray(SelectableItem item)
    {
        TraySlot emptySlot = traySlots.FirstOrDefault(s => s.IsEmpty());

        if (emptySlot == null)
        {
            //    GameManager.Instance.GameOver();
            GameManager.onGameOver?.Invoke();
            return;
        }

        // Animate item to tray position
        StartCoroutine(AnimateItemToTray(item, emptySlot));
    }

    private IEnumerator AnimateItemToTray(SelectableItem item, TraySlot targetSlot)
    {
        // Move item to tray slot position
        item.transform.DOMove(targetSlot.transform.position, 0.3f).SetEase(Ease.InQuad);

        // Scale down during movement
        item.transform.DOScale(0.8f, 0.3f);

        yield return new WaitForSeconds(0.3f);

        // Set item in tray
        targetSlot.SetItem(item);
        trayItems.Add(item);

        CheckMatch();
    }

    void CheckMatch()
    {
        var grouped = trayItems.GroupBy(i => i.objectType);

        foreach (var group in grouped)
        {
            if (group.Count() >= 3)
            {
                RemoveMatch(group.Take(3).ToList());
                break;
            }
        }
    }

    void RemoveMatch(List<SelectableItem> matchedItems)
    {
        CoinSystem.Instance.AddCoins(CoinSystem.Instance.coinsPerMatch);

        foreach (var item in matchedItems)
        {
            TraySlot slot = traySlots.Find(s => s.currentItem == item);
            if (slot != null) slot.Clear();

            trayItems.Remove(item);
            Destroy(item.gameObject);
        }

        RearrangeTray();
        GameManager.Instance.CheckWin();
    }

    void RearrangeTray()
    {
        foreach (var slot in traySlots)
            slot.Clear();

        for (int i = 0; i < trayItems.Count; i++)
        {
            traySlots[i].SetItem(trayItems[i]);
        }
    }
}
