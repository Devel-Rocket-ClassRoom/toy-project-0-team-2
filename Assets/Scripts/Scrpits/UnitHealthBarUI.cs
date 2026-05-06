using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitHealthBarUI : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 2f, 0);

    public TextMeshProUGUI unitHP;
    public Slider HPBar;

    private Camera cam;
    private UnitController unit;
    private float maxHP;

    private void Awake()
    {
        cam = Camera.main;

        if (HPBar == null)
        {
            HPBar = GetComponent<Slider>();
        }
    }

    public void Init(UnitController unit)
    {
        this.unit = unit;
        target = unit.transform;

        maxHP = unit.health;

        HPBar.minValue = 0;
        HPBar.maxValue = maxHP;
        HPBar.value = maxHP;

        unitHP.text = maxHP.ToString("0");
    }

    private void LateUpdate()
    {
        if (target == null || unit == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;

        UpdateUI();
    }

    private void UpdateUI()
    {
        float currentHP = unit.health;

        unitHP.text = currentHP.ToString("0");
        HPBar.value = currentHP;
    }
}