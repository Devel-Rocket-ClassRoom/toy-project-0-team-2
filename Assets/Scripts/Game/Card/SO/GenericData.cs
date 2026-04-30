using UnityEngine;

[CreateAssetMenu(fileName = "GenericData", menuName = "Scriptable Objects/GenericData")]
public class GenericData : ScriptableObject
{
    public int elixir;
    public Vector3 positionAdjustment;

    public GameObject model;
}
