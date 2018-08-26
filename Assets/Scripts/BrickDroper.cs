using UnityEngine;

public class BrickDroper : MonoBehaviour
{
    public GameObject Brick;
    public float DropCooldown;
    public AudioClip OnThrowSound;
    
    private AudioSource audioSource;


    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("Drop", DropCooldown);
    }

    void Drop()
    {
        int column = Random.Range(0, 4);
        float posX = column - 1.5f;
        float posY = 6.5f;

        Instantiate(Brick, new Vector3(posX, posY, -1), Quaternion.identity);
        
        audioSource.PlayOneShot(OnThrowSound);

        Invoke("Drop", RandomTime());
    }

    float RandomTime()
    {
        return Random.Range(DropCooldown - 0.3f, DropCooldown + 0.3f);
    }
}