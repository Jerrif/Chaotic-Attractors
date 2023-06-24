using UnityEngine;

public class TrailTest : MonoBehaviour {

    [SerializeField] private Material material;
    [SerializeField] private Swarm swarm;

    private TrailRenderer tr;

    void Start() {
        tr = GetComponent<TrailRenderer>();
        tr.material = material;

        // A simple 2 color gradient with a fixed alpha of 1.0f.
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.red, tr.time) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(0.0f, tr.time) }
        );
        tr.colorGradient = gradient;
    }

    // void Update() {
    //     tr.transform.position = new Vector3(Mathf.Sin(Time.time * 1.51f) * 7.0f, Mathf.Cos(Time.time * 1.27f) * 4.0f, 0.0f);
    // }
}
