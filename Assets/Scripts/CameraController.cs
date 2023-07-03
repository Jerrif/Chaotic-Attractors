using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {

    [SerializeField] private Swarm swarm;
    [SerializeField] public float orbitSpeed = 1f; // now scaled w/ deltaTime, used to be 0.05f

    private CameraSingleton cameraSingleton;

    private Vector3 defaultOrbitDistance;
    private bool userControlling = false;
    private const float moveSpeedChangeAmount = 5f;
    private const float normalMoveSpeed = 20f;
    private float moveSpeed = normalMoveSpeed;

    private Vector3 movementInput;
    private Vector2 rotationInput;

    // used to modify the origin of the mouselook clamp. This is set to initial orientation at start of mouselook.
    private Quaternion initialOrientation;
    private Vector2 smoothMouse;
    private Vector2 accumulatedMouseDelta;
    [SerializeField] private float mouseSmoothing = 1f;

    private Quaternion slerpBeginPos;
    private Vector3 orbitDistanceLerpBeginPos;
    private Vector3 positionTarget;

    void Start() {
        cameraSingleton = CameraSingleton.Instance;
        if (cameraSingleton == null) {
            return;
        }
        defaultOrbitDistance = new Vector3(swarm.attractor.cameraDist, 0, 0);
        cameraSingleton.transform.position = defaultOrbitDistance;

        // set the origin of the mouselook clamp to the starting rotation of the camera
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

        cameraSingleton.transform.position = currentPosition + movementVector;
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
        if (!userControlling) {
            userControlling = true;
            accumulatedMouseDelta = Vector2.zero;
            smoothMouse = Vector2.zero;
            initialOrientation = cameraSingleton.transform.rotation;
        }
        movementInput = value.Get<Vector3>();
    }

    void OnMouseLook(InputValue value) {
        if (!userControlling) {
            userControlling = true;
            accumulatedMouseDelta = Vector2.zero;
            smoothMouse = Vector2.zero;
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
        cameraSingleton.transform.position = defaultOrbitDistance;
        OnOrbitStart();
    }

    void Orbit() {
        if (swarm.transitioning) {
            Vector3 lookDirection = swarm.attractor.swarmPositionOffset - cameraSingleton.transform.position;
            lookDirection.Normalize();
            Quaternion target = Quaternion.LookRotation(lookDirection);

            if (swarm.transitionProgress == 0f) {
                slerpBeginPos = cameraSingleton.transform.rotation;
                orbitDistanceLerpBeginPos = cameraSingleton.transform.position;
                positionTarget = swarm.attractor.swarmPositionOffset + lookDirection * -swarm.attractor.cameraDist;

                // note: this just keeps the camera rotating during the transition lerp. It looks p good without it too
                positionTarget = Quaternion.Euler(0f, orbitSpeed * swarm.transitionDuration, 0f) * positionTarget;
            }
            float progress = swarm.transitionProgress / swarm.transitionDuration;
            
            cameraSingleton.transform.position = Vector3.Lerp(orbitDistanceLerpBeginPos, positionTarget, progress);
            cameraSingleton.transform.rotation = Quaternion.Slerp(slerpBeginPos, target, MySmoothstep(progress));
        } else {
            cameraSingleton.transform.RotateAround(swarm.attractor.swarmPositionOffset, Vector3.up, orbitSpeed * Time.deltaTime);
            cameraSingleton.transform.LookAt(swarm.attractor.swarmPositionOffset);
        }
    }
    
    float MySmoothstep(float t) {
        float start = t * t;
        float end = 1.0f - (1.0f - t) * (1.0f - t);
        return Mathf.Lerp(start, end, t);
    }
}
