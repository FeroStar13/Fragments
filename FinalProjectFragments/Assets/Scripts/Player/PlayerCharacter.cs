using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    [SerializeField] float _runningSpeed;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _movementSpeed;
    float _turnSmoothVelocity;
    [SerializeField] float _turnSmoothTime;

    [Header("Stamina")]
    [SerializeField] float _playerStamina = 100f; 
    [SerializeField] float _maxStamina = 100f;
    [SerializeField] float _playerStaminaThreshold;
    [SerializeField] float _playerStaminaLostMultiplier; //quatidade de stamida que o player perde ao correr
    [SerializeField] float _playerStaminaGainMultiplier; //quantidade de stamina que o player ganha ao andar/ficar parado

   [Header("Life")]
   [SerializeField] float _hp;

    [Header("Fire")]
    [SerializeField] PlayerFire _bulletPrefab;
    [SerializeField] int _playerFireCharge;
    [SerializeField] float _fireRate;
    [SerializeField] bool _canFire = true;

    [Header("MeleeDamage")]
    [SerializeField] float _meleeDamage;
    [SerializeField] bool _canAttack = true;
    [SerializeField] float _meleeAttackRate;
    [SerializeField] int _meleeAttackRange;

    [SerializeField] float _currentTime;


    CharacterController controller;
    Animator animator;
   // Rigidbody _rigidBody;

    public float PlayerStamina 
    { 
        get => _playerStamina;
        set
        {
            _playerStamina = value;
            _playerStamina = Mathf.Clamp(_playerStamina, 0, _maxStamina);
        }
        }

    public int PlayerFireCharge 
    { 
        get => _playerFireCharge;
        set
        {
            _playerFireCharge = value;
            _playerFireCharge = Mathf.Clamp(_playerFireCharge, 0, 100);
        }
        }

    private void Update()
    {
        _currentTime += Time.deltaTime;
        Running();

        if (_currentTime >= _meleeAttackRate)
        {
            MeleeAttack();
        }
       

     
        if (_currentTime >= _fireRate)
        {
            Fire();
        }
        InteractCast();
    }

    private void FixedUpdate()
    {
        MoveInDirection();
       
    }

    private void Awake()
    {
       // _rigidBody = GetComponent<Rigidbody>();
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
      
    }

    private void MoveInDirection()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;


       // _rigidBody.velocity = direction * _movementSpeed * Time.deltaTime;


        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0, angle, 0); //smooth rotation

             controller.Move(direction * _movementSpeed * Time.deltaTime);// movimentação

           


        }

    }
    void Running()
    {
        if (Input.GetKey(KeyCode.LeftShift) && PlayerStamina > _playerStaminaThreshold)
        {
            if (controller.velocity.magnitude != 0)
            {
                _movementSpeed = _runningSpeed;
                _playerStamina -= Time.deltaTime * _playerStaminaLostMultiplier; //perca de stamina
                UIManager.instance.UpdateStamina(_playerStamina);

            }
            else
            {
                PlayerStamina += Time.deltaTime * _playerStaminaGainMultiplier; //ganho de stamina 
                UIManager.instance.UpdateStamina(_playerStamina);
            }
        }
        else
        {
            _movementSpeed = _baseSpeed;
            PlayerStamina += Time.deltaTime * _playerStaminaGainMultiplier;
            UIManager.instance.UpdateStamina(_playerStamina);
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _movementSpeed = _baseSpeed;
        }
    }

    void Fire()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && _canFire == true)
        {

            StartCoroutine(TimeThatStopAttack());

            //se nao tiver carga nao dispara

            if (PlayerFireCharge <= 0)
            {
                _canFire = false;
            }

            //se tiver carga despara
            else
            {
                //Intantiate do fire

                PlayerFire newBullet = Instantiate(_bulletPrefab, transform.position, transform.rotation);
                PlayerFire fire = newBullet.GetComponent<PlayerFire>();
                fire.FireMove(transform.forward);

                _currentTime = 0; //resetar o tempo para atacar

                //Reduzir a quantidade de desparos

                PlayerFireCharge--;

            }
        }
        IEnumerator TimeThatStopAttack() //currotina que so permite usar uma arma de cada vez
            {
            _canAttack = false;
            yield return new WaitForSeconds(_fireRate);
            _canAttack = true;
        }
    }
    void MeleeAttack()
    {


        if (Input.GetKeyDown(KeyCode.Mouse1) && _canAttack == true)
        {
          
            StartCoroutine(TimeThatStopFire());


            //Attaque melee action
            Collider[] hitColliders = new Collider[100];

            hitColliders = Physics.OverlapSphere(transform.position, _meleeAttackRange);

            for (var i = 0; i < hitColliders.Length; i++)
            {
                Transform tempTarget = hitColliders[i].transform;

                IPlayerDamageable interacted = tempTarget.GetComponent<IPlayerDamageable>();


                if (interacted == null)
                {

                }
                else
                {
                    interacted.TakeDamage(_meleeDamage);


                }
            }

            _currentTime = 0; //resetar o tempo para atacar
        }
        IEnumerator TimeThatStopFire() //currotina que so permite usar uma arma de cada vez
        {
            
            _canFire = false;
            animator.SetTrigger("Attack");
            yield return new WaitForSeconds(_meleeAttackRate);
            _canFire = true;
            _movementSpeed = _baseSpeed;
        }
    }

    public void TakeDamage(float DamageToTake)
    {
        Die();
        _hp = _hp - DamageToTake;
        UIManager.instance.UpdateHp(_hp);
    }

    public void Die()
    {
        
    }

    void Rolling()
    {
      //  controller.AddForce(direction * _fireSpeed, ForceMode.Impulse);
    }
    private void InteractCast()
    {
        if (Input.GetKeyDown(KeyCode.E))
            
            {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
            {
                IInteractable interacted = hit.transform.GetComponent<IInteractable>();

                if (interacted == null)
                {
                }
                else
                {
                    interacted.Interact(this);

                }

            }
        }
     
    }
}
   
