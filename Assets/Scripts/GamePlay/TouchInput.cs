using UnityEngine;

public class TouchInput : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                SelectableItem selectableItem = hit.collider.GetComponent<SelectableItem>();
                if (selectableItem != null)
                {
                    selectableItem.Select();
                }
            }
        }
    }
}
