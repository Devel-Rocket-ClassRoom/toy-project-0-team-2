using UnityEngine;

[CreateAssetMenu(fileName = "SpecialData", menuName = "Scriptable Objects/SpecialData")]
public class SpecialData : ScriptableObject
{
    public float knuckbackTile;
    public float freezeTime;

    [Header("스프린트")]
    public bool hasSprint;
    public float springPrepareTime;
    public float sprintSpeed;
    public AttackData sprintAttack;

    [Header("돌진")]
    public bool hasCharge;
    public float chargePrepareTime;
    public float chargeSpeed;
    public AttackData chargeAttack;

    [Header("사망시 효과")]
    public bool hasDeathrattle;
    public EntityData summonEntity;
}
