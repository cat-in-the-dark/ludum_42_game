using System;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class CameraEnter : MonoBehaviour
{
    public Transform Camera;
    public Transform FinalCameraPos;

    private float moveSpeed = 0f;

    // Use this for initialization
    void Start()
    {
        Camera.position = Vector3.back * 10 + Vector3.up * 7;
        Invoke("SetMoveSpeed", 1.5f);
    }

    void SetMoveSpeed()
    {
        moveSpeed = -1.3f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.position.y >= FinalCameraPos.position.y)
        {
            Camera.Translate(0f, moveSpeed * Time.deltaTime, 0f);
        }
        else
        {
            Camera.position = FinalCameraPos.position;
            enabled = false;
            GetComponent<CameraControl>().enabled = true;
            GetComponent<DirtDropper>().enabled = true;
            foreach (var brickDroper in GetComponents<BrickDroper>())
            {
                brickDroper.enabled = true;
            }
        }
    }
}