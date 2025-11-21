using UnityEngine;

public class BugGenerator : MonoBehaviour
{
    public bool canGenerate;
    private float spawnCount = 0;
    [SerializeField] 
    public float spawnTime = 5;
    [SerializeField] 
    public GameObject setBug;
    [SerializeField] 
    public LineRenderer lineRend;

    private Vector3 spawnPos;

    void Start()
    {
        spawnPos = lineRend.GetPosition(0);
    }

    void Update()
    {
        if (canGenerate)
        {
            spawnCount += Time.deltaTime;
        }

        if (spawnCount >= spawnTime)
        {
            spawnCount = 0;
            Instantiate(setBug, spawnPos, Quaternion.identity);
        }
    }
}
