using UnityEngine;

public class Swarm : MonoBehaviour {

    [SerializeField]
    Transform pointPrefab;

    [SerializeField, Range(1, 1000)]
    int resolution = 5;

    [SerializeField]
    float speed = 1f;

    Transform[] points;

    public delegate Vector3 Function(Vector3 pos, float timestep);

    public struct AttractorData {
        public Function function;
        public float cameraPos;
    }

    public enum AttractorName {Lorenz, LorenzGPT, LorenzGPTVerlet, Unnamed};

    static AttractorData[] attractors = new AttractorData[] {
        new AttractorData {
            function = Attractors.Lorenz,
            cameraPos = 70f },
        new AttractorData {
            function = Attractors.LorenzGPT,
            cameraPos = 70f },
        new AttractorData {
            function = Attractors.LorenzGPTVerlet,
            cameraPos = 70f },
        new AttractorData {
            function = Attractors.Unnamed,
            cameraPos = 110f },
        };

    // static Function[] functions = {Attractors.Lorenz, Attractors.LorenzGPT, Attractors.Unnamed};

    public static AttractorData GetAttractor(AttractorName name) {
        return attractors[(int)name];
    }

    [SerializeField]
    AttractorName attractorName;

    AttractorData attractor;

    private struct PointData {
        public bool isAlive;
        public Vector3 startPos;
    }

    PointData[] pointData;

    void Awake() {
        attractor = GetAttractor(attractorName);
        var scale = Vector3.one * 0.05f;
        points = new Transform[resolution];
        pointData = new PointData[resolution];

        Debug.Log("[All Points Start]");
        for (int i = 0; i < resolution; i++) {
            Transform point = Instantiate(pointPrefab);
            points[i] = point;
            point.localScale = scale;
            point.localPosition = new Vector3(
                Random.Range(-0.001f, 0.001f),
                Random.Range(-0.001f, 0.001f),
                Random.Range(-0.001f, 0.001f));
            // point.localPosition = new Vector3(
            //     Random.Range(-0.003f, 0.001f),
            //     Random.Range(-0.003f, 0.001f),
            //     Random.Range(-0.003f, 0.001f));
            // if (i % 4 == 0) {
            //     point.localPosition = new Vector3(0.00037964f, 0.00050654f, 0.00033839f);
            // } else if (i % 4 == 1) {
            //     point.localPosition = new Vector3(0.00048954f, 0.00087665f, 0.00058015f);
            // } else if (i % 4 == 2) {
            //     point.localPosition = new Vector3(-0.00018724f, -0.00045014f, -0.00030304f);
            // } else if (i % 4 == 3) {
            //     point.localPosition = new Vector3(-0.00013153f,  0.00000609f, -0.00002767f);
            // }
            // Debug.Log(point.localPosition.ToString("F8") + " Dist: " + Vector3.Distance(point.localPosition, Vector3.zero));
            pointData[i].isAlive = true;
            pointData[i].startPos = point.localPosition; // NOTE: is this by ref or by val? very important its by val
            point.SetParent(transform, false);
        }
        Debug.Log("\n[Whack Points Start]");
    }

    void Update() {
        // float timestep = Time.deltaTime;
        float timestep = Time.fixedDeltaTime;
        timestep *= speed;
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
            // var newPos = Attractors.Unnamed(point.localPosition, timestep);
            // var newPos = attractors[1].function(point.localPosition, timestep);
            // var newPos = attractors[2].function(point.localPosition, timestep);

            // TODO: look into returning just dx,dy,dz from LorenzGPT, then using the Verlet Integration on that
            // TODO: the point.localPosition would then have to be += newPos I think?
            var accel = attractor.function(point.localPosition, timestep);
            point.localPosition += accel * timestep;
        }
    }

    bool isWhack(Vector3 pos) {
        if (Vector3.Distance(pos, Vector3.zero) > 20000f) {
            return true;
        }
        return false;
    }
}


// Verlet Integration
// https://www.youtube.com/watch?v=AZ8IGOHsjBk

// Verlet integration is a numerical method used to solve equations of motion in physics simulations. It is a method that calculates an object's position and velocity at each time step based on its position and acceleration from the previous time step.

// The basic idea of the Verlet integration method is to use the current position and the acceleration to predict the next position of the object. Then, the difference between the predicted position and the current position is used to calculate the velocity at the current time step. This velocity is then used to predict the position at the next time step, and so on.

// More specifically, in the Verlet integration method, the position and velocity are updated as follows:

// At the initial time step, the current position and the previous position are set to the initial position of the object.
// At each subsequent time step, the new position is calculated using the previous position, the current position, and the acceleration. This gives an estimate of the position at the next time step.
// The velocity at the current time step is then calculated as the difference between the new position and the previous position, divided by the time step.
// The previous position is set to the current position, and the current position is set to the new position.
// The acceleration is updated based on the current position and velocity, and the process repeats for the next time step.
// The Verlet integration method is known for its stability and simplicity, and is often used in physics simulations for this reason. However, it may not be as accurate as other methods, and can introduce numerical errors over time.



