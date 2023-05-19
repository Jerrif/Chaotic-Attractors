using UnityEngine;

public class CameraSingleton : MonoBehaviour {
    public static CameraSingleton Instance;
    public float speed = 0.05f;
    public float distance = 60f;

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
