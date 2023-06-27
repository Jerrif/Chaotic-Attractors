using UnityEngine;

public readonly struct Attractors {

    public static Vector4 Lorenz(Vector4 pos, float deltaTime, float speed) {
        const float attractorSpeed = 0.75f;
        // float timestep = Time.fixedDeltaTime * attractorSpeed * speed;
        float timestep = deltaTime * attractorSpeed * speed;
        const float o=10f, r=30f, b=2.667f;
        Vector4 d = pos;

        d.x += o * (pos.y - pos.x);
        d.y += (pos.x * (r - pos.z) - pos.y);
        d.z += (pos.x * pos.y - b * pos.z);
        return pos + d * timestep;
    }

    public static Vector4 HyperchaoticLorenz(Vector4 pos, float deltaTime, float speed) {
        // starting positions:
        // point.localPosition = new Vector4( Random.Range(4.5f, 5.5f), Random.Range(7.5f, 8.5f), Random.Range(11.5f, 12.5f), 21f);
        const float attractorSpeed = 0.75f;
        // float timestep = Time.fixedDeltaTime * attractorSpeed * speed;
        float timestep = deltaTime * attractorSpeed * speed;

        const float a=10f, b=2.667f, c=28f, d=1.1f;

        pos.x += (a * (pos.y - pos.x) + pos.w) * timestep;
        pos.y += ((pos.x * pos.z * -1) + c * pos.x - pos.y) * timestep;
        pos.z += ((b * pos.z * -1) + pos.x * pos.y) * timestep;
        pos.w += ((d * pos.w) - (pos.x * pos.z)) * timestep; // NOTE: this doesn't actually do anything LMAO, can prob switch back to vector3
        return pos;
    }

    public static Vector4 Unnamed(Vector4 pos, float deltaTime, float speed) {
        // from page 4 of: http://lsc.amss.ac.cn/~ljh/04LCC.pdf
        // NOTE: page 13 has a table with a bunch of good constants/parameters
        // also page 14, where it says `Case 2 (a = -20)`
        const float attractorSpeed = 0.25f;
        // float timestep = Time.fixedDeltaTime * attractorSpeed * speed;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=-10f, b=-4f, c=18.1f;
        // const float a=-10f, b=-5.607f, c=18.1f;

        pos.x += ((a * b / (a + b)* -1) * pos.x - pos.y * pos.z + c) * timestep;
        pos.y += (a * pos.y + pos.x * pos.z) * timestep;
        pos.z += (b * pos.z + pos.x * pos.y) * timestep;
        return pos;
    }

    public static Vector4 Unnamed2(Vector4 pos, float deltaTime, float speed) {
        // same as Unnamed but with the `c` constant changed
        const float attractorSpeed = 0.25f;
        float timestep = deltaTime * attractorSpeed * speed;
        // const float a=-10f, b=-4f, c=0f;
        // const float a=-10f, b=-4f, c=9f;
        const float a=-20f, b=-4.980f, c=9f;

        pos.x += ((a * b / (a + b)* -1) * pos.x - pos.y * pos.z + c) * timestep;
        pos.y += (a * pos.y + pos.x * pos.z) * timestep;
        pos.z += (b * pos.z + pos.x * pos.y) * timestep;
        return pos;
    }

    public static Vector4 Rossler(Vector4 pos, float deltaTime, float speed) {
        const float attractorSpeed = 3f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=0.2f, b=0.2f, c=5.7f;

        pos.x += (-pos.y - pos.z) * timestep;
        pos.y += (pos.x + a * pos.y) * timestep;
        pos.z += (b + pos.x * pos.z - c * pos.z) * timestep;
        return pos;
    }

    public static Vector4 Rucklidge(Vector4 pos, float deltaTime, float speed) {
        const float attractorSpeed = 2f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=-2.0f, b=-6.7f;

        pos.x += (a * pos.x - b * pos.y - pos.y * pos.z) * timestep;
        pos.y += (pos.x) * timestep;
        pos.z += (-pos.z + pos.y * pos.y) * timestep;
        return pos;
    }

    public static Vector4 Chen(Vector4 pos, float deltaTime, float speed) {
        const float attractorSpeed = 0.25f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=35f, b=3f, c=28f;

        pos.x += (a * (pos.y - pos.x)) * timestep;
        pos.y += ((c - a) * pos.x - pos.x * pos.z + c * pos.y) * timestep;
        pos.z += (pos.x * pos.y - b * pos.z) * timestep;
        return pos;
    }

    public static Vector4 ChenLin(Vector4 pos, float deltaTime, float speed) {
        const float attractorSpeed = 0.5f;
        float timestep = deltaTime * attractorSpeed * speed;
        // const float a=5f, b=-10f, c=-3.4f, d1=-1f, d2=1f, d3=1f;
        const float a=0.5f, b=-10f, c=-4f, d1=1f, d2=-1f, d3=-1f;

        pos.x += (a * pos.x + d1 * pos.y * pos.z) * timestep;
        pos.y += (b * pos.y + d2 * pos.x * pos.z) * timestep;
        pos.z += (c * pos.z + d3 * pos.x * pos.y) * timestep;
        return pos;
    }

    public static Vector4 Sprott33(Vector4 pos, float deltaTime, float speed) {
        // meh
        const float attractorSpeed = 2.5f;
        float timestep = deltaTime * attractorSpeed * speed;

        pos.x += (pos.y) * timestep;
        pos.y += (-pos.x + pos.y * pos.z) * timestep;
        pos.z += (1 - pos.y * pos.y) * timestep;
        return pos;
    }
}
