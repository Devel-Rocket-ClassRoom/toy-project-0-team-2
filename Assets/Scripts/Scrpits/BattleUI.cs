using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{

    public Gameendmanager gameendmanager;
    //타이머
    public TextMeshProUGUI Timer;
    public timerManager timerManager;
    //엘리서
    public ElixirManager elixirManager;
    public TextMeshProUGUI ElixirText;
    public Slider ElixirSlider;
    //크라운카운터
    public TextMeshProUGUI RedCrown;
    public int RedCrownCount = 0;

    public TextMeshProUGUI BlueCrown;
    public int BlueCrownCount = 0;


    //카드
    private Color originalColor;
    public TextMeshProUGUI[] CardNames = new TextMeshProUGUI[4];
    public TextMeshProUGUI[] CardElixirs = new TextMeshProUGUI[4];
    public Button[] CardButtons = new Button[4];
    public HandManager handManager;
    public TextMeshProUGUI NextCardNames;
    public TextMeshProUGUI NextCardElixirs;
    //미구현
    private Image nextCard;
    private TextMeshProUGUI RedName;
    private Image imoticonSub;
    private Image imoticonMain;


    private void Start()
    {
        ElixirSlider.minValue = 0;
        ElixirSlider.maxValue = 10;
        originalColor= Color.white;

        EntityManager.onCrounTowerDestroy += CrownCounter;
    }


    private void Update()
    {
        battleTime();
        ElixirUI();
        ButtonData();


        //확인용
    }


    //타이머정보표시
    private void battleTime()
    {
        int minutes = timerManager.battleTime / 60;
        int seconds = timerManager.battleTime % 60;

        Timer.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
    }

    //엘리서정보표시
    private void ElixirUI()
    {
        ElixirSlider.value = elixirManager.currentElixir;
        ElixirText.text = Mathf.FloorToInt(elixirManager.currentElixir).ToString();
    }

    //카드정보표시
    private void ButtonData()
    {
        if (handManager == null)
        {
            return;
        }

        for (int i = 0; i < 4; i++)
        {
            CardData card = handManager.handCards[i];

            if (card == null || card.cardDatas.Length == 0)
            {
                continue;
            }

            EntityData data = card.cardDatas[0].entityData;
            int cost = data.elixir;

            CardNames[i].text = data.name;
            CardElixirs[i].text = cost.ToString();


            ColorBlock cb = CardButtons[i].colors;

            if (elixirManager.currentElixir < cost)
            {
                cb.normalColor = Color.gray;
                cb.highlightedColor = Color.gray;
                cb.pressedColor = Color.gray;
                cb.selectedColor = Color.gray;
            }
            else
            {
                cb.normalColor = Color.white;
                cb.highlightedColor = Color.white;
                cb.pressedColor = Color.white;
                cb.selectedColor = Color.white;
            }

            CardButtons[i].colors = cb;
        }
        UpdateNextCardUI();
    }


    //크라운정보표시
    private void CrownCounter(Team team)
    {
        if (team == Team.BlueTeam)
        {
            RedCrownCount++;
            RedCrown.text = RedCrownCount.ToString();
        }

        if (team == Team.RedTeam)
        {
            BlueCrownCount++;
            BlueCrown.text = BlueCrownCount.ToString();
        }
    }

    //넥스트카드정보표시
    private void UpdateNextCardUI()
    {
    
        CardData nextCard = handManager.deckQueue.Peek();
        EntityData data = nextCard.cardDatas[0].entityData;

        NextCardNames.text = data.name;
        NextCardElixirs.text = data.elixir.ToString();
    }



}
