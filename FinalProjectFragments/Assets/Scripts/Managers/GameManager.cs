using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] Transform[] spawnPoints;

    List<GolemEnemy> GolemAlive = new List<GolemEnemy>();
    List<BatEnemy> BatAlive = new List<BatEnemy>();

    [SerializeField] GolemEnemy _golemPrefab;
    [SerializeField] BatEnemy _batPrefab;

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
            foreach (var spawn in spawnPoints)
            {
                GolemEnemy newGolem = Instantiate(_golemPrefab, spawn.position, spawn.rotation);
                GolemAlive.Add(newGolem);
            }
        }
    }

    public void GolemDied(GolemEnemy golemThatDied)
    {
        GolemAlive.Remove(golemThatDied);
        if (GolemAlive.Count <= 0)
        {

            GolemSpawn();
        }
    }

    void BatSpawn()
    {
        for (int i = 0; i < 1; i++)
        {
            foreach (var spawn in spawnPoints)
            {
                BatEnemy newBat = Instantiate(_batPrefab, spawn.position, spawn.rotation);
                BatAlive.Add(newBat);
            }
        }
    }
    public void BatDied(BatEnemy batThatDied)
    {
        BatAlive.Remove(batThatDied);
        if (BatAlive.Count <= 0)
        {

            BatSpawn();
        }

    }
}
