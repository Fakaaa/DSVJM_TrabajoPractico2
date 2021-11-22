using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesBehaviour : MonoBehaviour
{
    #region EXPOSED FIELDS
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    [SerializeField] float speedObstacles;
    [SerializeField] float timeToActivateObstacle;
    [SerializeField] int amountObstaclesPerTime;

    [SerializeField] List<float> speedsPerDificulty;

    public bool obstaclesActivated;
    #endregion

    #region PRIVATE FIELDS
    List<WallParent> obstaclesMoving = new List<WallParent>();
    float t;
    GameManager gmReference;
    #endregion

    void Start()
    {
        t = 0;
        obstaclesActivated = true;
        gmReference = GameManager.Instance;

        if(gmReference != null)
        {
            gmReference.OnChangeDificulty += IncreaseDificulty;
        }

        speedObstacles = speedsPerDificulty[0];
    }

    void Update()
    {
        if (!obstaclesActivated)
            return;

        if (t < timeToActivateObstacle)
            t += Time.deltaTime;
        else
        {
            if(obstaclesMoving.Count < amountObstaclesPerTime)
                obstaclesMoving.Add(ObstalcesPool.Instance.GetObstacleFromPool());
            
            t = 0;
        }

        for (int i = 0; i < obstaclesMoving.Count; i++)
        {
            if(obstaclesMoving[i] != null)
            {
                if(obstaclesMoving[i].transform.position != endPos.position)
                {
                    obstaclesMoving[i].transform.position = Vector3.MoveTowards(obstaclesMoving[i].transform.position, endPos.position, Time.deltaTime * speedObstacles);
                }
                else
                {
                    ObstalcesPool.Instance.AddToPool(obstaclesMoving[i]);
                    obstaclesMoving.RemoveAt(i);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (gmReference != null)
        {
            gmReference.OnChangeDificulty -= IncreaseDificulty;
        }
    }

    public void ResetObstacles()
    {
        for (int i = 0; i < obstaclesMoving.Count; i++)
        {
            if (obstaclesMoving[i] != null)
            {
                ObstalcesPool.Instance.AddToPool(obstaclesMoving[i]);
                obstaclesMoving.RemoveAt(i);
            }
        }
        obstaclesActivated = true;
    }

    public void IncreaseDificulty()
    {
        if (gmReference == null)
            return;

        switch (gmReference.GetDificulty()) 
        {
            case GameManager.Dificulty.Easy:

                speedObstacles = speedsPerDificulty[0];
                ObstalcesPool.Instance.minDistanceWallParts = 5;

                break;
            case GameManager.Dificulty.Medium:

                speedObstacles = speedsPerDificulty[1];
                ObstalcesPool.Instance.minDistanceWallParts = 3;

                break;
            case GameManager.Dificulty.Hard:

                speedObstacles = speedsPerDificulty[2];

                break;
            case GameManager.Dificulty.Nightmare:

                ObstalcesPool.Instance.minDistanceWallParts = 2;

                break;
        }
    }
}
