using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : EnemyBrain, IPlayerDamageable
{

    [SerializeField] bool _canAttack = false;
    [SerializeField] float _rockAttackRange;
    [SerializeField] float _fireAgainTimer;
    [SerializeField] float _fireCooldown;
    [SerializeField]  GameObject _rockPrefab;
    [SerializeField] float _rangeToFindPlayer;

    NavMeshAgent _agent;

    protected override void Update()
    {
        _fireAgainTimer += Time.deltaTime;
        RockThrow();
        base.Update();
        Move();
       
    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.speed = _movementSpeed;


    }
    void Move()
    {
        if (target != null)
        {

            Vector3 position = new Vector3(target.position.x, transform.position.y, target.position.z);
            transform.LookAt(position);
            _agent.SetDestination(position);
        }
    }
  
    void RockThrow()
    {
       
       // CreateRock();
       
    }
   

    void CreateRock()
    {
        Debug.Log("oi");
        if (_fireAgainTimer >= _fireCooldown && _canAttack == true)
        {
            GameObject bullet = Instantiate(_rockPrefab, transform.position, transform.localRotation);
            EnemyFire fire = bullet.GetComponent<EnemyFire>();
            fire.FireMove(target.position - bullet.transform.position);
            Destroy(bullet, 4f);
            _fireAgainTimer = 0;
            _canAttack = false;

        }
    }
    public void Die()
    {
        if (_hp <= 1)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float DamageToTake)
    {
        _hp = _hp - DamageToTake;
        Die();
    }
}
