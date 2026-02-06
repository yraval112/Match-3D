using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelDataSO", order = 1)]
public class LevelDataSO : ScriptableObject
{
    public List<LevelData> levels = new List<LevelData>();


}
[System.Serializable]
public class LevelData
{
    public int levelNumber;
    public int gridRows;
    public int gridColumns;
    public float slotSpacingX;
    public float slotSpacingY;
    public float gridZDistance;
    public Vector3 gridStartPosition;
    // Level time limit in minutes
    public float levelTimeLimit = 1f;

    public LevelData(int rows, int columns, float spacingX, float spacingY, float zDistance, Vector3 startPosition)
    {
        gridRows = rows;
        gridColumns = columns;
        slotSpacingX = spacingX;
        slotSpacingY = spacingY;
        gridZDistance = zDistance;
        gridStartPosition = startPosition;
    }
}