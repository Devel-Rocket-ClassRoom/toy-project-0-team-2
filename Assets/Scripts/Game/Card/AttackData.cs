using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/AttackData")]
public class AttackData : ScriptableObject
{
    public float damage;
    public float attackInterval;
    public float firstAttackInterval;
    public float loadTime;
    public float attackRange;
    public AttackTarget attackTarget;

}

public enum AttackTarget
{
    Ground, GroundAndAerial, Tower,
}