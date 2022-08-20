using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] int upThrust = 1000;
    [SerializeField] int rotationSpeed = 100;
    [SerializeField] AudioClip moveAudio;
    [SerializeField] ParticleSystem moveParticles;

    Rigidbody rigidBody;
    AudioSource audioSource;

    bool moveAudioPlaying = false;
    bool moveParticlesActivated = false;
    bool movingUp = false;
    bool movingLeft = false;
    bool movingRight = false;
    bool disableCrashing = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessThrust();
        ProcessRotation();
        ProcessCheatInputs();
    }

    void ProcessThrust() {
        if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) {
            ApplyUpwardsThrust();
            ApplyMovementAudio();
            ApplyMovementParticles();
        } else {
            DisableUpwardsThrust();
            DisableMovementAudio();
            DisableMovementParticles();
        }
    }

    public bool IsApplyingMovement() {
        return movingUp || movingLeft || movingRight;
    }

    public void DisableMovementParticles() {
        if (!IsApplyingMovement() && moveParticlesActivated) {
            moveParticles.Stop();
            moveParticlesActivated = false;
        }
    }

    private void DisableMovementAudio() {
        if (!IsApplyingMovement() && moveAudioPlaying) {
            audioSource.Stop();
            moveAudioPlaying = false;
        }
    }   
    
    private void DisableUpwardsThrust() {
        if (movingUp) {
            movingUp = false;
        }
    }

    private void ApplyMovementParticles() {
        if (!moveParticlesActivated) {
            moveParticles.Play();
            moveParticlesActivated = true;
        }
    }

    private void ApplyMovementAudio() {
        if (!moveAudioPlaying) {
            audioSource.PlayOneShot(moveAudio);
            moveAudioPlaying = true;
        }
    }

    private void ApplyUpwardsThrust() {
        if (!movingUp) movingUp = true;
        Vector3 thrustVector = Vector3.up * upThrust * Time.deltaTime;
        rigidBody.AddRelativeForce(thrustVector);
    }

    void ProcessRotation() {
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) {
            return;
        }
        MaybeRotateLeft();
        MaybeRotateRight();
    }

    private void MaybeRotateLeft()
    {
        if (Input.GetKey(KeyCode.A)) {
            if (!movingLeft) movingLeft = true;
            ApplyRotation(rotationSpeed);
            ApplyMovementAudio();
            ApplyMovementParticles();
        } else {
            if (movingLeft) movingLeft = false;
            DisableMovementAudio();
            DisableMovementParticles();
        }
    }

    private void MaybeRotateRight()
    {
        if (Input.GetKey(KeyCode.D)) {
            if (!movingRight) movingRight = true;
            ApplyRotation(rotationSpeed * -1);
            ApplyMovementAudio();
            ApplyMovementParticles();
        } else {
            if (movingRight) movingRight = false;
            DisableMovementAudio();
            DisableMovementParticles();
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

    void ProcessCheatInputs() {
        if (Input.GetKeyDown(KeyCode.L)) {
            GetComponent<CollisionHandler>().LoadNextLevel();
        } else if (Input.GetKeyDown(KeyCode.C)) {
            ToggleCrashing();
        }
    }

    void ToggleCrashing() {
        if (disableCrashing) {
            disableCrashing = false;
            GetComponent<CollisionHandler>().EnableCrashing();
        } else {
            disableCrashing = true;
            GetComponent<CollisionHandler>().DisableCrashing();
        }
    }
}
