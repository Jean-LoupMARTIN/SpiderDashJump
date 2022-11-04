using System;
using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Player3D))]
public class ShakeBody : MonoBehaviour
{
    [SerializeField] Transform body;
    [SerializeField] BodyShakeData bodyShakeIdle, bodyShakeMove;
    [SerializeField] float landingBodyY = 0.8f;
    [SerializeField] float landingBodyYTime = 0.5f;
    [SerializeField] AnimationCurve landingBodyYCurve;
    float landingTime;

    Player3D player3D;
    Vector3 bodyLocalPos;
    Quaternion bodyLocalRot;
    float timeOffset;

    public Transform Body { get => body; }
    public void ResetLandingTime() => landingTime = 0;


    void Awake()
    {
        Cache();
    }

    void OnEnable()
    {
        landingTime = landingBodyYTime;
    }

    void OnDisable()
    {
        body.localPosition = bodyLocalPos;
        body.localRotation = bodyLocalRot;

        landingTime = landingBodyYTime;
    }


    void Cache()
    {
        player3D = GetComponent<Player3D>();
        bodyLocalPos = body.localPosition;
        bodyLocalRot = body.localRotation;
        timeOffset = UnityEngine.Random.value * 1000;
    }

    void Update()
    {
        Vector3    pos = Vector3   .Lerp(bodyShakeIdle.Pos(timeOffset), bodyShakeMove.Pos(timeOffset), player3D.SpeedProgress);
        Quaternion rot = Quaternion.Lerp(bodyShakeIdle.Rot(timeOffset), bodyShakeMove.Rot(timeOffset), player3D.SpeedProgress);

        if (landingTime < landingBodyYTime)
        {
            float progress = Mathf.Clamp01(landingTime / landingBodyYTime);
            float bodyY = landingBodyY * landingBodyYCurve.Evaluate(progress);
            body.localPosition = bodyLocalPos + Vector3.Lerp(new Vector3(0, bodyY, 0), pos, progress);
            body.localRotation = bodyLocalRot * Quaternion.Lerp(Quaternion.identity, rot, progress);
            landingTime += Time.deltaTime;
        }

        else {
            body.localPosition = bodyLocalPos + pos;
            body.localRotation = bodyLocalRot * rot;
        }
    }


    [Serializable]
    public class BodyShakeData
    {
        public float posNoise, rotNoise, ySin;
        public float posNoiseFreq, rotNoiseFreq, ySinFreq;

        public Vector3 Pos(float timeOffset = 0)
        {
            float time = Time.time + timeOffset;

            return new Vector3(
                (0.5f - Mathf.PerlinNoise(000, time * posNoiseFreq)) * posNoise,
                (0.5f - Mathf.PerlinNoise(100, time * posNoiseFreq)) * posNoise + Mathf.Sin(time * Mathf.PI * 2 * ySinFreq) * ySin,
                (0.5f - Mathf.PerlinNoise(200, time * posNoiseFreq)) * posNoise);
        }

        public Quaternion Rot(float timeOffset = 0)
        {
            float time = Time.time + timeOffset;

            return Quaternion.Euler(
                (0.5f - Mathf.PerlinNoise(300, time * rotNoiseFreq)) * rotNoise,
                (0.5f - Mathf.PerlinNoise(400, time * rotNoiseFreq)) * rotNoise,
                (0.5f - Mathf.PerlinNoise(500, time * rotNoiseFreq)) * rotNoise);
        }
    }
}
