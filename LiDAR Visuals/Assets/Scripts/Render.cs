using UnityEngine;
using System.Collections.Generic;

public class Render : MonoBehaviour
{
    public float displayScale = 0.02f;
    public Color nearColor = Color.green;
    public Color farColor = Color.red;
    private Queue<GameObject> points = new Queue<GameObject>();
    private float lastPitch = -1f;
    private float lastYaw = -1f;

    void Update(){
        float pitch = PullData.CurrentPitch;
        float yaw = PullData.CurrentYaw;
        float distance = PullData.CurrentDistance;

        if (pitch == lastPitch && yaw == lastYaw) return;
        lastPitch = pitch;
        lastYaw = yaw;

        float pitchRad = pitch * Mathf.Deg2Rad;
        float yawRad = yaw * Mathf.Deg2Rad;

    
        Vector3 pos = new Vector3(
            distance * Mathf.Cos(pitchRad) * Mathf.Sin(yawRad),
            distance * Mathf.Sin(pitchRad),
            distance * Mathf.Cos(pitchRad) * Mathf.Cos(yawRad)
        ) * displayScale;

        SpawnPoint(pos, distance);
    }

    void SpawnPoint(Vector3 pos, float distance){
        GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        dot.transform.parent = transform;
        dot.transform.localPosition = pos;
        dot.transform.localScale = Vector3.one * 0.10f;

        // Color by distance
        float t = Mathf.Clamp01(distance);
        dot.GetComponent<Renderer>().material.color = Color.Lerp(nearColor, farColor, t);

        // Remove collider for performance
        Destroy(dot.GetComponent<SphereCollider>());

        points.Enqueue(dot);
    }
}