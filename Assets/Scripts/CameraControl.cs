using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform Player;
    public Transform Camera;
    
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        var y = Player.position.y + 1f;
        
        if (y <= 0) y = 0;
        if (y >= 7) y = 7;
        
        Debug.Log(y);
        
        Camera.position = Vector3.up * y + Vector3.back * 10;
    }
}