using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public ObjectType objectType;
        public GameObject prefab;
        public int size;
    }

    public List<Pool> pools;
    private Dictionary<ObjectType, Queue<SelectableItem>> poolDictionary;

    void Awake()
    {
        poolDictionary = new Dictionary<ObjectType, Queue<SelectableItem>>();

        foreach (Pool pool in pools)
        {
            Queue<SelectableItem> objectQueue = new Queue<SelectableItem>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab, transform);
                SelectableItem selectableItem = obj.GetComponent<SelectableItem>();
                obj.SetActive(false);
                objectQueue.Enqueue(selectableItem);
            }

            poolDictionary.Add(pool.objectType, objectQueue);
        }
    }

    public SelectableItem SpawnFromPool(ObjectType objectType)
    {
        if (!poolDictionary.ContainsKey(objectType))
        {
            return null;
        }

        SelectableItem itemToSpawn = poolDictionary[objectType].Dequeue();
        itemToSpawn.gameObject.SetActive(true);
        poolDictionary[objectType].Enqueue(itemToSpawn);

        return itemToSpawn;
    }

    public void ReturnToPool(SelectableItem item)
    {
        item.gameObject.SetActive(false);
    }
}
