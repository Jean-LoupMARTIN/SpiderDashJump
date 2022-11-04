using UnityEngine;


[DefaultExecutionOrder(3)]
public class CamFollowTarget : MonoBehaviour
{
    static public CamFollowTarget inst;

    [SerializeField] Transform target;
    [SerializeField] float positionSpeed = 10;
    [SerializeField] float rotationSpeed = 10;
    [SerializeField] LayerMask layer;

    Vector3 posProj;
    Quaternion rotProj;

    Player3D player3D;
    Controller controller;
    [SerializeField] float stickAngle = 30;

    public Player3D Player3D { get => player3D; }

    void Awake()
    {
        inst = this;

        posProj = target.InverseTransformPoint(transform.position);
        rotProj = Quaternion.Inverse(target.rotation) * transform.rotation;

        player3D = target?.GetComponentInParent<Player3D>();
        controller = player3D?.Controller;
    }

    void Update()
    {
        Quaternion stickRot = Quaternion.Euler(-stickAngle * controller.StickArcValue(false, false, 60), 0, 0);

        Quaternion rot = target.rotation * rotProj * stickRot;

        Vector3 pos = target.TransformPoint(stickRot * posProj);
        Vector3 dir = pos - target.position;

        if (Physics.Raycast(target.position, dir, out RaycastHit hit, dir.magnitude, layer))
            pos = hit.point;

        transform.position = Vector3   .Lerp(transform.position, pos, Time.deltaTime * positionSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * rotationSpeed);
    }
}
