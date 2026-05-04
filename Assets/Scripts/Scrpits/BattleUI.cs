using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{

    //타이머
    public TextMeshProUGUI Timer;
    public BattleManager battleManager;
    //엘리서
    public TextMeshProUGUI ElixirText;
    public Slider ElixirSlider;
    //크라운카운터
    public TextMeshProUGUI RedCrown;
    private int RedCrownCount = 0;
    private bool DestroyRedTown;

    public TextMeshProUGUI BlueCrown;
    private int BlueCrownCount = 0;
    private bool DestroyBlueTown;    
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
    }


    private void Update()
    {
        battleTime();
        ElixirUI();
        ButtonData();
        CrownCounter();

        //확인용
        if(Input.GetKeyDown(KeyCode.R))
        {
            DestroyRedTown=true;
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            DestroyBlueTown = true;
        }
    }


    //타이머정보표시
    private void battleTime()
    {
        int minutes = battleManager.battleTime / 60;
        int seconds = battleManager.battleTime % 60;

        Timer.text = $"{minutes.ToString("00")}:{seconds.ToString("00")}";
    }

    //엘리서정보표시
    private void ElixirUI()
    {
        ElixirSlider.value = battleManager.currentElixir;
        ElixirText.text = Mathf.FloorToInt(battleManager.currentElixir).ToString();
        
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

            if (battleManager.currentElixir < cost)
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
    private void CrownCounter()
    {
        if (DestroyRedTown)
        {
            RedCrownCount++;
            RedCrown.text = RedCrownCount.ToString();
            if (RedCrownCount >= 3)
            {
                battleManager.Gameover = true;
                Debug.Log("레드팀 승리");
            }
            DestroyRedTown =false;

        }

        if (DestroyBlueTown)
        {
            BlueCrownCount++;
            BlueCrown.text = BlueCrownCount.ToString();
            if (BlueCrownCount >= 3)
            {
                battleManager.Gameover=true;
                Debug.Log("블루팀 승리");
            }
            DestroyBlueTown = false;
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
