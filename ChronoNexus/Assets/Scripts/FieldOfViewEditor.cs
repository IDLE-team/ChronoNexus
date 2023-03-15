using UnityEngine;
using System.Collections;
using UnityEditor;

/*
[CustomEditor(typeof(Enemy))]
public class FieldOfViewEditor : Editor
{
    private void OnSceneGUI()
    {
        Enemy fow = (Enemy)target;
        Handles.color = Color.white;
        Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2, false);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2, false);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;

        if (fow.player != null && fow.canSeePlayer)
        {
            Handles.DrawLine(fow.transform.position, fow.player.position);
        }
    }
}*/