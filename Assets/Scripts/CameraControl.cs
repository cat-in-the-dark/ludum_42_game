using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public Transform Player;
    public Transform Camera;

    // Update is called once per frame
    void Update()
    {
        var y = Player.position.y + 1f;
        
        if (y <= 0) y = 0;
        if (y >= 7) y = 7;
        
        Camera.position = Vector3.up * y + Vector3.back * 10;
    }
}