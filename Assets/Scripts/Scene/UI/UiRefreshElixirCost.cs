using TMPro;
using UnityEngine;

public class UiRefreshElixirCost : MonoBehaviour
{
    public TextMeshProUGUI totalElixirCostText;
    public TextMeshProUGUI[] elixirCostTexts;
    public GameObject CardDeck;

    private void Update()
    {
        elixirCostTexts = CardDeck.GetComponentsInChildren<TextMeshProUGUI>();
        RefreshElixirCost();
    }

    public void RefreshElixirCost()
    {
        float totalCost = 0;
        foreach (var text in elixirCostTexts)
        {
            if (int.TryParse(text.text, out int cost))
            {
                totalCost += (float)cost;
            }
        }

        if (totalCost <= 0 || elixirCostTexts.Length == 0)
        {
            totalElixirCostText.text = "Average Elixir cost: 0.0";
            return;
        }
        totalElixirCostText.text = $"Average Elixir cost: {totalCost / elixirCostTexts.Length:F1}";
    }
}