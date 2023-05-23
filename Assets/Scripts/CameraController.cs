using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour {

    [SerializeField]
    private Swarm swarm;
    private CameraSingleton cameraSingleton;

    private bool userControlling = false;
    private const float slowMoveSpeed = 1f;
    private const float normalMoveSpeed = 2f;
    private const float fastMoveSpeed = 4f;
    private float moveSpeed = normalMoveSpeed;

    private const float moveDuration = 8f;
    private float elapsed = 0f;
    private float dollyStartDistance;
    private float dollyFinalDistance;
    private float orbitDistance;

    private Vector3 inputVector;
    private Vector2 inputVectorRotation;



    private Vector2 initialDirection;
    Vector2 _mouseAbsolute;
    Vector2 _smoothMouse;

    public Vector2 clampInDegrees = new Vector2(360, 180);
    public bool lockCursor;
    public Vector2 sensitivity = new Vector2(2, 2);
    public Vector2 smoothing = new Vector2(3, 3);


    void Start() {
        cameraSingleton = CameraSingleton.Instance;
        if (cameraSingleton == null) {
            return;
        }
        cameraSingleton.transform.position = swarm.transform.position;
        dollyStartDistance = 150f; // hardcoded for now; should be added to attractorData I guess
        dollyFinalDistance = 30f;
        orbitDistance = swarm.attractor.cameraDist;

        // set the initial direction of the thingo
        initialDirection = cameraSingleton.transform.rotation.eulerAngles;
    }

    void Update() {
        // DollyThenOrbit(moveDuration);
        HandleInput();
        if (!userControlling) {
            Orbit();
        }
    }

    void HandleInput() {
        Vector3 currentPosition = cameraSingleton.transform.position;
        Quaternion currentRotation = cameraSingleton.transform.rotation;

        Vector3 direction = currentRotation * inputVector;
        direction.Normalize();
        Vector3 movementVector = direction * moveSpeed * Time.deltaTime;

        Vector3 newPosition = currentPosition + movementVector;
        cameraSingleton.transform.position = newPosition;

        HandleRotation();
    }

    void HandleRotation() {
        if (inputVectorRotation == Vector2.zero) {
            return;
        }

        // mouse look script from UnityCommunity on github:
        // https://docs.unity3d.com/ScriptReference/Transform.Rotate.html
        // also look at:
        // https://forum.unity.com/threads/a-free-simple-smooth-mouselook.73117/

        // Allow the script to clamp based on a desired target value.
        Quaternion targetOrientation = Quaternion.Euler(initialDirection);
        // Quaternion targetOrientation = Quaternion.identity;

        // Get raw mouse input for a cleaner reading on more sensitive mice.
        // var mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // Scale input against the sensitivity setting and multiply that against the smoothing value.
        // inputVectorRotation = Vector2.Scale(inputVectorRotation, new Vector2(sensitivity.x * smoothing.x, sensitivity.y * smoothing.y));

        // Interpolate mouse movement over time to apply smoothing delta.
        _smoothMouse.x = Mathf.Lerp(_smoothMouse.x, inputVectorRotation.x, 1f / smoothing.x);
        _smoothMouse.y = Mathf.Lerp(_smoothMouse.y, inputVectorRotation.y, 1f / smoothing.y);

        // Find the absolute mouse movement value from point zero.
        _mouseAbsolute += _smoothMouse;
        // _mouseAbsolute += new Vector2(inputVectorRotation.x, inputVectorRotation.y);

        // Clamp and apply the local x value first, so as not to be affected by world transforms.
        if (clampInDegrees.x < 360)
            _mouseAbsolute.x = Mathf.Clamp(_mouseAbsolute.x, -clampInDegrees.x * 0.5f, clampInDegrees.x * 0.5f);

        var xRotation = Quaternion.AngleAxis(-_mouseAbsolute.y, targetOrientation * Vector3.right);
        cameraSingleton.transform.localRotation = xRotation; // AHA! this is what zeros out the Z axis

        if (clampInDegrees.y < 360)
            _mouseAbsolute.y = Mathf.Clamp(_mouseAbsolute.y, -clampInDegrees.y * 0.5f, clampInDegrees.y * 0.5f);

        var yRotation = Quaternion.AngleAxis(_mouseAbsolute.x, cameraSingleton.transform.InverseTransformDirection(Vector3.up));
        cameraSingleton.transform.localRotation *= yRotation;
        
        // var zRotation = Quaternion.AngleAxis(10f, targetOrientation * Vector3.forward);
        // cameraSingleton.transform.localRotation *= zRotation;

        cameraSingleton.transform.rotation *= targetOrientation;

        // TODO: PLEASE TRY THIS GUYS CODE AT THE BOTTOM HERE. IT LOOKS WAY SIMPLER
        // https://forum.unity.com/threads/input-system-raw-input-from-mouse.949914/
        // and this:
        // https://www.reddit.com/r/Unity3D/comments/ups1or/mouse_delta_in_the_new_input_system_is_too_fast/


        // TODO: maybe just read:
        // https://vionixstudio.com/2022/06/16/unity-quaternion-and-rotation-guide/

        // TODO: watch
        // https://www.youtube.com/watch?v=tE1qH8OxO2Y
        

    }

    void OnMovement(InputValue value) {
        userControlling = true;
        inputVector = value.Get<Vector3>();
    }

    void OnMouseRotate(InputValue value) {
        if (!userControlling) {
            userControlling = true;
            _mouseAbsolute = Vector2.zero;
            _smoothMouse = Vector2.zero;
            initialDirection = cameraSingleton.transform.rotation.eulerAngles;
        }
        inputVectorRotation = value.Get<Vector2>();
    }

    void OnOrbit() {
        userControlling = false;
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

    // *******************************
    // USING UNITY'S OLD INPUT SYSTEM
    // *******************************
    // void HandleInput() {
    //     if (Input.GetKey(KeyCode.O)) {
    //         userControlling = false;
    //         return;
    //     }
    //     if (Input.anyKeyDown){
    //         userControlling = true;
    //     }
    //     HandleMouseLook();
    //     HandleMovementKeys();
    // }

    // void HandleMouseLook() {
    //     if (Input.GetMouseButton(1)) {
    //         print(Input.GetAxis("Horizontal"));
    //     }
    // }

    // void HandleMovementKeys() {
    //     if (Input.GetKey(KeyCode.LeftShift)) {
    //         moveSpeed = fastMoveSpeed;
    //     } else if (Input.GetKey(KeyCode.LeftControl)) {
    //         moveSpeed = slowMoveSpeed;
    //     } else {
    //         moveSpeed = normalMoveSpeed;
    //     }

    //     // TODO: move these to the top? Make the instance vars?
    //     Vector3 currentPosition = cameraSingleton.transform.position;
    //     Quaternion currentRotation = cameraSingleton.transform.rotation;
    //     Vector3 direction = Vector3.zero;

    //     // Calculate the direction vector based on the rotation
    //     if (Input.GetKey(KeyCode.W)) {
    //         direction += currentRotation * Vector3.forward;
    //     }
    //     if (Input.GetKey(KeyCode.A)) {
    //         direction += currentRotation * Vector3.left;
    //     }
    //     if (Input.GetKey(KeyCode.S)) {
    //         direction += currentRotation * Vector3.back;
    //     }
    //     if (Input.GetKey(KeyCode.D)) {
    //         direction += currentRotation * Vector3.right;
    //     }

    //     // Scale the direction vector by the movement speed
    //     direction.Normalize();
    //     Vector3 movementVector = direction * moveSpeed * Time.deltaTime;

    //     // Update the character's position by adding the movement vector
    //     Vector3 newPosition = currentPosition + movementVector;
    //     cameraSingleton.transform.position = newPosition;

    // }

