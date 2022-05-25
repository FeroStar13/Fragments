using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreHp : MonoBehaviour
{
    [SerializeField] float _healToIncrese;
    private void OnCollisionEnter(Collision collision)
    {
        IHealeable item = collision.transform.GetComponent<IHealeable>();
        if (item == null)
        {

        }
        else
        {
            item.Healed(_healToIncrese);
            Destroy(gameObject);
        }
    }
}
