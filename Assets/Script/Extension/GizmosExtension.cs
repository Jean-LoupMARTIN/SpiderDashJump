using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GizmosExtension
{
    public static void GizmoDrawCircle(Vector3 pos, Vector3 forward, float radius, int resolution = 30)
    {
        Quaternion rot = Quaternion.LookRotation(forward);
        float drot = 360 / resolution;

        for (int i = 0; i < resolution; i++)
        {
            Vector3 from = pos + rot * Vector3.up * radius;
            rot *= Quaternion.Euler(Vector3.forward * drot);
            Vector3 to = pos + rot * Vector3.up * radius;

            Gizmos.DrawLine(from, to);
        }
    }

    public static void GizmoDrawWireCone(Vector3 pos, Vector3 forward, float length, float angle)
    {
        angle /= 2;
        Quaternion rot;

        rot = Quaternion.LookRotation(forward);
        rot *= Quaternion.Euler(Vector3.right * angle);
        Gizmos.DrawLine(pos, pos + rot * Vector3.forward * length);

        rot = Quaternion.LookRotation(forward);
        rot *= Quaternion.Euler(Vector3.left * angle);
        Gizmos.DrawLine(pos, pos + rot * Vector3.forward * length);

        rot = Quaternion.LookRotation(forward);
        rot *= Quaternion.Euler(Vector3.up * angle);
        Gizmos.DrawLine(pos, pos + rot * Vector3.forward * length);

        rot = Quaternion.LookRotation(forward);
        rot *= Quaternion.Euler(Vector3.down * angle);
        Gizmos.DrawLine(pos, pos + rot * Vector3.forward * length);

        angle = Mathf.Deg2Rad * angle;
        float adj = Mathf.Cos(angle) * length;
        float opp = Mathf.Sin(angle) * length;

        GizmoDrawCircle(pos + forward * adj, forward, opp);
    }
}
