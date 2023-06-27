using UnityEngine;
using TMPro;

public class FrameRateCounter : MonoBehaviour {

	[SerializeField]
	TextMeshProUGUI display;
	[SerializeField, Range(0.1f, 2f)]
	float sampleDuration = 1f;

    int frames;
    float duration;

    void Update() {
        float frameDuration = Time.unscaledDeltaTime;
        frames += 1;
        duration += frameDuration;
        
        if (duration >= sampleDuration) {
            display.SetText("FPS\n{0:0}\nMS\n{1:1}", frames / duration, 1000f * duration / frames);
            frames = 0;
            duration = 0f;
        }
    }
}
