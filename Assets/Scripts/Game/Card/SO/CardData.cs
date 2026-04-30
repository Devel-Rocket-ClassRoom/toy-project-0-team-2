using System;
using UnityEngine;

public abstract class CardData : ScriptableObject
{
    public int elixir;
    public Vector3 positionAdjustment;

    public AttackData AttackData;
    public DefenseData DefenseData;
    public SpecialData SpecialData;

    public GameObject model;
    public float activateWaitTime;
}

[Flags]
public enum EntityType
{
    Nothing = 0,
    Ground = 1, 
    Aerial = 1 << 1, 
    Tower = 1 << 2, 
    CrownTower = 1 << 3,
    Everything = Ground | Aerial | Tower | CrownTower,
}