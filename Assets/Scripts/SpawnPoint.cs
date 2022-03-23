using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SpawnPoint : NetworkBehaviour
{
    public int teamId = 0; // 0 is for everyone   


    public static List<SpawnPoint> GetAllSpawnPoints()
    {
        List<SpawnPoint> spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>().ToList();        
        return spawnPoints;
    }

    public static List<SpawnPoint> GetSpawnPointsByTeamId(int id)
    {
        List<SpawnPoint> spawnPoints = GameObject.FindObjectsOfType<SpawnPoint>()
            .Where(sp => sp.teamId == id)
            .ToList();        
        return spawnPoints;
    }
}