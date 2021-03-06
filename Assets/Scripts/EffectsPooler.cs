﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EffectsPooler : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static EffectsPooler Instance;

    private void Awake() {
        Instance = this;
    }


    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> effectsDictionary;

    void Start()
    {
        effectsDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach (var item in pools)
        {
            Queue<GameObject> objPool = new Queue<GameObject>();
            for (int i = 0; i < item.size; i++)
            {
                GameObject obj = Instantiate(item.prefab);
                obj.SetActive(false);
                objPool.Enqueue(obj);
            }
            effectsDictionary.Add(item.tag, objPool);

        }

    }
    
    public GameObject SpawnFromDictionary(string tag, Vector3 position, Quaternion rotation)
    {
      
            
            GameObject objectToSpawn = effectsDictionary[tag].Dequeue();
            objectToSpawn.SetActive(true);
            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            objectToSpawn.GetComponent<IPooledObject>().OnObjectPooled();
            effectsDictionary[tag].Enqueue(objectToSpawn);
            return objectToSpawn;
        
    }

   
}
