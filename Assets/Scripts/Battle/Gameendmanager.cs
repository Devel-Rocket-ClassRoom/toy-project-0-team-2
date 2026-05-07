using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class Gameendmanager : MonoBehaviour
{
    public bool Gameover;
    public BattleUI battleUI;
    public timerManager timerManager;

    public TextMeshProUGUI VictoryTeam;
    public GameObject gameObject;

    private void Start()
    {
        gameObject.SetActive(Gameover);
    }

    private void Update()
    {
        Win();
    }

    private void Win()
    {
        if (battleUI.RedCrownCount >= 3)
        {
            Gameover = true;
            gameObject.SetActive(Gameover);
            VictoryTeam.text = "Red Team Victory!";
            VictoryTeam.color = Color.red;
            VictoryTeam.transform.localPosition = new Vector3(-235, 0, 0);
        }
        else if (battleUI.BlueCrownCount >= 3)
        {
            Gameover = true;
            gameObject.SetActive(Gameover);
            VictoryTeam.text = ("Blue Team Victory!");
            VictoryTeam.color = Color.blue;
            VictoryTeam.transform.localPosition = new Vector3(235, 0, 0);
        }
       

        if (timerManager.battleTime == 0)
        {
            if (battleUI.RedCrownCount > battleUI.BlueCrownCount)
            {
                Gameover = true;
                gameObject.SetActive(Gameover);
                VictoryTeam.text = ("Red Team Victory!");
                VictoryTeam.color = Color.red;
                VictoryTeam.transform.localPosition = new Vector3(-235, 0, 0);
            }
            else if (battleUI.RedCrownCount < battleUI.BlueCrownCount)
            {
                Gameover = true;
                gameObject.SetActive(Gameover);
                VictoryTeam.text = ("Blue Team Victory!");
                VictoryTeam.color = Color.blue;
                VictoryTeam.transform.localPosition = new Vector3(235, 0, 0);
            }
            else
            {
                Gameover = true;
                gameObject.SetActive(Gameover);
                VictoryTeam.text = ("Draw!");
                VictoryTeam.color = Color.violet;

            }
        }
    }

}
