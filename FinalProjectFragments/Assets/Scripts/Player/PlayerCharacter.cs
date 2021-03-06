using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour, IDamageable, ICollectable, IHealeable
{
    [Header("Movement")]
    [SerializeField] float _runningSpeed;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _movementSpeed;
    [SerializeField] float _rotationMultiplier;
    [SerializeField] float _playerRotation;

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
    [SerializeField] Transform _firePosition;

    [Header("Rool")]
    [SerializeField] float _distanceToRoll;
    [SerializeField] float _timeToRoll;
    [SerializeField] float _timeToRollAgain;
    [SerializeField] float _staminaLostOnRoll;

    [Header("MeleeDamage")]
    [SerializeField] float _meleeDamage;
    [SerializeField] bool _canAttack = true;
    [SerializeField] float _meleeAttackRate;
    [SerializeField] int _meleeAttackRange;
    [SerializeField] GameObject DamageArea;

    [Header("EspecialWeapons")]
    [SerializeField] SpecialWeapons myWeapon;
    [SerializeField] GameObject ExplosiveBoxPrefab;
    [SerializeField] float _ammountOfExplosiveBoxes;
    [SerializeField] Item _boomBoxItem;

    [SerializeField] SuperFireCannon _SuperFirePrefab;
    [SerializeField] float _ammountOfEnergyCannons;
    [SerializeField] Item _energyCannonItem;

    [Header("Inventory")]
    [SerializeField] inventory _inv;

    [SerializeField] float _currentTime;

    enum SpecialWeapons
    {
       ExplosiveBox,
       EnergyCanon
    }

    Animator animatorInChild;
    Animator animator;
    Rigidbody _rigidBody;


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

    public float AmmountOfExplosiveBoxes { get => _ammountOfExplosiveBoxes; set => _ammountOfExplosiveBoxes = value; }
    public float AmmountOfEnergyCannons { get => _ammountOfEnergyCannons; set => _ammountOfEnergyCannons = value; }
    public bool CanFire { get => _canFire; set => _canFire = value; }
    public float Hp
    {
        get => _hp;
        set
        {
            _hp = value;
            _hp = Mathf.Clamp(_hp, 0, 100);
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
        ChangeWeapon();
        ActivateEspecialWeapons();

        _timeToRoll += Time.deltaTime;
        if (_timeToRoll >= _timeToRollAgain)
        {
            Rolling();
        }

    }

    private void FixedUpdate()
    {
        MoveInDirection();
       
    }

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
       // controller = GetComponent<CharacterController>();
        animatorInChild = GetComponentInChildren<Animator>();
        animator = GetComponent<Animator>();
       
      
    }

    private void MoveInDirection()
    {
        FaceDirection();

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0, vertical).normalized;
       

      /*  float targetAngle = Vector3.Angle(transform.forward, direction);
        float cross = Vector3.Cross(transform.forward, direction).y;

        if(cross < 0)
        {
            targetAngle *= -1;
        }

        transform.Rotate(Vector3.up * targetAngle * Time.deltaTime * _rotationMultiplier);*/

        _rigidBody.velocity = direction * _movementSpeed;
        animator.SetFloat("Walk", _rigidBody.velocity.magnitude);
   
    }

    private void FaceDirection()
    {
        Plane playerPlane = new Plane(Vector3.up, transform.position);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitdist;

        if(playerPlane.Raycast(ray, out hitdist))
        {
            Vector3 targetpoint = ray.GetPoint(hitdist);
            Quaternion targetrotation = Quaternion.LookRotation(targetpoint - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetrotation, _playerRotation * Time.deltaTime);
        }
    }

    void Running()
    {
        if(PlayerStamina <= 5 || !(Input.GetKey(KeyCode.LeftShift)))
        {
            animator.SetBool("IsRunning", false);
        }
        if (Input.GetKey(KeyCode.LeftShift) && PlayerStamina > _playerStaminaThreshold)
        {
            animator.SetBool("IsRunning", true);
            if (_rigidBody.velocity.magnitude != 0)
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
        if (Input.GetKeyDown(KeyCode.Mouse0) && CanFire == true)
        {

           

            //se nao tiver carga nao dispara

            if (PlayerFireCharge <= 0)
            {
                CanFire = false;
            }

            //se tiver carga despara
            else
            {
                //Intantiate do fire and amimation

                StartCoroutine(AttackAnimation());
               
                _currentTime = 0; //resetar o tempo para atacar

                //Reduzir a quantidade de desparos

                PlayerFireCharge--;
                UIManager.instance.UpdateCharge(_playerFireCharge);
              

            }
        }

        IEnumerator AttackAnimation()
        {
            CanFire = false;
            _canAttack = false;
            animator.SetTrigger("FireAttack");
            yield return new WaitForSeconds(0.40f);
            PlayerFire newBullet = Instantiate(_bulletPrefab, _firePosition.transform.position, transform.rotation);
            PlayerFire fire = newBullet.GetComponent<PlayerFire>();
            fire.FireMove(transform.forward);
            _canAttack = true;
            CanFire = true;
        }
     
    }
    void MeleeAttack()
    {


        if (Input.GetKeyDown(KeyCode.Mouse1) && _canAttack == true)
        {
            animator.SetTrigger("MeleeAttack");

            StartCoroutine(TimeThatStopFire());


            //Attaque melee action
         

            _currentTime = 0; //resetar o tempo para atacar
        }
        IEnumerator TimeThatStopFire() //currotina que so permite usar uma arma de cada vez
        {
            
            CanFire = false;
            DamageArea.SetActive(true);
            animatorInChild.SetTrigger("Attack");
            yield return new WaitForSeconds(_meleeAttackRate);
            DamageArea.SetActive(false);
            CanFire = true;
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
        }
    }

    public void TakeDamage(float DamageToTake)
    {
        Die();
        Hp = Hp - DamageToTake;
        UIManager.instance.UpdateHp(Hp);
    }

    public void Die()
    {
        if(Hp <= 0)
        {
            UIManager.instance.Death();
            gameObject.SetActive(false);
        }
    }

    void Rolling()
    {
       /* 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("Roll");
            _rigidBody.AddForce(transform.forward * _distanceToRoll, ForceMode.Impulse);           
            _playerStamina -= _staminaLostOnRoll;
            _timeToRoll = 0;
        }
        */
    }

    private void InteractCast()
    {
        if (Input.GetKeyDown(KeyCode.E))
            
            {
           // RaycastHit hit;
           // if (Physics.Raycast(transform.position, transform.forward, out hit, 5))
            {
                Collider[] hitColliders = new Collider[100];

                hitColliders = Physics.OverlapSphere(transform.position, _meleeAttackRange);

                for (var i = 0; i < hitColliders.Length; i++)
                {
                    Transform tempTarget = hitColliders[i].transform;
                    IInteractable interacted = tempTarget.transform.GetComponent<IInteractable>();

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
    void ChangeWeapon()
    {
        if(Input.GetKeyDown(KeyCode.LeftControl))
       
            myWeapon++;
        
        if (myWeapon == SpecialWeapons.ExplosiveBox)
        {
            UIManager.instance.isExplosiveBox();
        }
        if (myWeapon == SpecialWeapons.EnergyCanon)
        {
            UIManager.instance.isSuperCannon();
        }

        if((int)myWeapon >= 2)
        {
            myWeapon = 0;
        }
    }

    void ActivateEspecialWeapons()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            switch (myWeapon)
            {
                case SpecialWeapons.ExplosiveBox:
             
                    SpawnExplosiveBox();
                    break;

                case SpecialWeapons.EnergyCanon:
                   
                    ActivateSpecialCannon();
                    break;
            }
        }        
    }

    void SpawnExplosiveBox()
    {        
        if(AmmountOfExplosiveBoxes > 0)
        {
            GameObject boxToSpawn = Instantiate(ExplosiveBoxPrefab, transform.position, transform.localRotation);
            ExplosiveBox box = boxToSpawn.GetComponent<ExplosiveBox>();
            AmmountOfExplosiveBoxes--;
            _inv.RemoveItem(_boomBoxItem);
        }
                
    }

    void ActivateSpecialCannon()
    {
        if (AmmountOfEnergyCannons > 0)
        {
            StartCoroutine(SuperCannonAnimationAndFire());
        }
    }

    IEnumerator SuperCannonAnimationAndFire()
    {
            animator.SetTrigger("FireAttack");
        yield return new WaitForSeconds(0.40f);
        SuperFireCannon newBullet = Instantiate(_SuperFirePrefab, _firePosition.transform.position, transform.rotation);
            SuperFireCannon fire = newBullet.GetComponent<SuperFireCannon>();
            fire.FireMove(transform.forward);
            AmmountOfEnergyCannons--;
            _inv.RemoveItem(_energyCannonItem);
        
    }

    public void Collected(Item intendedItem)
    {
        _inv.AddItem(intendedItem);
    }

    public void Healed(float ammountOfHeal)
    {

        Hp += ammountOfHeal;

        UIManager.instance.UpdateHp(Hp);
            }
}
   
