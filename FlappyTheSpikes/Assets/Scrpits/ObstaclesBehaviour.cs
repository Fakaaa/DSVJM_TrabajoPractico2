using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesBehaviour : MonoBehaviour
{
    [SerializeField] Transform startPos;
    [SerializeField] Transform endPos;

    [SerializeField] float speedObstacles;
    [SerializeField] float timeToActivateObstacle;
    float t;

    [SerializeField] int amountObstaclesPerTime;
    public bool obstaclesActivated;

    List<WallParent> obstaclesMoving = new List<WallParent>();

    void Start()
    {
        t = 0;
        obstaclesActivated = true;
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
}
