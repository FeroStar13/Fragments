using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    [SerializeField] PlayerCharacter player;
    [SerializeField] Vector3 _teleportPositon;

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = _teleportPositon;
    }
}
