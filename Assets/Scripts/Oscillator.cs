using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector;  // how far to oscillate
    [SerializeField] float cycleTime = 2f;  // how long it takes to oscillate
    Vector3 startingPosition;

    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        const float tau = Mathf.PI * 2;
        float cycles = Time.timeSinceLevelLoad / cycleTime;
        float rawSinWave = Mathf.Sin(tau * cycles);  // [-1, 1]
        float movementFactor = (rawSinWave + 1f) / 2f;  // [0, 1]
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPosition + offset;
    }
}
