using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EntityMover : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void MoveTo(EntityController target, float speed)
    {
        if (target == null) return;

        MoveTo(target.transform.position, speed);
    }

    public void MoveTo(Vector3 target, float speed)
    {
        agent.speed = speed;
        agent.SetDestination(target);
    }

    public void MoveControl(bool isStop)
    {
        agent.isStopped = isStop;
    }
}
