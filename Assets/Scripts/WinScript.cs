using UnityEngine;

public class WinScript : MonoBehaviour
{   
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("WIN");
            // TODO: go to game win
        }
    }
}