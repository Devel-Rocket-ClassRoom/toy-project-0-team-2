using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    public CardData cardData;
    public EntityType entityType;
    public Team team;
    public Transform modelPosition;

    protected float searchInterval = 0.2f;
    protected float lastSearchTime;


    protected Coroutine runningCoroutine;


    public float size;

    public virtual void Init(CardData cardData, Vector3 point)
    {
        Init(cardData, point, cardData.model);
    }

    public virtual void Init(CardData cardData, Vector3 point, GameObject model)
    {
        this.cardData = cardData;
        transform.position = point;

        if (cardData.DefenseData != null)
        {
            entityType = cardData.DefenseData.entityType;
            size = cardData.DefenseData.radius;
        }

        if (model != null)
            Instantiate(model, modelPosition);
    }


    protected abstract void Move();
    protected abstract void LockOn();
    protected abstract void CheckAttackRange();
    protected abstract void Attack();

    public abstract void TakeDamage(float damage, Team team);
}
