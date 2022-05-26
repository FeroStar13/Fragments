using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform[] golemSpawnPoints;
    [SerializeField] Transform[] batSpawnPoints;

    List<GolemEnemy> GolemAlive = new List<GolemEnemy>();
    List<BatEnemy> BatAlive = new List<BatEnemy>();

    [SerializeField] GolemEnemy _golemPrefab;
    [SerializeField] BatEnemy _batPrefab;

    [SerializeField] GameObject BossActivation;

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    

    private void Start()
    {
        GolemSpawn();
        BatSpawn();
     
    }
    void GolemSpawn()
    {
        for (int i = 0; i < 1; i++)
        {
            foreach (var spawn in golemSpawnPoints)
            {
                GolemEnemy newGolem = Instantiate(_golemPrefab, spawn.position, spawn.rotation);
                GolemAlive.Add(newGolem);
            }
        }
    }

    public void GolemDied(GolemEnemy golemThatDied)
    {
        GolemAlive.Remove(golemThatDied);
        BossSpawn();
    }

    void BatSpawn()
    {
        for (int i = 0; i < 1; i++)
        {
            foreach (var spawn in batSpawnPoints)
            {
                BatEnemy newBat = Instantiate(_batPrefab, spawn.position, spawn.rotation);
                BatAlive.Add(newBat);
            }
        }
    }
    public void BatDied(BatEnemy batThatDied)
    {
        BatAlive.Remove(batThatDied);
        BossSpawn();

    }

    void BossSpawn()
    {
        if(BatAlive.Count <= 0 && GolemAlive.Count <= 0)
        {
            BossActivation.SetActive(true);
        }
      
    }
}
