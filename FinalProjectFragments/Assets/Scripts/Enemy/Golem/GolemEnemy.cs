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

    [Header("DropItem")]
    [SerializeField] GameObject _crystalDropPrefab;
    [SerializeField] GameObject _ironDropPrefab;
    [SerializeField] float _itemDropChance;
  

    NavMeshAgent _agent;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _itemDropChance = Random.Range(0, 20);

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
        GameManager.instance.GolemDied(this);
        if (_hp <= 1)
        {
            ItemDrop();
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float DamageToTake)
    {
      
        _hp = _hp - DamageToTake;
        Die();
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
          
                
             

            IDamageable characterHit = hit.transform.GetComponent<IDamageable>();
            if (characterHit != null)
            {
                _agent.speed = 0;
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

    void ItemDrop()
    {
        

        if(_itemDropChance > 6 && _itemDropChance < 15)
        {
            Instantiate(_crystalDropPrefab, transform.position, transform.rotation);
        }
        if(_itemDropChance > 15 && _itemDropChance < 20)
        {
            Instantiate(_ironDropPrefab, transform.position, transform.rotation);
        }
    }
}