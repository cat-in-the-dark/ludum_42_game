﻿using UnityEngine;

public class WormControl : MonoBehaviour
{
	public float DeleteAfter = 3f;
	private Animator animator;
	
	// Use this for initialization
	void Start ()
	{
		animator = GetComponent<Animator>();
		Invoke("PrepareDestroy", DeleteAfter);
	}

	void PrepareDestroy()
	{
		animator.SetTrigger("Destroy");
		Destroy(gameObject, 2f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
