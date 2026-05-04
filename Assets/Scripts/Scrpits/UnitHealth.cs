using UnityEngine;

public class UnitHealth : MonoBehaviour
{
    public EntityData entityData;

    private float currentHP;
    private UnitHealthBarUI healthBarUI;

    public void Init(EntityData data, UnitHealthBarUI hpBar)
    {
        entityData = data;
        healthBarUI = hpBar;

        currentHP = entityData.DefenseData.health;
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
        currentHP = Mathf.Clamp(currentHP, 0, entityData.DefenseData.health);

        healthBarUI.SetHealth(currentHP);

        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }
}