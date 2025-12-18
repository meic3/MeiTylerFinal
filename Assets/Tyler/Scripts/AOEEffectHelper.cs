using UnityEngine;

public class AOEEffectHelper : MonoBehaviour
{
    public float lifetime = .1f;

    [HideInInspector] public float radius;
    [SerializeField] GameObject spr;
    public int segments = 32;

    LineRenderer line;

    void Start()
    {
        Destroy(gameObject, lifetime);
        spr.transform.localScale = new Vector3(radius, radius, 1);

        line = GetComponent<LineRenderer>();
        line.loop = true;
        line.positionCount = segments;

        DrawCircle();
    }

    void DrawCircle()
    {
        float angleStep = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * (angleStep * i);
            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            line.SetPosition(i, new Vector3(x, y, 0) + transform.position);
        }
    }
}
