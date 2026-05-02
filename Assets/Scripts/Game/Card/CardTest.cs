using UnityEngine;

public class CardTest : MonoBehaviour
{
    public Card UnitCard;
    public Card TowerCard;
    public Card SpellCard;
    public Card PrincessCard;
    public CardArrangementManager manager;
    public LayerMask layerMask;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                manager.Arrangement(UnitCard, Team.RedTeam, hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                manager.Arrangement(UnitCard, Team.BlueTeam, hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
            manager.Arrangement(TowerCard, Team.RedTeam, new Vector3(0, 1, 10));
        if (Input.GetKeyDown(KeyCode.Alpha4))
            manager.Arrangement(TowerCard, Team.BlueTeam, new Vector3(0, 1, -10));

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                manager.Arrangement(SpellCard, Team.RedTeam, hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                manager.Arrangement(SpellCard, Team.BlueTeam, hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                manager.Arrangement(PrincessCard, Team.RedTeam, hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                manager.Arrangement(PrincessCard, Team.BlueTeam, hit.point);
            }
        }
    }
}
