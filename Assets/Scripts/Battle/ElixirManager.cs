using UnityEngine;

public class ElixirManager : MonoBehaviour
{
    public Gameendmanager gameendmanager;
    private const float StartElixir = 6;
    private const float maxElixir = 10;
    private float regenRate = 0.35f;
    public float currentElixir;
    public float elixerMult = 1f;
    public timerManager timerManager;


    private void Start()
    {
        currentElixir = StartElixir;
    }

    private void Update()
    {
        if (gameendmanager.Gameover)
        {
            return;
        }
        if (timerManager.battleTime<=60)
        {
            regenRate = 0.7f;
        }
        currentElixir += Time.deltaTime * regenRate * elixerMult;
        currentElixir = Mathf.Clamp(currentElixir, 0, maxElixir);
    }


}
