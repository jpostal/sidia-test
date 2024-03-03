using UnityEngine;
using System.Collections;

public class MoveFloor : MonoBehaviour {
	//public GameObject prefab;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		// Move o chão e os obstáculos como se fosse uma esteira.
		transform.position -= new Vector3 (0, 0, 0.1f);
	}
}
