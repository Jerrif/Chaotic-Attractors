using UnityEngine;
using UnityEngine.InputSystem;

public class Swarm : MonoBehaviour {

    [SerializeField] Transform pointPrefab;
    [SerializeField, Range(1, 10000)] int resolution = 500;
    [SerializeField] float speed = 1f;

    public bool transitioning { get; private set; } = false;
    public float transitionDuration { get; private set; } = 5f;
    public float transitionProgress { get; private set; } = 0f;

    Transform[] points;
    Vector3[] lerpBeginPos;

    public delegate Vector3 Function(Vector3 pos, float deltaTime, float speed);

    public struct AttractorData {
        public Function function;
        public float cameraDist;
        public Vector3 swarmPositionOffset; // some attractors are off center
    }

    public enum AttractorName {Lorenz, HyperchaoticLorenz, Unnamed, Unnamed2, Rossler, Rucklidge, Chen, Sprott33, ChenLin};

    static AttractorData[] attractors = new AttractorData[] {
        new AttractorData {
            function = Attractors.Lorenz,
            cameraDist = 55f,
            swarmPositionOffset = new Vector3(0, 0, 31f) },
        new AttractorData {
            function = Attractors.HyperchaoticLorenz,
            cameraDist = 55f,
            swarmPositionOffset = new Vector3(0, 0, 24f) },
        new AttractorData {
            function = Attractors.Unnamed,
            cameraDist = 55f,
            swarmPositionOffset = new Vector3(0, 0, 0) },
        new AttractorData {
            function = Attractors.Unnamed2,
            cameraDist = 55f,
            swarmPositionOffset = new Vector3(0, 0, 0) },
        new AttractorData {
            function = Attractors.Rossler,
            cameraDist = 25f,
            swarmPositionOffset = new Vector3(0, 0, 0) },
        new AttractorData {
            function = Attractors.Rucklidge,
            cameraDist = 18f,
            swarmPositionOffset = new Vector3(0, 0, 6f) },
        new AttractorData {
            function = Attractors.Chen,
            cameraDist = 40f,
            swarmPositionOffset = new Vector3(0, 0, 22.5f) },
        new AttractorData {
            function = Attractors.Sprott33,
            cameraDist = 10f,
            swarmPositionOffset = new Vector3(0, 0, 0) },
        new AttractorData {
            function = Attractors.ChenLin,
            cameraDist = 125f,
            swarmPositionOffset = new Vector3(0, 0, 0f) },
        };

    public static AttractorData GetAttractor(AttractorName name) {
        return attractors[(int)name];
    }

    [SerializeField] public AttractorName attractorName { get; private set; }
    public AttractorData attractor;

    // TODO: a struct of arrays would be more efficient
    private struct PointData {
        public bool isAlive;
        public Vector3 startPos;
    }

    PointData[] pointData;

    void Awake() {
        attractor = GetAttractor(attractorName);
        points = new Transform[resolution];
        pointData = new PointData[resolution];
        lerpBeginPos = new Vector3[resolution];

        for (int i = 0; i < resolution; i++) {
            Transform point = Instantiate(pointPrefab);
            points[i] = point;
            point.localScale = Vector3.one * 0.15f;
            point.localPosition = new Vector3(
                Random.Range(-0.001f, 0.001f),
                Random.Range(-0.001f, 0.001f),
                Random.Range(-0.001f, 0.001f));
            pointData[i].isAlive = true;
            pointData[i].startPos = point.localPosition;
            point.SetParent(transform, false);
        }
    }

    // NOTE: I changed `fixed timestep` from 0.02 to 0.01 in the options
    // This made the trails look much better (smoother) at the cost of performance
    void FixedUpdate() {
        if (transitioning) {
            Transition();
            return;
        }

        float deltaTime = Time.deltaTime;

        for (int i = 0; i < resolution; i++) {
            // TODO: remove all this whack stuff?
            if (!pointData[i].isAlive) {
                continue;
            }
            if (pointData[i].isAlive && isWhack(points[i].localPosition)) {
                string s = pointData[i].startPos.ToString("F8");
                Debug.Log("Whack point: " + s + " Dist: " + Vector3.Distance(pointData[i].startPos, Vector3.zero));
                pointData[i].isAlive = false;
                continue;
            }

            points[i].localPosition = attractor.function(points[i].localPosition, deltaTime, speed);
        }
    }

    bool isWhack(Vector3 pos) {
        if (Vector3.Distance(pos, Vector3.zero) > 20000f) {
            return true;
        }
        return false;
    }

    // note: these are called by the Input system
    void OnNextAttractor() {
        attractorName = (int)attractorName < attractors.Length - 1 ? (AttractorName)attractorName + 1 : (AttractorName)0;
        print(attractorName);
        attractor = GetAttractor(attractorName);
        OnConvergePoints();
    }

    void OnPreviousAttractor() {
        attractorName = (int)attractorName > 0 ? (AttractorName)attractorName - 1 : (AttractorName)attractors.Length - 1;
        print(attractorName);
        attractor = GetAttractor(attractorName);
        OnConvergePoints();
    }

    void OnConvergePoints() {
        transitioning = true;
        transitionProgress = 0f;

        // kinda annoying; For a lerp to work, you need the current pos _from the first frame of the lerp_, not current pos each update
        for (int i=0; i < resolution; i++) {
            // get the point's current position for the lerp back to 0
            lerpBeginPos[i] = points[i].localPosition;
        }
    }

    void OnToggleTrails() {
        for (int i=0; i < resolution; i++) {
            TrailRenderer trail = points[i].GetComponent<TrailRenderer>();
            trail.enabled = !trail.enabled;
        }
    }

    void OnTogglePoints() {
        for (int i=0; i < resolution; i++) {
            MeshRenderer pointRenderer = points[i].GetComponent<MeshRenderer>();
            pointRenderer.enabled = !pointRenderer.enabled;
        }
    }
    
    void Transition() {
        if (transitionProgress >= transitionDuration) {
            transitioning = false;
            return;
        }

        transitionProgress += Time.deltaTime;
        float progress = transitionProgress / transitionDuration;
        progress = progress * progress; // nicer looking lerp

        for (int i=0; i < resolution; i++) {
            if (!pointData[i].isAlive) {
                // revive dead points
                points[i].localPosition = pointData[i].startPos;
                pointData[i].isAlive = true;
                continue;
            }

            points[i].localPosition = Vector3.Slerp(lerpBeginPos[i], pointData[i].startPos, progress);
        }
    }
}


// TODO:
// https://learn.microsoft.com/en-us/windows/mixed-reality/develop/unity/performance-recommendations-for-unity?tabs=openxr#cpu-to-gpu-performance-recommendations