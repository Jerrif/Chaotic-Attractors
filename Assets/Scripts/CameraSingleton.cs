using UnityEngine;

public class CameraSingleton : MonoBehaviour {
    public static CameraSingleton Instance;

    void Awake() {
        if (Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
}
