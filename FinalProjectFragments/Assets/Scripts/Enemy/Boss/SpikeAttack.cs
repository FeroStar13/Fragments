using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeAttack : MonoBehaviour
{
    [SerializeField] float _damage;
    [SerializeField] float _explosionRange;
    [SerializeField] float _timesToHit;
    [SerializeField] float _delayBetweenAttacks;

    private void Start()
    {
        Explosion();
    }
    void Explosion()
    {

        StartCoroutine(ExplosionTimer());

    }

    IEnumerator ExplosionTimer()
    {
      
        Collider[] hitColliders = new Collider[100];

        hitColliders = Physics.OverlapSphere(transform.position, _explosionRange);


        for (var i = 0; i < hitColliders.Length; i++)
        {
            Transform tempTarget = hitColliders[i].transform;
           
            IDamageable interacted2 = tempTarget.GetComponent<IDamageable>();

            if (interacted2 == null)
            {
            }
            else
            {

                for (int e = 0; e < _timesToHit; e++)
                {
                    interacted2.TakeDamage(_damage);
                    yield return new WaitForSeconds(_delayBetweenAttacks);
                }

            }
        }
        Destroy(gameObject);
    }

 
}

