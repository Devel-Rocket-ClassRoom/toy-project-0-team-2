using UnityEngine;

public abstract class EntityController : MonoBehaviour
{
    public EntityType entityType;
    public Team team;
    public Transform modelPosition;

    protected float searchInterval = 0.2f;
    protected float lastSearchTime;

    public float size;

    public virtual void Init(CardData cardData)
    {
        entityType = cardData.DefenseData.entityType;
        size = cardData.DefenseData.radius;
        Instantiate(cardData.model, modelPosition);
    }


    protected abstract void Move();
    protected abstract void LockOn();
    protected abstract void CheckAttackRange();
    protected abstract void Attack();
}
