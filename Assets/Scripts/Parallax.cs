using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {


	// parallaxSpeed deve ser valor entre 0 e 1
	// 0 -> Sem parallax
	// 1 -> Cenário imóvel em relação a camera (se move junto com a camera)
	[Range(0f,1f)]
	public float parallaxSpeed = 0f;

	private Transform cameraTransform;

	private float Xant; // x da camera no frame anterior


	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
		Xant = cameraTransform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		float deltaCamera = cameraTransform.position.x - Xant;

		if (true){//deltaCamera > 0) {
			Vector3 newPos = transform.position;
			newPos.x += parallaxSpeed * deltaCamera;
			transform.position = newPos;
		}

		Xant = cameraTransform.position.x;
	}
}
