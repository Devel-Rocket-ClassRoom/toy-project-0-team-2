using System.IO;
using Newtonsoft.Json;
using UnityEngine;

public class DeckContainer : MonoBehaviour
{
    public static DeckContainer Instance;
    public CardData[] Deck;

    private static string SavePath => Path.Combine(Application.persistentDataPath, "deck.json");

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

    public void SaveDeck(CardData[] deck)
    {
        var names = new string[deck.Length];
        for (int i = 0; i < deck.Length; i++)
        {
            names[i] = deck[i] != null ? deck[i].cardName : "";
        }
        File.WriteAllText(SavePath, JsonConvert.SerializeObject(names));
    }

    public string[] LoadDeckNames()
    {
        if (!File.Exists(SavePath))
            return null;

        try
        {
            return JsonConvert.DeserializeObject<string[]>(File.ReadAllText(SavePath));
        }
        catch
        {
            return null;
        }
    }
}
