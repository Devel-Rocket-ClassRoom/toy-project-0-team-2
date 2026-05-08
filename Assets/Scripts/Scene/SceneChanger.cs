using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        var deck = DeckContainer.Instance.Deck;

        for (int i = 0; i < deck.Length; i++)
        {
            if (deck[i] == null) return;
            Debug.Log(i);
        }

        SceneManager.LoadScene(sceneName);
    }
}