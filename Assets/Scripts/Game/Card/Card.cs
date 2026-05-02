using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Scriptable Objects/Card")]
public class Card : ScriptableObject
{
    public CardDataStructure[] cardDatas;

    public float arrangmentCompletTime;
}

[Serializable]
public struct CardDataStructure
{
    public CardData cardData;
    public Vector3 positionAdjustment;
}