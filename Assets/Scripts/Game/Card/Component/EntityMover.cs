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

    public void MoveTo(EntityController target, float speed)
    {
        if (target == null) return;

        MoveTo(target.transform.position, speed);
    }

    public void MoveTo(Vector3 target, float speed)
    {
        if (agent.isOnNavMesh)
        {
            agent.SetDestination(target);
            agent.speed = speed;
        }
        else
        {
            // 디버그를 통해 어떤 상태인지 파악 가능
            Debug.LogWarning($"{gameObject.name} : Agent is not ready! {agent.isActiveAndEnabled} {agent.isOnNavMesh}");
        }
        

    }



    public void MoveControl(bool isStop)
    {
        agent.isStopped = isStop;
    }
}
