using UnityEngine;

// public static class Attractors {
public readonly struct Attractors {

    public static Vector3 Lorenz(Vector3 pos, float speed) {
        const float attractorSpeed = 0.75f;
        float timestep = Time.fixedDeltaTime * attractorSpeed * speed;
        const float o=10f, r=30f, b=2.667f;
        Vector3 d = pos;

        // d.x = o * (pos.y - pos.x);
        // d.y = (pos.x * (r - pos.z) - pos.y);
        // d.z = (pos.x * pos.y - b * pos.z);
        // return d * timestep;

        d.x += o * (pos.y - pos.x);
        d.y += (pos.x * (r - pos.z) - pos.y);
        d.z += (pos.x * pos.y - b * pos.z);
        return pos + d * timestep;
    }

    // public static Vector4 HyperchaoticLorenz(Vector4 pos) {
    //     // starting positions:
    //     // point.localPosition = new Vector4( Random.Range(4.5f, 5.5f), Random.Range(7.5f, 8.5f), Random.Range(11.5f, 12.5f), 21f);

    //     const float a=10f, b=2.667f, c=28f, d=1.1f;

    //     pos.x += (a * (pos.y - pos.x) + pos.w) * timestep;
    //     pos.y += ((pos.x * pos.z * -1) + c * pos.x - pos.y) * timestep;
    //     pos.z += ((b * pos.z * -1) + pos.x * pos.y) * timestep;
    //     pos.w += ((d * pos.w) - (pos.x * pos.z)) * timestep;
    //     return pos;
    // }

    public static Vector3 Unnamed(Vector3 pos, float speed) {
        // from page 4 of: http://lsc.amss.ac.cn/~ljh/04LCC.pdf
        // NOTE: I think the camera dist was 110
        const float attractorSpeed = 0.25f;
        float timestep = Time.fixedDeltaTime * attractorSpeed * speed;
        const float a=-10f, b=-4f, c=18.1f;

        pos.x += ((a * b / (a + b)* -1) * pos.x - pos.y * pos.z + c) * timestep;
        pos.y += (a * pos.y + pos.x * pos.z) * timestep;
        pos.z += (b * pos.z + pos.x * pos.y) * timestep;
        return pos;

        // pos.x += ((a * b / (a + b)* -1) * pos.x - pos.y * pos.z + c);
        // pos.y += (a * pos.y + pos.x * pos.z);
        // pos.z += (b * pos.z + pos.x * pos.y);
        // return pos * timestep;

        // Vector3 d = pos;
        // d.x = ((a * b / (a + b)* -1) * pos.x - pos.y * pos.z + c) * timestep;
        // d.y = (a * pos.y + pos.x * pos.z) * timestep;
        // d.z = (b * pos.z + pos.x * pos.y) * timestep;
        // return d;
    }
}
