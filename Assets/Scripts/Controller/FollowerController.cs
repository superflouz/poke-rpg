using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : Controller
{
    public CharacterMovement following;
    List<Vector2> path = new List<Vector2>();

    private readonly float delayTime = 0.2f;
    private float delayTimer;


    private readonly float recalculateTime = 0.2f;
    private float recalculateTimer;

    // Update is called once per frame
    override protected void Update()
    {
        if (path.Count > 1)
        {
            if (!controlled.Moving && delayTimer <= 0)
            {
                controlled.Direction = path[0];
                path.RemoveAt(0);

                delayTimer = delayTime;
            }
            else
            {
                delayTimer -= Time.deltaTime;
            }
        }
        else
        {
            controlled.Direction = Vector2.zero;
        }
        if (recalculateTimer <= 0)
        {
            path = Pathfinding.FindPathToDestination(controlled.Destination, following.Destination, controlled.size);
            recalculateTimer = recalculateTime;
        }
        else
            recalculateTimer -= Time.deltaTime;
    }
}
