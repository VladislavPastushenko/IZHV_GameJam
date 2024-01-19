using UnityEngine;

public class TrailRenderer : MonoBehaviour
{
    public Transform target; // The triangle to follow
    public LineRenderer lineRenderer; // Reference to the LineRenderer component
    public int maxPoints = 1000; // Maximum number of points in the trail
    public float pointSpacing = 0.1f; // Distance between points

    private void Start()
    {
        lineRenderer.positionCount = 0; // Initial number of points
    }

    private void Update()
    {
        if (target != null && !GameManager.Instance.mGameLost)
        {
            // Add a new point to the trail
            AddPoint(target.position);
        }
        else if (lineRenderer.positionCount > 1)
        {
            RemoveOldestPoint();
        }
    }

    void AddPoint(Vector3 point)
    {
        // If the maximum number of points is exceeded, remove the oldest point
        if (lineRenderer.positionCount >= maxPoints)
        {
            RemoveOldestPoint();
        }

        MovePoints();

        // Add a new point
        lineRenderer.positionCount++;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, point);
    }

    void MovePoints()
    {
        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i - 1, lineRenderer.GetPosition(i) - new Vector3(0.03f, 0, 0));
        }
    }

    void RemoveOldestPoint()
    {
        // Shift all points to the  to remove the oldest one
        for (int i = 1; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i - 1, lineRenderer.GetPosition(i));
        }

        // Decrease the number of points
        lineRenderer.positionCount--;
    }
}
