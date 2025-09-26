using UnityEngine;
using System.Collections;
public class BackgroundRepeater : MonoBehaviour {

	public float offset = 19f;

	private Transform cameraTransform;

	void Start() {
		cameraTransform = Camera.main.transform;
	}

	void Update() {
		if( ( transform.position.x + offset ) < cameraTransform.position.x ) {
			Vector3 newPos = transform.position;
			newPos.x += 2*offset;
			transform.position = newPos;
		}
	}
}