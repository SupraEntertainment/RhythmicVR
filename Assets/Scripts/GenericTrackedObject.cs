using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericTrackedObject : MonoBehaviour {
    public Collider collider;
    public TrackingPoint role;
    
    // Start is called before the first frame update
    void Start() {
        collider.gameObject.AddComponent<Rigidbody>();
        CreateRigidbody(collider.gameObject.GetComponent<Rigidbody>());
    }

    private void CreateRigidbody(Rigidbody rb) {
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.GetComponent<TargetObject>() != null) {
            if (other.gameObject.GetComponent<TargetObject>().MatchCollider(role)) {
                Destroy(other.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
