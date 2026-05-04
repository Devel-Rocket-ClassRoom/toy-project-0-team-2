using UnityEngine;

public class CardTest : MonoBehaviour
{
    public CardData Card1;
    public CardData Card2;
    public CardData Card3;
    public CardData Card4;
    public CardData Card5;
    public CardData Card6;
    public CardData Card7;
    public CardData Card8;

    private CardData selectedCard;

    public CardArrangementManager manager;
    public LayerMask layerMask;

    private Camera cam;

    private void Awake()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        SelectCard();

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (selectedCard == null) return;

            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                manager.Arrangement(selectedCard, Team.BlueTeam, hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (selectedCard == null) return;

            if (Physics.Raycast(ray, out RaycastHit hit, layerMask))
            {
                manager.Arrangement(selectedCard, Team.RedTeam, hit.point);
            }
        }
    }


    private void SelectCard()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { selectedCard = Card1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { selectedCard = Card2; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { selectedCard = Card3; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { selectedCard = Card4; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { selectedCard = Card5; }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { selectedCard = Card6; }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { selectedCard = Card7; }
        if (Input.GetKeyDown(KeyCode.Alpha8)) { selectedCard = Card8; }
    }
}
