using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {
    public float speed = 0.1f;
    private Vector3 startLocation;
    private Transform endLocation;
    // Start is called before the first frame update
    void Start() {
        /*endLocation = new GameObject().transform;
        endLocation.position = transform.position;
        endLocation.transform.Translate(0, 0, -12, Space.Self);*/
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.transform.Translate(0, 0, -speed, Space.Self);
        if (false /* i dunno how to check if they got too far.... */) {
            Destroy(gameObject);
        }
    }
}
