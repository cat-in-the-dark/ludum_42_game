using UnityEngine;

public class WormDropper : MonoBehaviour
{
    public GameObject worm;
    public float DropCooldown = 5f;

    private void Start()
    {
        Invoke("Drop", RandomTime());
    }

    void Drop()
    {
        float posX = RandomX();
        float posY = RandomY();
        var inst = Instantiate(worm, new Vector3(posX, posY, 0f), Quaternion.identity);
        inst.GetComponent<SpriteRenderer>().flipX = (posX < 0);

        Invoke("Drop", RandomTime());
    }

    float RandomTime()
    {
        return Random.Range(DropCooldown - 1f, DropCooldown + 1f);
    }

    float RandomY()
    {
        return Random.Range(-2, 6) + 0.5f;
    }

    float RandomX()
    {
        int column = Random.Range(0, 2);
        if (column == 0)
        {
            return 1.5f;
        }
        else
        {
            return -1.5f;
        }
    }
}