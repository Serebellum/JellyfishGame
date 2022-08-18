using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] int upThrust = 1000;
    [SerializeField] int rotationSpeed = 100;

    Rigidbody rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
    }

    void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) {
            Vector3 thrustVector = Vector3.up * upThrust * Time.deltaTime;
            rigidBody.AddRelativeForce(thrustVector);
        }
    }

    void ProcessRotation() {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) {
            return;
        }
        if (Input.GetKey(KeyCode.A)) {
            ApplyRotation(rotationSpeed);
        }
        if (Input.GetKey(KeyCode.D)) {
            ApplyRotation(rotationSpeed * -1);
        }
    }

    void ApplyRotation(float rotationSpeed) {
        // Freeze the rotation so as to override the Rigidbody physics system.
        rigidBody.freezeRotation = true;
        Vector3 rotationVector = Vector3.forward * rotationSpeed * Time.deltaTime;
        transform.Rotate(rotationVector);
        // Unfreeze to allow Rigidbody physics system to do its thing.
        rigidBody.freezeRotation = false;
    }
}
