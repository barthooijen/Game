using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamera : MonoBehaviour {

	public Camera Cam;

	void Start () 
	{
		Cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.LookAt (transform.position + Cam.transform.rotation * Vector3.forward, Cam.transform.rotation * Vector3.up);
	}
}
