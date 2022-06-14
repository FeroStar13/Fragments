using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{

    [SerializeField] PlayerCharacter player;
    [SerializeField] Vector3 _teleportPositon;
    [SerializeField] GameObject _bossActivation;

    [SerializeField] GameObject LoadingCanvas;
    [SerializeField] float _timeLoading;


    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = _teleportPositon;
        StartCoroutine(LoadingTime());
        _bossActivation.SetActive(true);
        GameManager.instance.IsOnBossStage = true;
    
    }

  

    IEnumerator LoadingTime()
    {
        LoadingCanvas.SetActive(true);
        yield return new WaitForSeconds(_timeLoading);
        LoadingCanvas.SetActive(false);
    }
}
