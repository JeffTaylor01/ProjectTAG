using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingDoor : MonoBehaviour
{
    public List<Transform> waypoints;
    public float moveSpeed = 2;
    public int target;

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, waypoints[target].position, moveSpeed * Time.deltaTime);

        if (transform.position == waypoints[target].position)
        {
            if (target == waypoints.Count - 1)
            {
                target = 0;
            }
            else
            {
                target += 1;
            }
        }
    }

    

}
