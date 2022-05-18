using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperFireCannon : MonoBehaviour
{
 
    Rigidbody _rigidbody;
    [SerializeField] float _fireSpeed;
    [SerializeField] float _fireRange;
    [SerializeField] float _damage;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void FireMove(Vector3 direction)
    {
        _rigidbody.AddForce(direction * _fireSpeed, ForceMode.Impulse);
        Destroy(gameObject, _fireRange);

    }

    private void OnTriggerEnter(Collider other)
    {
        
        IPlayerDamageable damage = other.transform.GetComponent<IPlayerDamageable>();

        if (damage == null)
        {

        }
        else
        {

            damage.TakeDamage(_damage);
           
        }

    }

}

