using UnityEngine;
using UnityEngine.AI;

public abstract class EntityController : RootController
{
    public EntityData cardData;
    public EntityType entityType;
    public Transform modelPosition;

    protected float searchInterval = 0.2f;
    protected float lastSearchTime;
    protected float lastChargeTime;
    protected bool isChargeEnd;


    public float health { get; protected set; }

    protected Coroutine runningCoroutine;


    public float size;

    public virtual void Init(EntityData cardData, Vector3 point, Team team)
    {
        Init(cardData, point, cardData.model, team);
    }

    public virtual void Init(EntityData cardData, Vector3 point, GameObject model, Team team)
    {
        this.cardData = cardData;
        this.team = team;
        if ((cardData.DefenseData.entityType & EntityType.Aerial) != 0)
        {
            modelPosition.position += Vector3.up * 3;
            transform.GetComponent<NavMeshAgent>().areaMask |= 0xFF;
        }
        transform.position = point;

        transform.forward = team == Team.RedTeam ? -Vector3.forward : Vector3.forward;

        if (cardData != null && cardData.DefenseData != null)
        {
            entityType = cardData.DefenseData.entityType;
            size = cardData.DefenseData.radius;
        }

        if (model != null)
        {
            var attack = Instantiate(model, modelPosition);
        }
    }



}
