using UnityEngine;

public class PatrolRoute : MonoBehaviour
{
    [field: SerializeField, Tooltip("Waypoints array")]
    private Transform[] waypoints;

    public Vector2[] GetWaypoints()
    {
        var returnWaypoints = new Vector2[waypoints.Length];
        for(int i = 0; i < returnWaypoints.Length; i++)
        {
            returnWaypoints[i] = waypoints[i].position;
        }
        
        return returnWaypoints;
    }
    
    public void GatherWaypoints()
    {
        waypoints = new Transform[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            waypoints[i] = transform.GetChild(i);
        }
    }

    public void GatherWaypointsOppositeSide()
    {
        waypoints = new Transform[transform.childCount];

        for (int i = transform.childCount - 1, j = 0; i >= 0; i--, j++)
        {
            waypoints[j] = transform.GetChild(i);
        }
    }

    private void OnDrawGizmos()
    {
        foreach (var transform1 in waypoints)
        {
            if (transform1 == null)
                continue;

            Gizmos.DrawIcon(transform1.position, "", false, Color.green);
        }
    }
}
