using UnityEngine;
using static UnityEngine.Mathf;

public class LerpTest : MonoBehaviour {

    // private float speed = 0.2f;

    public float amplitude = 10f;
    public float period = 5f;
    Vector3 direction;
    Vector3 startPos;

    void Awake() {
        startPos = transform.position;

        // direction = Vector3.zero - transform.position;
        direction = transform.position - Vector3.zero;
        direction.Normalize();
    }

    void Update() {
        float t = Time.fixedTime / period;
        float dist = amplitude * Sin(t);
        // Vector3 p = startPos;
        startPos.x += Sin(t) * direction.x;
        startPos.y += Cos(t) * direction.y;
        startPos.z += Sin(t) * direction.z;

        // transform.position = startPos + direction * dist;
        // transform.position = startPos + direction;
        transform.position = startPos;
    }

    void Spiral() {
        const float a = 20.2f;
        const float b = 0.1f;

        float t = Time.fixedTime;

        // var p = transform.position;
        direction = Vector3.zero - transform.position;
        // direction = -transform.position;
        direction.Normalize();

        // float r = a * t + b;
        // float r = a + b * t;
        float r = a * Exp(-b * t);
        float h = 1f;
        
        Vector3 nextPos = transform.position;
        nextPos.x += (r * Cos(t) + direction.x) * Time.deltaTime;
        nextPos.y += (r * Sin(t) + direction.y) * Time.deltaTime;
        nextPos.z += (h * t + direction.z) * Time.deltaTime;
        nextPos += direction * Time.deltaTime;

        transform.position = nextPos;

    }
}
