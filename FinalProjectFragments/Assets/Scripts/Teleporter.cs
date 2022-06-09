using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    [SerializeField] PlayerCharacter player;
    [SerializeField] Vector3 _teleportPositon;
    [SerializeField] GameObject _bossActivation;


    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = _teleportPositon;
        _bossActivation.SetActive(true);
    }
}
