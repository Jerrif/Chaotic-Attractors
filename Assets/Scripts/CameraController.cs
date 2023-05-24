using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {

    [SerializeField] private Swarm swarm;
    [SerializeField] public float orbitSpeed = 0.05f;

    private CameraSingleton cameraSingleton;

    private Vector3 orbitDistance;
    private bool userControlling = false;
    private const float moveSpeedChangeAmount = 1f;
    private const float normalMoveSpeed = 2f;
    private float moveSpeed = normalMoveSpeed;

    private Vector3 movementInput;
    private Vector2 rotationInput;

    // used to modify the origin of the clamp. This is set to initial orientation at start.
    private Quaternion initialOrientation;
    private Vector2 smoothMouse;
    private Vector2 accumulatedMouseDelta;

    public float mouseSmoothing = 1f;

    void Start() {
        cameraSingleton = CameraSingleton.Instance;
        if (cameraSingleton == null) {
            return;
        }
        orbitDistance = new Vector3(swarm.attractor.cameraDist, 0, 0);
        cameraSingleton.transform.position = orbitDistance;

        // set the origin of the clamp to the starting rotation of the camera
        initialOrientation = cameraSingleton.transform.rotation;
    }

    void Update() {
        if (userControlling) {
            UpdateMovement();
            UpdateMouseLook();
        } else {
            Orbit();
        }
    }

    void UpdateMovement() {
        Vector3 currentPosition = cameraSingleton.transform.position;
        Quaternion currentRotation = cameraSingleton.transform.rotation;

        Vector3 direction = currentRotation * movementInput;
        direction.Normalize();
        Vector3 movementVector = direction * moveSpeed * Time.deltaTime;

        Vector3 newPosition = currentPosition + movementVector;
        cameraSingleton.transform.position = newPosition;
    }

    void UpdateMouseLook() {
        // mouse look script from UnityCommunity on github: https://docs.unity3d.com/ScriptReference/Transform.Rotate.html
        // this function is relatively complex compared to others because it exclusively uses Quaternions, no Euler conversions
        // (wait, maybe I'm wrong -- I think AngleAxis might be a Euler conversion)
        if (rotationInput == Vector2.zero) {
            return;
        }

        // Interpolate mouse movement over time to apply smoothing delta. This is apparently NOT framerate-independent
        smoothMouse = Vector2.Lerp(smoothMouse, rotationInput, 1f / mouseSmoothing);
        accumulatedMouseDelta += smoothMouse; // this is basically += rotationInput (accumulates the mouse movement this frame)

        // apply the local x value first, so as not to be affected by world transforms.
        var xRotation = Quaternion.AngleAxis(-accumulatedMouseDelta.y, initialOrientation * Vector3.right); // Vector3.forward for z/roll
        cameraSingleton.transform.localRotation = xRotation; // AHA! this is what zeros out the Z axis

        accumulatedMouseDelta.y = Mathf.Clamp(accumulatedMouseDelta.y, -180 * 0.5f, 180 * 0.5f);

        var yRotation = Quaternion.AngleAxis(accumulatedMouseDelta.x, cameraSingleton.transform.InverseTransformDirection(Vector3.up));
        cameraSingleton.transform.localRotation *= yRotation;

        cameraSingleton.transform.rotation *= initialOrientation;
    }

    void OnMovement(InputValue value) {
        userControlling = true;
        movementInput = value.Get<Vector3>();
    }

    void OnMouseLook(InputValue value) {
        if (!userControlling) {
            userControlling = true;
            accumulatedMouseDelta = Vector2.zero;
            smoothMouse = Vector2.zero;
            // update the origin of the clamp to the current rotation of the camera
            initialOrientation = cameraSingleton.transform.rotation;
        }
        rotationInput = value.Get<Vector2>();
    }

    void OnIncreaseSpeed() {
        moveSpeed += moveSpeedChangeAmount;
    }

    void OnDecreaseSpeed() {
        moveSpeed -= moveSpeedChangeAmount;
        moveSpeed = moveSpeed <= moveSpeedChangeAmount ? moveSpeedChangeAmount : moveSpeed;
    }

    void OnOrbitStart() {
        userControlling = false;
    }

    void OnResetPosition() {
        cameraSingleton.transform.position = orbitDistance;
        OnOrbitStart();
    }

    void Orbit() {
        cameraSingleton.transform.RotateAround(Vector3.zero, Vector3.up, orbitSpeed);
        cameraSingleton.transform.LookAt(Vector3.zero);
    }
}
