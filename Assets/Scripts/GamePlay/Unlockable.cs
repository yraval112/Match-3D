using UnityEngine;

// Attach this to a GameObject that should be unlocked at a specific level.
// Set `unlockLevel` and assign one or more `targets` (GameObjects to enable/disable).
public class Unlockable : MonoBehaviour
{
    public int unlockLevel = 1;
    public GameObject[] targets;

    void Start()
    {
        if (LevelManager.Instance != null)
        {
            UpdateState(LevelManager.Instance.levelNumber);
            LevelManager.onLevelChanged += UpdateState;
        }
    }

    void OnDestroy()
    {
        LevelManager.onLevelChanged -= UpdateState;
    }

    void UpdateState(int level)
    {
        bool unlocked = level >= unlockLevel;
        if (targets == null || targets.Length == 0)
        {
            gameObject.SetActive(unlocked);
            return;
        }

        foreach (var t in targets)
        {
            if (t != null) t.SetActive(unlocked);
        }
    }
}
