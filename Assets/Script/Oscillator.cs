using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector;
    [SerializeField] float period = 2f;

    [SerializeField] float movementFactor;

    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(period <= Mathf.Epsilon) { return; }
        float cycles = Time.time / period;
        const float tau = Mathf.PI * 2f;
        float sinWave = Mathf.Sin(cycles * tau); // -1 to 1

        movementFactor = sinWave / 2f + 0.5f; // 0 to 1
        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
