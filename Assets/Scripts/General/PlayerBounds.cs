using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBounds : MonoBehaviour
{
    public Vector3 downLeftLimit = new Vector3(-1, -1);
    public Vector3 upRightLimit = new Vector3(1, 1);

    private void OnDrawGizmos()
    {
        Color baseGizmoColor = Gizmos.color;
        Gizmos.color = Color.green;
        Gizmos.DrawLine(downLeftLimit, new Vector3(downLeftLimit.x, upRightLimit.y));
        Gizmos.DrawLine(new Vector3(downLeftLimit.x, upRightLimit.y), upRightLimit);
        Gizmos.DrawLine(upRightLimit, new Vector3(upRightLimit.x, downLeftLimit.y));
        Gizmos.DrawLine(new Vector3(upRightLimit.x, downLeftLimit.y), downLeftLimit);
        Gizmos.color = baseGizmoColor;
    }
}
