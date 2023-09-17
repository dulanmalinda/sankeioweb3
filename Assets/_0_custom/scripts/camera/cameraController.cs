using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();

    [Header("Generals")]
    public Vector3 offset;
    public float smoothTime = 0.5f;
    public float minZoom;
    public float maxZoom;
    public float zoomLimiter;

    private Vector3 velocity;
    private snakeManager snakemanager;

    private void Start()
    {
        snakemanager = GameObject.FindObjectOfType<snakeManager>();
    }

    private void FixedUpdate()
    {
        if (targets.Count == 0)
        {
            return;
        }

        Move();
        Zoom();
    }

    void Move()
    {
        Vector3 centerPoint = getCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom,minZoom, getGreatestDistance()/zoomLimiter);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize,newZoom,Time.deltaTime);
    }

    float getGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }

    Vector3 getCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }
        var bounds = new Bounds(targets[0].position,Vector3.zero);
        for (int i = 0; i<targets.Count;i++)
        {
            bounds.Encapsulate(targets[i].position);
        }

        return bounds.center;
    }
}
