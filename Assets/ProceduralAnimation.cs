using UnityEngine;
using Unity.Mathematics;

sealed class ProceduralAnimation : MonoBehaviour
{
    [Header("Rig Targets")]
    [SerializeField] Transform _hip = null;
    [SerializeField] Transform _frontL = null;
    [SerializeField] Transform _frontR = null;
    [SerializeField] Transform _backL = null;
    [SerializeField] Transform _backR = null;

    [Header("Animation Settings")]
    [SerializeField, Range(0, 20)] float _walkSpeed = 5;
    [SerializeField, Range(0, 0.5f)] float _stride = 0.12f;
    [SerializeField, Range(0, 0.2f)] float _footUp = 0.06f;
    [SerializeField, Range(0, 0.2f)] float _hipUp = 0.02f;
    [SerializeField, Range(0, 1)] float _hipShake = 0.2f;
    [SerializeField, Range(0, 0.2f)] float _noise = 0.02f;

    public float Amplitude { get; set; } = 1;

    float _time;

    void Update()
    {
        // Add simple perlin noise
        float3 noise = new float3(
            Mathf.PerlinNoise(Time.time, 0),
            Mathf.PerlinNoise(0, Time.time),
            Mathf.PerlinNoise(Time.time, Time.time)
        ) * _noise;

        // Hip bob + shake
        var walk = math.sin(_time * 2 + math.PI * 0.25f) * _hipUp * Amplitude;
        _hip.localPosition = noise + math.float3(0, walk - 0.2f * _noise, 0);
        _hip.localRotation = quaternion.RotateY(math.sin(_time) * -_hipShake * Amplitude);

        // Leg cycle (diagonal pairs)
        var y = math.cos(_time) * -_footUp * Amplitude;
        var z = math.sin(_time) * _stride * Amplitude;

        _frontL.localPosition = math.float3(0, math.min(0, -y), +z);
        _backR.localPosition  = math.float3(0, math.min(0, -y), -z);

        _frontR.localPosition = math.float3(0, math.min(0, +y), -z);
        _backL.localPosition  = math.float3(0, math.min(0, +y), +z);

        _time += Time.deltaTime * _walkSpeed * math.pow(Amplitude, 0.5f);
    }
}

