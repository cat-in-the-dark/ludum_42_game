using UnityEngine;

public class KrotDropper : MonoBehaviour
{
	public GameObject Krot;
	
	// Use this for initialization
	void Start () {
		Invoke("Drop", 4f);
	}

	void Drop()
	{
		var inst = Instantiate(Krot, new Vector3(-1.5f, -1.5f, 0f), Quaternion.identity);
		Destroy(inst, 7f);
	}
}
