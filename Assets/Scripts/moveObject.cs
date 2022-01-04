using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DisallowMultipleComponent]

public class moveObject : MonoBehaviour
{
    [SerializeField] Vector3 movePosition;
    [SerializeField] float moveSpeed;
    [SerializeField] [Range(0,1)] float moveProgress; // 0-1 , 0 = обьект не двигался
    Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        moveProgress = Mathf.PingPong(moveSpeed * Time.time, 1);
        Vector3 offset = movePosition * moveProgress;
        transform.position = startPosition + offset;
    }

}
