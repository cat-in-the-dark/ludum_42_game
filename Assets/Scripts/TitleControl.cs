using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleControl : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Submit"))
        {
            SceneManager.LoadScene(1);
        }
    }
}