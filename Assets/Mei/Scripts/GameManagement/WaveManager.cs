
using UnityEngine;
using System;

[Serializable]
public class BugSpawnData
{
    public enum BugType { Fly, Gnat, Stinkbug, DungBeetle, Cockroach }
    
    public BugType bugType;
    public int count;
    public int spawnInterval; // delay before the next bug is spawned
}

[Serializable]
public class WaveManager
{
    //public int waveNumber;
    public BugSpawnData[] bugSpawns;
    public int reward;
}