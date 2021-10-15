using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstalcesPool : MonoBehaviour
{
    #region Singleton

    private static ObstalcesPool instance;
    public static ObstalcesPool Instance
    {
        get
        {
            if (instance == null)
                instance = new ObstalcesPool();

            return instance;
        }
    }

    public void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        GrowPool();
    }

    #endregion

    public List<GameObject> obstaclesPrefabs;

    public int poolSize;

    Queue<GameObject> obstaclesPool = new Queue<GameObject>();

    public void GrowPool()
    {
        int randVal = 0;

        for (int i = 0; i < poolSize; i++)
        {
            if (obstaclesPrefabs[randVal] != null)
            {
                randVal = Random.Range(0, obstaclesPrefabs.Count);
                var instanceToAdd = Instantiate(obstaclesPrefabs[randVal], transform.position, Quaternion.identity, transform);
                instanceToAdd.transform.parent = transform;
                AddToPool(instanceToAdd);
            }
        }
    }

    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        instance.transform.position = transform.position;
        obstaclesPool.Enqueue(instance);
    }

    public GameObject GetObstacleFromPool()
    {
        if (obstaclesPool.Count == 0)
            GrowPool();

        var instance = obstaclesPool.Dequeue();

        if (instance != null)
            instance.SetActive(true);

        return instance;
    }
}