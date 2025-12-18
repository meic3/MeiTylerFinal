using System.Collections;
using UnityEngine;

public class BugMove : MonoBehaviour
{
    Bug bug;

    private LineRenderer lineRend;
    [SerializeField] 
    private float moveSpeed = 1f;

    private Vector3[] points;
    //private float t = 0f; 
    private Vector3 losePos;

    private float[] cumulativeDistances;
    private float totalLength;
    private float distanceTravelled = 0f;

    void Start()
    {
        bug = GetComponent<Bug>();

        points = new Vector3[lineRend.positionCount];
        lineRend.GetPositions(points);
        losePos = points[points.Length - 1];

        cumulativeDistances = new float[points.Length];
        cumulativeDistances[0] = 0f;

        for (int i = 1; i < points.Length; i++)
        {
            cumulativeDistances[i] =
                cumulativeDistances[i - 1] + Vector3.Distance(points[i], points[i - 1]);
        }

        totalLength = cumulativeDistances[cumulativeDistances.Length - 1];
        losePos = points[points.Length - 1];
    }

    public float Speed()
    {
        return bug.isSlowed ? moveSpeed * (1 - bug.effectiveSlowInstance.slow) :  moveSpeed;
    }

    void Update()
    {
        if (transform.position == losePos)
        {
            PhaseManager phaseManager = FindFirstObjectByType<PhaseManager>();
            phaseManager.LoseGame();
            Debug.Log("Lost");
        }
        //if (points.Length < 2) return;

        distanceTravelled += Speed() * Time.deltaTime;

        if (distanceTravelled >= totalLength)
        {
            transform.position = losePos;
            return;
        }

        transform.position = GetPointAtDistance(distanceTravelled);
    }

    Vector3 GetPointAtDistance(float distance)
    {
        // find segment using cumulative distance array
        int index = 0;
        while (index < cumulativeDistances.Length - 1 &&
               cumulativeDistances[index + 1] < distance)
        {
            index++;
        }

        float segmentDist = cumulativeDistances[index + 1] - cumulativeDistances[index];
        float distIntoSegment = distance - cumulativeDistances[index];
        float lerpT = distIntoSegment / segmentDist;

        return Vector3.Lerp(points[index], points[index + 1], lerpT);
    }

    /*
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
    }*/

    public void SetRoute(LineRenderer route)
    {
        lineRend = route;
    }

    
}
