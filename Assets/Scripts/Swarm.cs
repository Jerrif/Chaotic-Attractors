using UnityEngine;

public class Swarm : MonoBehaviour {

    [SerializeField]
    Transform pointPrefab;
    [SerializeField, Range(1, 1000)]
    int resolution = 100;
    [SerializeField]
    float speed = 1f;

    Transform[] points;

    public delegate Vector4 Function(Vector4 pos, float speed);
    // public delegate Vector3 Function(Vector4 pos, float speed);

    public struct AttractorData {
        public Function function;
        public float cameraDist;
        // some attractors just end up at a different place from where they started
        public Vector3 swarmPositionOffset;
    }

    public enum AttractorName {Lorenz, HyperchaoticLorenz, Unnamed};

    static AttractorData[] attractors = new AttractorData[] {
        new AttractorData {
            function = Attractors.Lorenz,
            cameraDist = 70f,
            swarmPositionOffset = new Vector3(0, 0, -31) }, // try 20
        new AttractorData {
            function = Attractors.HyperchaoticLorenz,
            cameraDist = 60f, // 110f
            swarmPositionOffset = new Vector3(0, 0, -24) },
        new AttractorData {
            function = Attractors.Unnamed,
            cameraDist = 60f, // 110f
            swarmPositionOffset = new Vector3(0, 0, 0) },
        };

    public static AttractorData GetAttractor(AttractorName name) {
        return attractors[(int)name];
    }

    [SerializeField]
    AttractorName attractorName;

    public AttractorData attractor;

    private struct PointData {
        public bool isAlive;
        public Vector3 startPos;
    }

    PointData[] pointData;

    void Awake() {
        attractor = GetAttractor(attractorName);
        // move the swarm to be roughly centered on 0, 0, 0
        transform.position = attractor.swarmPositionOffset;
        var scale = Vector3.one * 0.05f;
        points = new Transform[resolution];
        pointData = new PointData[resolution];

        for (int i = 0; i < resolution; i++) {
            Transform point = Instantiate(pointPrefab);
            points[i] = point;
            point.localScale = scale;
            point.localPosition = new Vector3(
                Random.Range(-0.001f, 0.001f),
                Random.Range(-0.001f, 0.001f),
                Random.Range(-0.001f, 0.001f));
            pointData[i].isAlive = true;
            pointData[i].startPos = point.localPosition;
            point.SetParent(transform, false);
        }
    }

    void Update() {
        for (int i = 0; i < resolution; i++) {
            Transform point = points[i]; // I could put this below the first if, but looks nicer here
            if (!pointData[i].isAlive) {
                continue;
            }
            if (pointData[i].isAlive && isWhack(point.localPosition)) {
                string s = pointData[i].startPos.ToString("F8");
                Debug.Log(s + " Dist: " + Vector3.Distance(pointData[i].startPos, Vector3.zero));
                pointData[i].isAlive = false;
                continue;
            }
            
            // = works with Unnamed, += works with Lorenz
            point.localPosition = attractor.function(point.localPosition, speed);
            // point.localPosition += attractor.function(point.localPosition, speed);
        }
    }

    bool isWhack(Vector3 pos) {
        if (Vector3.Distance(pos, Vector3.zero) > 20000f) {
            return true;
        }
        return false;
    }
}
