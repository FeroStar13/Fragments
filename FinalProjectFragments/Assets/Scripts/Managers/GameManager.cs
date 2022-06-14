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

    [SerializeField] GameObject _BossRoomTeleporter;

    [Header("Respawn")]
    [SerializeField] Vector3 _playerSpawn;
    [SerializeField] Vector3 _bossPlayerSpawn;

    [Header("Reference")]
    [SerializeField] PlayerCharacter player;

    [Header("DeathMensage")]
    [SerializeField] GameObject DeathUI;

    [Header("Boss")]
    [SerializeField] GameObject _bossActivation;
    bool _isOnBossStage = false;


    public static GameManager instance;

    public bool IsOnBossStage { get => _isOnBossStage; set => _isOnBossStage = value; }

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
            _BossRoomTeleporter.SetActive(true);
        }
      
    }

    public void Respawn()
    {
        if (IsOnBossStage == true)
        {
            _playerSpawn = _bossPlayerSpawn;
        }
        DeathUI.SetActive(false);
        player.transform.position = _playerSpawn;
        player.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        player.Hp = 100;
        UIManager.instance.UpdateHp(player.Hp);
        
    }
}
