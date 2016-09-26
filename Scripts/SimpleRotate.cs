using UnityEngine;
using System.Collections;

public class SimpleRotate : MonoBehaviour {

    public Vector3 axisOfRotation = Vector3.up;
    public float rotationSpeed;

    void Awake () {

    }

    void Update () {
        transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, axisOfRotation);
    }

}
