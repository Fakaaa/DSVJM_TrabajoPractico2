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

    [SerializeField] WallParent obstaclesPrefabs;

    public int poolSize;

    Queue<WallParent> obstaclesPool = new Queue<WallParent>();

    public int minDistanceWallParts;

    public void GrowPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            var instanceToAdd = Instantiate(obstaclesPrefabs, transform.position, Quaternion.identity, transform);
            AddToPool(instanceToAdd);
        }
    }

    public void AddToPool(WallParent instance)
    {
        instance.gameObject.SetActive(false);
        instance.transform.position = transform.position;
        obstaclesPool.Enqueue(instance);
    }

    public WallParent GetObstacleFromPool()
    {
        if (obstaclesPool.Count == 0)
            GrowPool();

        var instance = obstaclesPool.Dequeue();

        if (instance != null)
        {
            Vector3 randomPositionWallTop = new Vector3(instance.transform.position.x,Random.Range(4f, 10f), instance.transform.position.z);
    
            if(instance.GetWall(WallParent.TypeWall.Top) != null)
            {
                instance.SetWallPosition(randomPositionWallTop, WallParent.TypeWall.Top);
                instance.SetWallPosition(randomPositionWallTop - 
                    new Vector3(0, minDistanceWallParts+instance.GetWall(WallParent.TypeWall.Top).localScale.y,0), 
                    WallParent.TypeWall.Bot);
            }

            instance.gameObject.SetActive(true);
        }

        return instance;
    }
}