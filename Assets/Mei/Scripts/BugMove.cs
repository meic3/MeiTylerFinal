using UnityEngine;

public class BugMove : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRend;
    [SerializeField] private float moveSpeed = 1f;

    private Vector3[] points;
    private float t = 0f; // 0 to 1 along the whole path

    void Start()
    {
        // Cache all positions at start
        points = new Vector3[lineRend.positionCount];
        lineRend.GetPositions(points);
    }

    void Update()
    {
        if (points.Length < 2) return;

        t += moveSpeed * Time.deltaTime;



        // Find where we are along the path
        Vector3 pos = GetPointOnLine(points, t);
        transform.position = pos;
    }

    // Returns a position between all points
    Vector3 GetPointOnLine(Vector3[] pts, float t)
    {
        // scale t to array size
        float scaledT = t * (pts.Length - 1);
        int index = Mathf.FloorToInt(scaledT);
        int nextIndex = Mathf.Min(index + 1, pts.Length - 1);

        float lerpT = scaledT - index;

        return Vector3.Lerp(pts[index], pts[nextIndex], lerpT);
    }
}
