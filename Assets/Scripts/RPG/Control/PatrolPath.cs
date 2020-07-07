using System;
using UnityEngine;

namespace RPG.Control
{
    public class PatrolPath : MonoBehaviour
    {
        private const float WayPointGizmoRadius = 0.3f;
        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                var wayPoint = GetWaypoint(i);
                var nextIndex = GetNextIndex(i);
                Gizmos.DrawSphere(wayPoint, WayPointGizmoRadius);
                Gizmos.DrawLine(wayPoint, GetWaypoint(nextIndex));
            }
        }

        public int GetNextIndex(int i)
        {
            return (i + 1) % transform.childCount;
        }

        public Vector3 GetWaypoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}