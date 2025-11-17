using UnityEngine;

public class BugMove : MonoBehaviour
{
    [SerializeField] 
    private LineRenderer lineRend;
    [SerializeField] 
    private float moveSpeed = 1f;

    private Vector3[] points;
    private float t = 0f; 
    void Start()
    {
        points = new Vector3[lineRend.positionCount];
        lineRend.GetPositions(points);
    }

    void Update()
    {
        if (points.Length < 2) return;

        t += moveSpeed * Time.deltaTime;


        Vector3 pos = GetPointOnLine(points, t);
        transform.position = pos;
    }

    Vector3 GetPointOnLine(Vector3[] pts, float t)
    {
        float scaledT = t * (pts.Length - 1);
        int index = Mathf.FloorToInt(scaledT);
        int nextIndex = Mathf.Min(index + 1, pts.Length - 1);
        if (index >= pts.Length) {
            Destroy(gameObject);
            return Vector3.zero;
        }
        
        float lerpT = scaledT - index;

        return Vector3.Lerp(pts[index], pts[nextIndex], lerpT);
    }
}
