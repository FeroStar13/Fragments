using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : EnemyBrain, IPlayerDamageable
{
    [Header("RockAttack")]
    [SerializeField] bool _canAttack = false;
    [SerializeField] float _rockAttackRange;
    [SerializeField] float _fireAgainTimer;
    [SerializeField] float _fireCooldown;
    [SerializeField] GameObject _rockPrefab; 
    [SerializeField] float _rangeToFindPlayer;
    [SerializeField] float _AmmoutOfRockThrowed; //um numero que aumenta cada vez que o boss atira uma pedra, quando o numero é x o proximo ataque será outro

    [Header("LazerAttack")]
    [SerializeField] GameObject _LazerPrefab;
    [SerializeField] bool _changeAttack = false;
    [SerializeField] float _lazerBeamAmmount; //quantidade de vezes em que o ataque de lazer atira 
    [SerializeField] float _timesToFireTillChangeAttack; //quantidade de vezes em que o boss usa o ataque normal até mudar para o ataque especial

    [Header("AttackFromGround")]
    [SerializeField] Transform _position1;
    [SerializeField] Transform _position2;
    [SerializeField] GameObject _spikePrefab;


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
       
      StartCoroutine(Attack());
       
    }
    IEnumerator Attack()
    {
        if (_canAttack == false)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _rangeToFindPlayer))
            {
                _canAttack = true;

                IDamageable characterHit = hit.transform.GetComponent<IDamageable>();
                if (characterHit != null)
                {
                   
                    CreateRock();
                    EnergyBeam();
                    yield return new WaitForSeconds(1);
                   
                }
            }
            _canAttack = false;
        }
    }

    void CreateRock()
    {

        if (_fireAgainTimer >= _fireCooldown && _canAttack == true && _changeAttack == false)
        {
            _AmmoutOfRockThrowed++;
           GameObject bullet = Instantiate(_rockPrefab, transform.position, transform.localRotation);
            BossFire1 fire = bullet.GetComponent<BossFire1>();
            fire.FireMove(target.position - bullet.transform.position);
            Destroy(bullet, 4f);
            _fireAgainTimer = 0;
            _canAttack = false;

        }
    }

    void EnergyBeam()
    {
        if(_AmmoutOfRockThrowed >= _timesToFireTillChangeAttack)
        {
            _changeAttack = true;
        }
        if(_fireAgainTimer >= _fireCooldown && _canAttack == true && _changeAttack == true)
        {

            StartCoroutine(EnergyBeamLazer());
           
        }
    }

    IEnumerator EnergyBeamLazer()
    {
        for (int i = 0; i < _lazerBeamAmmount; i++)
        {
            GameObject bullet = Instantiate(_LazerPrefab, transform.position, transform.localRotation);
            BossFire1 fire = bullet.GetComponent<BossFire1>();
            fire.FireMove(target.position - bullet.transform.position);
            Destroy(bullet, 4f);
            _fireAgainTimer = 0;
            _canAttack = false;
            _AmmoutOfRockThrowed = 0;
            
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1);

        Instantiate(_spikePrefab, _position1.position, _position1.rotation);
        Instantiate(_spikePrefab, _position2.position, _position1.rotation);
        _changeAttack = false;
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
        UIManager.instance.UpdateBossHp(_hp);
        _hp = _hp - DamageToTake;
        Die();
    }
}
