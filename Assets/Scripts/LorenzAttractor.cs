using UnityEngine;

public class LorenzAttractor : MonoBehaviour {

    // [SerializeField]
    // float sigma, rho, beta; // 1, 6.42, 1 makes something somewhat interesting
    // float σ, ρ, β; // NOTE: p = r in the german attractor site constants
    // try 10, 30, 2.667 (8/3)
    float o=10f, r=30f, b=2.667f;

    // uint u = 0; // ??? "Attractor parameters"
    [SerializeField]
    float speed = 1f;

    void Awake() {
        var scale = Vector3.one * 0.05f;
        transform.localScale = scale;
        transform.localPosition = new Vector3(
            Random.Range(-0.001f, 0.001f),
            Random.Range(-0.001f, 0.001f),
            Random.Range(-0.001f, 0.001f));
    }

    void Update() {
        // var timestep = Time.fixedTime;
        var timestep = Time.deltaTime;
        timestep *= speed;
        Vector3 pos = transform.localPosition;
        pos.x += o * (pos.y - pos.x) * timestep;
        pos.y += (pos.x * (r - pos.z) - pos.y) * timestep;
        pos.z += (pos.x * pos.y - b * pos.z) * timestep;
        transform.localPosition = pos;
    }
}

// http://www.3d-meier.de/tut19/Seite0.html # All attractors
// http://www.3d-meier.de/tut19/Seite78.html # Lorenz attractor
// http://www.3d-meier.de/tut19/Seite76.html # Coupled Lorenz
// http://www.3d-meier.de/tut19/Seite105.html # Hyperchaotic Lorenz

// https://stackoverflow.com/questions/2422750/in-opengl-vertex-shaders-what-is-w-and-why-do-i-divide-by-it
// https://community.khronos.org/t/x-y-z-and-w/18360
// https://answers.unity.com/questions/147712/what-is-affected-by-the-w-in-quaternionxyzw.html



// dx/dt = σ(y - x)
// dy/dt = x(ρ - z) - y
// dz/dt = xy - βz

// In these equations, σ, ρ, and β are parameters that define the behavior of the system. 
// The parameter σ represents the rate of heat transfer
// ρ represents the rate of fluid circulation, 
// and β represents the ratio of the height to the length of the box-shaped region in which the system operates.


// each element in this array is a set of params for a different attractor
// float[] attractorParams = {
//  {10.0f, 30.0f, 8 / 3}, // http://www.3d-meier.de/tut19/Seite76.html (Coupled L A)
// 	{1.24f, 1.1f, 4.4f, 3.21f},
// 	{0.95f, 0.7f, 0.6f, 3.5f, 0.25f, 0.1f},
// 	{0.3f, 1.0f},
// 	{5.0f, -10.0f, -0.38f},
// 	{1.4f},
// 	{0.001f, 0.2f, 1.1f},
// 	{0.4f, 0.175f},
// 	{1.5f},
// 	{0.2f}
// }