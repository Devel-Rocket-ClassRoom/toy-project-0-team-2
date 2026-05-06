using UnityEngine;

[CreateAssetMenu(fileName = "SpecialData", menuName = "Scriptable Objects/SpecialData")]
public class SpecialData : ScriptableObject
{
    [Header("디버프")]
    public float knuckbackTile;
    public float freezeTime;
    public float speedDownRate;
    public float attackSpeedDownRate;

    [Header("버프")]
    public float speedUpRate;
    public float attackSpeedUpRate;

    [Header("스프린트")]
    public bool hasSprint;
    public float springPrepareTime;
    public float sprintSpeed;
    public AttackData sprintAttack;

    [Header("돌진")]
    public bool hasCharge;
    public float minChargeRange;
    public float maxChargeRange;
    public float chargePrepareTime;
    public float chargeSpeed;
    public AttackData chargeAttack;

    [Header("사망시 효과")]
    public bool hasDeathrattle;
    public CardData deathrattleEntity;

    [Header("소환")]
    public bool hasSummon;
    public CardData summonEntity;
    public float summonInterval;
}
