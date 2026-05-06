using UnityEngine;

public class ElixirManager : MonoBehaviour
{
    public bool Gameover;
    private const float StartElixir = 6;
    private const float maxElixir = 10;
    private const float regenRate = 0.35f;
    public float currentElixir;



    private void Start()
    {
        currentElixir = StartElixir;
    }

    private void Update()
    {
        if (Gameover)
        {
            return;
        }

        currentElixir += Time.deltaTime * regenRate;
        currentElixir = Mathf.Clamp(currentElixir, 0, maxElixir);
    }


}
