using UnityEngine;

public class CameraController : MonoBehaviour {
    private Swarm swarm;
    private CameraSingleton cameraSingleton;

    void Start() {
        // I don't understand why this has to occur every update; it doesn't work on Awake()
        cameraSingleton = CameraSingleton.Instance;
        if (cameraSingleton == null) {
            return;
        }
    }

    void Update() {
        var positionFactor = Time.fixedTime * cameraSingleton.speed;

        cameraSingleton.transform.position = new Vector3 {
            x = Mathf.Cos(positionFactor) * cameraSingleton.distance,
            y = 0f,
            z = Mathf.Sin(positionFactor) * cameraSingleton.distance
        };
        cameraSingleton.transform.LookAt(Vector3.zero, Vector3.up);
    }
}
