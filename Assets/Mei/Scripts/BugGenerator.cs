using UnityEngine;

public class BugGenerator : MonoBehaviour
{
    private float spawnCount = 0;
    [SerializeField] public float spawnTime = 5;
    [SerializeField] public GameObject setBug;
    [SerializeField] public LineRenderer lineRend;

    private Vector3 spawnPos;

    void Start()
    {
        // take the first point in the line
        spawnPos = lineRend.GetPosition(0);
    }

    void Update()
    {
        spawnCount += Time.deltaTime;

        if (spawnCount >= spawnTime)
        {
            spawnCount = 0;
            Instantiate(setBug, spawnPos, Quaternion.identity);
        }
    }
}
