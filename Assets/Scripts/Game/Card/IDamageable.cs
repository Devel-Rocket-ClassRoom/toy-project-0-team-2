using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, Team team);
}

public enum Team
{
    RedTeam,
    BlueTeam,
    Neutrality,
}
