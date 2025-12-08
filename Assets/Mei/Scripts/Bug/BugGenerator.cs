using UnityEngine;

public class BugGenerator : MonoBehaviour
{
    public bool canGenerate;
    private float spawnCount = 0;
    [SerializeField] 
    public float spawnTime = 5;
    [SerializeField] 
    public GameObject setFly, setGnat, setStinkbug, setDungBeetle, setCockroach;

    [SerializeField] 
    public LineRenderer lineRend;

    public WaveManager currentWave;
    private int spawnCurrentNum = 0;
    private int bugsSpawnedRN = 0;
    private int totalBugsToSpawn = 0;
    private int bugsSpawned = 0;
    private int bugsAlive = 0;

    public int deadBug = 0;

    private Vector3 spawnPos;

    public bool isPaused = false;


    void Start()
    {
        spawnPos = lineRend.GetPosition(0);
    }

    void Update()
    {
        if (canGenerate && currentWave != null && !isPaused)
        {
            spawnCount += Time.deltaTime;
        }

        if (spawnCount >= spawnTime && canGenerate && currentWave != null)
        {
            spawnCount = 0;
            SpawnNextBug();
            spawnTime = Random.Range(3f, 7f);
        }
    }

    void SpawnNextBug()
{
    if (spawnCurrentNum >= currentWave.bugSpawns.Length)
    {
        canGenerate = false;
        return;
    }

    BugSpawnData currentEntry = currentWave.bugSpawns[spawnCurrentNum];
    GameObject bugToSpawn = GetBugPrefab(currentEntry.bugType);
    
    if (bugToSpawn != null)
    {
        GameObject bug = Instantiate(bugToSpawn, spawnPos, Quaternion.identity);
        bug.GetComponent<BugMove>().SetRoute(lineRend);
        bugsSpawnedRN++;
        bugsSpawned++; 
        bugsAlive++; 

        
        if (bugsSpawnedRN >= currentEntry.count)
        {
            bugsSpawnedRN = 0;
            spawnCurrentNum++;
        }
    }
}

    GameObject GetBugPrefab(BugSpawnData.BugType bugType)
    {
        switch (bugType)
        {
            case BugSpawnData.BugType.Fly: 
                return setFly;
            case BugSpawnData.BugType.Gnat: 
                return setGnat;
            case BugSpawnData.BugType.Stinkbug: 
                return setStinkbug;
            case BugSpawnData.BugType.DungBeetle: 
                return setDungBeetle;
            case BugSpawnData.BugType.Cockroach: 
                return setCockroach;
            default: 
                return null;
        }
    }

    public void StartNewWave(WaveManager wave)
    {
        currentWave = wave;
        spawnCurrentNum = 0;
        bugsSpawnedRN = 0;
        canGenerate = true;

        totalBugsToSpawn = 0;
        bugsSpawned = 0;
        bugsAlive = 0;
    
    foreach (BugSpawnData spawnData in wave.bugSpawns)
        {
            totalBugsToSpawn += spawnData.count;
        }

    }

    public void OnBugKilled()
    {
        bugsAlive--;
        deadBug++;
    }


    public bool IsWaveComplete()
    {
        return bugsSpawned >= totalBugsToSpawn && bugsAlive <= 0;
    }
}