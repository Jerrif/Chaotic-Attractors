using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Swarm swarm;
    private CameraSingleton cameraSingleton;

    private bool userControlling = false;
    // private float moveSpeed = 0.25f;
    private float moveSpeed = 1f;

    const float moveDuration = 8f;
    float elapsed = 0f;
    float dollyStartDistance;
    float dollyFinalDistance;
    float orbitDistance;

    void Start() {
        cameraSingleton = CameraSingleton.Instance;
        if (cameraSingleton == null) {
            return;
        }
        cameraSingleton.transform.position = swarm.transform.position;
        dollyStartDistance = 150f; // hardcoded for now; should be added to attractorData I guess
        dollyFinalDistance = 30f;
        orbitDistance = swarm.attractor.cameraDist;
    }

    void Update() {

        // DollyThenOrbit(moveDuration);
        HandleInput();
        if (!userControlling) {
            Orbit();
        }
    }

    void HandleInput() {
        if (Input.anyKeyDown){
            userControlling = true;
        }
        if (Input.GetKey(KeyCode.O)) {
            userControlling = false;
        }

        if (Input.GetKey(KeyCode.W)) {
            Vector3 currentPosition = cameraSingleton.transform.position;
            Quaternion currentRotation = cameraSingleton.transform.rotation;
            // Calculate the direction vector based on the rotation
            Vector3 direction = currentRotation * Vector3.forward;

            // Scale the direction vector by the movement speed
            direction.Normalize();
            Vector3 movementVector = direction * moveSpeed * Time.deltaTime;

            // Update the character's position by adding the movement vector
            Vector3 newPosition = currentPosition + movementVector;
            cameraSingleton.transform.position = newPosition;

        }
        if (Input.GetKey(KeyCode.A)) {
            Vector3 currentPosition = cameraSingleton.transform.position;
            Quaternion currentRotation = cameraSingleton.transform.rotation;
            // Calculate the direction vector based on the rotation
            Vector3 direction = currentRotation * Vector3.left;

            // Scale the direction vector by the movement speed
            direction.Normalize();
            Vector3 movementVector = direction * moveSpeed * Time.deltaTime;

            // Update the character's position by adding the movement vector
            Vector3 newPosition = currentPosition + movementVector;
            cameraSingleton.transform.position = newPosition;

        }
    }

    void Orbit() {
        var positionFactor = Time.fixedTime * cameraSingleton.speed;

        cameraSingleton.transform.position = new Vector3 {
            x = Mathf.Cos(positionFactor) * orbitDistance,
            y = 0f,
            z = Mathf.Sin(positionFactor) * orbitDistance
        };
        cameraSingleton.transform.LookAt(Vector3.zero, Vector3.up);
    }

    void DollyThenOrbit(float duration) {
        // duration is in seconds
        elapsed += Time.deltaTime;
        if (elapsed <= duration) {
            orbitDistance = Mathf.SmoothStep(dollyStartDistance, dollyFinalDistance, elapsed/duration);
        } else {
            orbitDistance = dollyFinalDistance;
        }
        Orbit();
    }

}
