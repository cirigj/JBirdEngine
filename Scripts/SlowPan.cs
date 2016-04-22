using UnityEngine;
using System.Collections;

public class SlowPan : MonoBehaviour {

	public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += new Vector3(speed, 0f, 0f) * Time.deltaTime;
	}
}
