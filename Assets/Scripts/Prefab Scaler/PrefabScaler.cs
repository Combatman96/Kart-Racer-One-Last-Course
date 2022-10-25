using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class PrefabScaler : MonoBehaviour
{
    public float scale = 10f;

    public Transform rootPrefab;
    public List<GameObject> prefabs;

    [Button]
    public void ScalePrefab()
    {
        foreach(var prefab in prefabs)
        {
            var root = Instantiate(rootPrefab, Vector3.zero, Quaternion.identity);
            var obj = Instantiate(prefab, Vector3.zero, Quaternion.identity, root.GetChild(0));
            string newName = obj.name;
            root.name = newName;
            root.GetChild(0).localScale = Vector3.one * scale;
            obj.AddComponent<MeshCollider>();
        }
    }

    public void AddCollider()
    {
        foreach(var obj in prefabs)
        {
            var child = obj.transform.GetChild(0).GetChild(0);
            child.gameObject.AddComponent<MeshCollider>();
        }
    }
}
