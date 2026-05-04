using UnityEngine;

public class RootController : MonoBehaviour
{
    [SerializeField]
    protected EntityState state;
    public Team team;
    protected float lastSummonTime;
}
