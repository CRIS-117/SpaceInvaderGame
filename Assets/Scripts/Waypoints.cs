using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;

    private void OnDrawGizmos()
    {
        for(int waypointIndex = 0; waypointIndex < waypoints.Length - 1; waypointIndex++)
        {
            Gizmos.DrawLine(waypoints[waypointIndex].position, waypoints[waypointIndex + 1].position);
        }
        
    }
}
