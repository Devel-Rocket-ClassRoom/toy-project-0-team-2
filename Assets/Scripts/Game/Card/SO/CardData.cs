using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public GameObject cardImage;
    public int elixir;
    public CardDataStructure[] cardDatas;

    public float arrangmentCompletTime;
}

[Serializable]
public struct CardDataStructure
{
    public EntityData entityData;
    public Vector3 positionAdjustment;
}