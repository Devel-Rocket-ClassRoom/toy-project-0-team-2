using UnityEngine;

public abstract class EntityController : RootController
{
    public EntityData cardData;
    public EntityType entityType;
    public Team team;
    public Transform modelPosition;

    protected float searchInterval = 0.2f;
    protected float lastSearchTime;

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
        transform.position = point;

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
