using TMPro;
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

    public void UnitMoveTo(EntityController target, float speed)
    {
        if (target == null) return;

        var t = target.transform.position;
        t.y = transform.position.y;

        if (agent.isOnNavMesh)
        {
            agent.SetDestination(t);
            agent.speed = speed;
        }
    }

    public void AttackMoveTo(Vector3 position, EntityController target, float speed)
    {
        if (target == null) return;

        AttackMoveTo(position, target.modelPosition.transform.position, speed);
    }

    public void AttackMoveTo(Vector3 position, Vector3 target, float speed)
    {
        var dir = target - position;
        dir.Normalize();

        transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }



    public void MoveControl(bool isStop)
    {
        agent.isStopped = isStop;
    }
}
