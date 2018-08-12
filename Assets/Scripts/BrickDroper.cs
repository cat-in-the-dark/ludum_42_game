using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickDroper : MonoBehaviour
{
    public GameObject Brick;
    public float DropCooldown;


    // Use this for initialization
    void Start()
    {
        Invoke("Drop", DropCooldown);
    }

    void Drop()
    {
        int column = Random.Range(0, 4);
        float posX = column - 1.5f;
        float posY = 6.5f;

        var inst = Instantiate(Brick, new Vector3(posX, posY, -1), Quaternion.identity);
        
        Invoke("Drop", RandomTime());
    }

    float RandomTime()
    {
        return Random.Range(DropCooldown - 0.3f, DropCooldown + 0.3f);
    }
}