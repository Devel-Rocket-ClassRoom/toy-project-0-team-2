using TMPro;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class EntityMover : MonoBehaviour
{
    private NavMeshAgent agent;

    private Team team;
    public static float VerticalMidLine;
    public static float HorizontalMidLine;
    public static float ArenaTowerLine;
    public static float RoadLine;
    private static bool isInit = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (!isInit)
        {
            VerticalMidLine = CardArrangementManager.Instance.Mid.position.x;
            HorizontalMidLine = CardArrangementManager.Instance.Mid.position.z;
            Vector3 arena = CardArrangementManager.Instance.RedArena[1].position;
            ArenaTowerLine = Mathf.Abs(arena.z - HorizontalMidLine);
            RoadLine = Mathf.Abs(arena.x - VerticalMidLine);

            Debug.Log(VerticalMidLine);
            Debug.Log(HorizontalMidLine);
            Debug.Log(ArenaTowerLine);

            isInit = true;
        }
    }

    public void Init(Team team)
    {
        this.team = team;
    }

    public void UnitMoveTo(EntityController target, float speed)
    {
        Vector3 t = Vector3.zero;
        if (target != null)
        {
            t = target.transform.position;
            t.y = transform.position.y;
        }
        else
        {
            int reverse = (team == Team.RedTeam) ? -1 : 1;

            if (transform.position.x > VerticalMidLine)
                t.x = VerticalMidLine + RoadLine;
            else
                t.x = VerticalMidLine - RoadLine;

            float relativeZ = (transform.position.z - HorizontalMidLine) * reverse;

            float targetRelativeZ = 0;

            if (relativeZ < -ArenaTowerLine - 0.5f)
            {
                targetRelativeZ = -ArenaTowerLine;
            }
            else if (relativeZ < -0.5f)
            {
                targetRelativeZ = 0;
            }
            else if (relativeZ < ArenaTowerLine- 0.5f)
            {
                targetRelativeZ = ArenaTowerLine;
            }
            else
            {
                t.x = VerticalMidLine;
                targetRelativeZ = ArenaTowerLine * 2;
            }

            t.z = HorizontalMidLine + (targetRelativeZ * reverse);
        }

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
