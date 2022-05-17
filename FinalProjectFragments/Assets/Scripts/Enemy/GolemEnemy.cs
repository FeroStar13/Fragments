using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class GolemEnemy : EnemyBrain, IPlayerDamageable
{
  
    [Header("Attack")]
    bool _canAttack = false;
    [SerializeField] float _attackRange;
    [SerializeField] float _damage;
    [SerializeField] float _timeToHit; //quantidade de vezes que o inimigo ataca enquanto esta parado
    [SerializeField] float _attackPerStopWalk; //quantidade de tempo que dura o ataque

 NavMeshAgent _agent;

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
    public void Die()
    {
        if(_hp <= 1)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float DamageToTake)
    {
        Die();
        _hp = _hp - DamageToTake;
        UpdateEnemyHP(_hp);
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

    void EnemyDamage()
    {
        if (_canAttack == false)
        {
            StartCoroutine(AttackPerSecond());

        }
    }
    IEnumerator AttackPerSecond()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _attackRange))
        {
            _canAttack = true;
          
                _agent.speed = 0;
             

            IDamageable characterHit = hit.transform.GetComponent<IDamageable>();
            if (characterHit != null)
            {
               
                for (int i = 0; i < _timeToHit; i++)
                {
                    
                    characterHit.TakeDamage(_damage);
                   
                    yield return new WaitForSeconds(_attackPerStopWalk);
                }             
                _agent.speed = _baseMovementSpeed;
            }
        }
        _canAttack = false;
    }
    public void UpdateEnemyHP(float hpDoEnemy)
    {
        enemyHpImage.fillAmount = hpDoEnemy / 100.0f;
    }
}
