#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatrolRoute))]
public class PatrolRouteEditor : Editor //код TeaWalker
{
    public override void OnInspectorGUI()
    {
        PatrolRoute myTarget = (PatrolRoute)target; 
        base.OnInspectorGUI();

        if (GUILayout.Button(nameof(myTarget.GatherWaypoints)))
            myTarget.GatherWaypoints();

        if (GUILayout.Button(nameof(myTarget.GatherWaypointsOppositeSide)))
            myTarget.GatherWaypointsOppositeSide();
    }
}

#endif