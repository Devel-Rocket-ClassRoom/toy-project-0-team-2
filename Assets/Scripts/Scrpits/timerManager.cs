using UnityEngine;
using System.Collections;
public class timerManager : MonoBehaviour
{
    public bool Gameover;
    public int battleTime = 180;

 private void Start()
    {
        StartCoroutine(BattleTimer());
    }


    private IEnumerator BattleTimer()
    {
        while (battleTime > 0 && !Gameover)
        {
            yield return new WaitForSeconds(1f);
            battleTime--;
        }

        Gameover = true;
    }

}
