using UnityEngine;

public class DeckContainer : MonoBehaviour
{
    public static DeckContainer Instance;
    public CardData[] Deck;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
