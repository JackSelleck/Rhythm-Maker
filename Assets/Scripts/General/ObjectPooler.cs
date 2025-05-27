using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static Dictionary<string, Queue<Component>> poolDictionary = new Dictionary<string, Queue<Component>>();

    public static void SetupPool<T>(T pooledItemPrefab, int poolSize, string dictionaryEntry)
    {
        
    }
    
}
