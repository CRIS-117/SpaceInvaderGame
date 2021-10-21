using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3 : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed;
    List <Transform> waypointsList;
    int currentWaypoint;
    
    void Update()
    {
        Move();
    }

    private void Move()
    {
         if(currentWaypoint < waypointsList.Count)
        {
            var targetPosition = waypointsList[currentWaypoint].position;
            float delta = moveSpeed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, delta);
            if(transform.position == targetPosition)
            currentWaypoint++;
        }
        else
                Destroy(gameObject);
    }

    public void SetPath(GameObject waypointsObject)
    {
        waypointsList = new List<Transform>();
        foreach(Transform child in waypointsObject.transform)
            waypointsList.Add(child);
    }
}
