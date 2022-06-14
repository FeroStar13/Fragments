using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBox : MonoBehaviour
{

    [SerializeField] float _damage;
    [SerializeField] float _explosionRange;
    [SerializeField] float _timeToExplode;

    [SerializeField] GameObject ExplosionDistance;


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
        ExplosionDistance.SetActive(true);
        yield return new WaitForSeconds(_timeToExplode);
        ExplosionDistance.SetActive(false);

        Collider[] hitColliders = new Collider[100];

        hitColliders = Physics.OverlapSphere(transform.position, _explosionRange);


        for (var i = 0; i < hitColliders.Length; i++)
        {
            Transform tempTarget = hitColliders[i].transform;
            IPlayerDamageable interacted = tempTarget.GetComponent<IPlayerDamageable>();

            if (interacted == null)
            {
            }
            else
            {                         
                interacted.TakeDamage(_damage);
                     
            }

            IDamageable interacted2 = tempTarget.GetComponent<IDamageable>();

            if (interacted2 == null)
            {
            }
            else
            {
                interacted2.TakeDamage(_damage);
               
            }


        }
        Destroy(gameObject);

    }
}
