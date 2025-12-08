
using UnityEngine;
using System;

[Serializable]
public class BugSpawnData
{
    public enum BugType { Fly, Gnat, Stinkbug, DungBeetle, Cockroach }
    
    public BugType bugType;
    public int count;
}

[Serializable]
public class WaveManager
{
    public int waveNumber;
    public BugSpawnData[] bugSpawns; 
}