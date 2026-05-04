using UnityEngine;

public class CardTest : MonoBehaviour
{
    public Card UnitCard;
    public Card TowerCard;
    public CardArrangementManager manager;

    private void Awake()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            manager.Arrangement(UnitCard.cardDatas, Team.RedTeam, new Vector3(0, 1, 10));
        if (Input.GetKeyDown(KeyCode.Alpha2))
            manager.Arrangement(UnitCard.cardDatas, Team.BlueTeam, new Vector3(0, 1, -10));
        if (Input.GetKeyDown(KeyCode.Alpha3))
            manager.Arrangement(TowerCard.cardDatas, Team.RedTeam, new Vector3(0, 1, 10));
        if (Input.GetKeyDown(KeyCode.Alpha4))
            manager.Arrangement(TowerCard.cardDatas, Team.BlueTeam, new Vector3(0, 1, -10));
    }
}
