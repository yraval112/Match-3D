using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            GameManager.Instance.GameOver();
            return;
        }

        emptySlot.SetItem(item);
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
