using UnityEngine;

public readonly struct Attractors {

    public static Vector3 Lorenz(Vector3 pos, float deltaTime, float speed) {
        const float attractorSpeed = 0.75f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float o=10f, r=30f, b=2.667f;
        Vector3 d = pos;

        d.x += o * (pos.y - pos.x);
        d.y += (pos.x * (r - pos.z) - pos.y);
        d.z += (pos.x * pos.y - b * pos.z);
        return pos + d * timestep;
    }

    public static Vector3 HyperchaoticLorenz(Vector3 pos, float deltaTime, float speed) {
        const float attractorSpeed = 0.75f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=10f, b=2.667f, c=28f;

        pos.x += (a * (pos.y - pos.x)) * timestep;
        pos.y += ((pos.x * pos.z * -1) + c * pos.x - pos.y) * timestep;
        pos.z += ((b * pos.z * -1) + pos.x * pos.y) * timestep;
        return pos;
    }

    public static Vector3 Unnamed(Vector3 pos, float deltaTime, float speed) {
        // from page 4 of: http://lsc.amss.ac.cn/~ljh/04LCC.pdf , page 13 has a table with a bunch of good constants/parameters
        const float attractorSpeed = 0.25f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=-10f, b=-4f, c=18.1f;
        // const float a=-10f, b=-5.607f, c=18.1f;

        pos.x += ((a * b / (a + b)* -1) * pos.x - pos.y * pos.z + c) * timestep;
        pos.y += (a * pos.y + pos.x * pos.z) * timestep;
        pos.z += (b * pos.z + pos.x * pos.y) * timestep;
        return pos;
    }

    public static Vector3 Unnamed2(Vector3 pos, float deltaTime, float speed) {
        // same as Unnamed but with the `c` constant changed
        const float attractorSpeed = 0.25f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=-20f, b=-4.980f, c=9f; // -10, 4, (0 or 9)

        pos.x += ((a * b / (a + b)* -1) * pos.x - pos.y * pos.z + c) * timestep;
        pos.y += (a * pos.y + pos.x * pos.z) * timestep;
        pos.z += (b * pos.z + pos.x * pos.y) * timestep;
        return pos;
    }

    public static Vector3 Rossler(Vector3 pos, float deltaTime, float speed) {
        const float attractorSpeed = 3f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=0.2f, b=0.2f, c=5.7f;

        pos.x += (-pos.y - pos.z) * timestep;
        pos.y += (pos.x + a * pos.y) * timestep;
        pos.z += (b + pos.x * pos.z - c * pos.z) * timestep;
        return pos;
    }

    public static Vector3 Rucklidge(Vector3 pos, float deltaTime, float speed) {
        const float attractorSpeed = 2f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=-2.0f, b=-6.7f;

        pos.x += (a * pos.x - b * pos.y - pos.y * pos.z) * timestep;
        pos.y += (pos.x) * timestep;
        pos.z += (-pos.z + pos.y * pos.y) * timestep;
        return pos;
    }

    public static Vector3 Chen(Vector3 pos, float deltaTime, float speed) {
        const float attractorSpeed = 0.25f;
        float timestep = deltaTime * attractorSpeed * speed;
        const float a=35f, b=3f, c=28f;

        pos.x += (a * (pos.y - pos.x)) * timestep;
        pos.y += ((c - a) * pos.x - pos.x * pos.z + c * pos.y) * timestep;
        pos.z += (pos.x * pos.y - b * pos.z) * timestep;
        return pos;
    }

    public static Vector3 ChenLin(Vector3 pos, float deltaTime, float speed) {
        const float attractorSpeed = 0.5f;
        float timestep = deltaTime * attractorSpeed * speed;
        // const float a=5f, b=-10f, c=-3.4f, d1=-1f, d2=1f, d3=1f;
        const float a=0.5f, b=-10f, c=-4f, d1=1f, d2=-1f, d3=-1f;

        pos.x += (a * pos.x + d1 * pos.y * pos.z) * timestep;
        pos.y += (b * pos.y + d2 * pos.x * pos.z) * timestep;
        pos.z += (c * pos.z + d3 * pos.x * pos.y) * timestep;
        return pos;
    }

    public static Vector3 Sprott33(Vector3 pos, float deltaTime, float speed) {
        // meh
        const float attractorSpeed = 2.5f;
        float timestep = deltaTime * attractorSpeed * speed;

        pos.x += (pos.y) * timestep;
        pos.y += (-pos.x + pos.y * pos.z) * timestep;
        pos.z += (1 - pos.y * pos.y) * timestep;
        return pos;
    }
}
