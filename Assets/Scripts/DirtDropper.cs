using UnityEngine;

public class DirtDropper : MonoBehaviour
{
    public GameObject dirt;
    public float DropCooldown = 1f;

    // Use this for initialization
    void Start()
    {
        Invoke("Drop", RandomTime());
    }

    void Drop()
    {
        int column = Random.Range(0, 4);
        float posX = column - 1.5f;
        float posY = 6.5f;
        var inst = Instantiate(dirt, new Vector3(posX, posY, 0f), Quaternion.identity);
        
        Invoke("Drop", RandomTime());
    }

    float RandomTime()
    {
        return Random.Range(DropCooldown - 0.2f, DropCooldown + 0.2f);
    }
}