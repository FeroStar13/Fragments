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
    [SerializeField] float _fireAgainTimer;
    [SerializeField] float _fireCooldown;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform _firePosition;

    [Header("DropItem")]
    [SerializeField] GameObject _healingPrefab;
    [SerializeField] float _itemDropChance;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();

        _itemDropChance = Random.Range(0, 20);

        _agent.speed = _movementSpeed;
    }


    protected override void Update()
    {
        _fireAgainTimer += Time.deltaTime;


        base.Update();
        Move();
        EnemyDamage();
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

        _hp = _hp - DamageToTake;
        Die();
        UpdateEnemyHP(_hp);
    }

    public void Die()
    {
        GameManager.instance.BatDied(this);
        if (_hp <= 1)
        {
            ItemDrop();
            Destroy(gameObject);
        }
    }
    public void UpdateEnemyHP(float hpDoEnemy)
    {
        enemyHpImage.fillAmount = hpDoEnemy / 30.0f;
    }

    void EnemyDamage()
    {
        StartCoroutine(Attack());

    }

    IEnumerator Attack()
    {
        if (_canAttack == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _attackRange))
            {
                _canAttack = true;




                IDamageable characterHit = hit.transform.GetComponent<IDamageable>();
                if (characterHit != null)
                {
                    _agent.speed = 0;
                    CreateBullet();
                    yield return new WaitForSeconds(1);
                    _agent.speed = _baseMovementSpeed;
                }
            }
            _canAttack = false;
        }
    }
    void CreateBullet()
    {
        if (_fireAgainTimer >= _fireCooldown && _canAttack == true)
        {
            GameObject bullet = Instantiate(bulletPrefab, _firePosition.transform.position, transform.localRotation);
            EnemyFire fire = bullet.GetComponent<EnemyFire>();
            fire.FireMove(target.position - bullet.transform.position);
            Destroy(bullet, 4f);
            _fireAgainTimer = 0;
            _canAttack = false;
        }
    }
    void ItemDrop()
    {

        if (_itemDropChance > 10 && _itemDropChance < 20)
        {
            Instantiate(_healingPrefab, transform.position, transform.rotation);
        }
    }
}


