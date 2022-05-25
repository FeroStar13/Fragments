using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRock : MonoBehaviour
{
    Rigidbody _rigidBody;

    [SerializeField] float _movementSpeed;
    [SerializeField] float _damage;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void FireMove(Vector3 direction)
    {
        direction.Normalize();
        _rigidBody.AddForce(direction * _movementSpeed, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        IDamageable damage = other.transform.GetComponent<IDamageable>();

        if (damage == null)
        {

        }
        else
        {

            damage.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }

}
