using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBarUI : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);

    public TextMeshProUGUI unitHP;
    public Slider HPBar;
    public Image fillImage;

    private Camera cam;
    private UnitController unit;
    private float maxHP;
    public TowerController tower;


    public void InitTower(TowerController tower, Team team)
    {
        this.tower = tower;
        target = tower.transform;

        cam = Camera.main;

        HPBar.minValue = 0;
        HPBar.maxValue = tower.cardData.DefenseData.health;

        if (fillImage != null)
        {
            fillImage.color =
                team == Team.RedTeam
                ? Color.red
                : Color.blue;
        }
    }
    public void Init(UnitController unit, Team team)
    {
        this.unit = unit;
        target = unit.transform;
        cam = Camera.main;

        maxHP = unit.health;

        HPBar.minValue = 0;
        HPBar.maxValue = maxHP;
        HPBar.value = maxHP;


        if (fillImage != null)
        {
            if (team == Team.RedTeam)
            {
                fillImage.color = Color.red;
            }
            else
            {
                fillImage.color = Color.blue;
            }
        }
        unitHP.text = maxHP.ToString("0");
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position =
            cam.WorldToScreenPoint(target.position + offset);

        if (unit != null)
        {
            HPBar.value = unit.health;
            unitHP.text = unit.health.ToString("0");
        }
        else if (tower != null)
        {
            HPBar.value = tower.health;
            unitHP.text = tower.health.ToString("0");
        }
    }
}