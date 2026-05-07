using UnityEngine;
using System.Collections;
public class timerManager : MonoBehaviour
{
    public Gameendmanager gameendmanager;
    public int battleTime = 180;

     private void Start()
    {
        StartCoroutine(BattleTimer());
    }


    private IEnumerator BattleTimer()
    {
        while (battleTime > 0 && !gameendmanager.Gameover)
        {
            yield return new WaitForSeconds(1f);
            battleTime--;
        }

        gameendmanager.Gameover = true;
    }

}
