using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBox : MonoBehaviour
{

    [SerializeField] float _damage;
    [SerializeField] float _explosionRange;
    [SerializeField] float _timeToExplode;

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

           

            IPlayerDamageable interacted = tempTarget.GetComponent<IPlayerDamageable>();

            if (interacted == null)
            {
            }
            else
            {
                yield return new WaitForSeconds(_timeToExplode);              
                interacted.TakeDamage(_damage);
                Destroy(gameObject);
                Debug.Log("boom");
            }

            IDamageable interacted2 = tempTarget.GetComponent<IDamageable>();

            if (interacted2 == null)
            {
            }
            else
            {

                yield return new WaitForSeconds(_timeToExplode);             
                interacted2.TakeDamage(_damage);
                Destroy(gameObject);
                Debug.Log("boom2");
            }
        }

       
    }
}
