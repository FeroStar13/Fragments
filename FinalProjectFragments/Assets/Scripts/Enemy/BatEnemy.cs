using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BatEnemy : EnemyBrain, IPlayerDamageable
{
    NavMeshAgent _agent;

    [Header("Attack")]
    [SerializeField] bool _canAttack;
    [SerializeField] float _attackRange;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _agent.speed = _movementSpeed;
    }

         protected override void Update()
    {
        EnemyDamage();
        base.Update();
        Move();
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

    public void TakeDamage(float DamageToTake)
    {
        Die();
        _hp = _hp - DamageToTake;
        UpdateEnemyHP(_hp);
    }

    public void Die()
    {
        if (_hp <= 1)
        {
            Destroy(gameObject);
        }
    }
    public void UpdateEnemyHP(float hpDoEnemy)
    {
        enemyHpImage.fillAmount = hpDoEnemy / 30.0f;
    }

    void EnemyDamage()
    {
        if (_canAttack == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _attackRange))
            {
                _canAttack = true;

                _agent.speed = 0;


                IDamageable characterHit = hit.transform.GetComponent<IDamageable>();
                if (characterHit != null)
                {

                    _agent.speed = 6;
                }
            }
            _canAttack = false;
        }

    }
    }


