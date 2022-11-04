using UnityEngine;


public class FollowTargetController : Controller
{
    [SerializeField] Transform target;
    [SerializeField] float distStop = 1;

    public Transform Target { get => target; set => target = value; }

    void Update()
    {
        if ((transform.position - target.position).magnitude < distStop)
        {
            StickL = Vector2.zero;
            StickR = Vector2.zero;
        }

        Vector3 targetProj = transform.InverseTransformPoint(target.position);
        targetProj.y = 0;
        targetProj.Normalize();

        float angle = Vector3.SignedAngle(Vector3.forward, targetProj, Vector3.up);

        StickL = new Vector2(targetProj.x, targetProj.z);
        StickR = new Vector2(angle > 0 ? 1 : -1, 0);
    }
}
