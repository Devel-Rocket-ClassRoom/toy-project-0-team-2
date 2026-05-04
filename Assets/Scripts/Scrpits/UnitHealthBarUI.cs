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
    private float currentHP;
    private float maxHP;

    private void Awake()
    {
        cam = Camera.main;
    }

    public void Init(Transform unitTarget, EntityData entityData)
    {
        target = unitTarget;

        maxHP = entityData.DefenseData.health;
        currentHP = maxHP;

        HPBar.minValue = 0;
        HPBar.maxValue = maxHP;
        HPBar.value = currentHP;

        UpdateUI();
    }

    private void LateUpdate()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 screenPos = cam.WorldToScreenPoint(target.position + offset);
        transform.position = screenPos;
    }

    public void SetHealth(float hp)
    {
        currentHP = Mathf.Clamp(hp, 0, maxHP);
        UpdateUI();
    }

    private void UpdateUI()
    {
        unitHP.text = currentHP.ToString("0");
        HPBar.value = currentHP;
    }
}