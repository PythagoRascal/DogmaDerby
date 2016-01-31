using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaitingForPlayerCanvas : ScriptBase {

    private static string PlayerRequiredText = "One more player has to connect";
    private static string PlayersRequiredText = "{0} more players have to connect";

    private Text PlayersRequiredUIElement;

    private void Awake()
    {
        PlayersRequiredUIElement = FindSubObject("PlayersRequiredText").GetComponent<Text>();
    }

    public void UpdateReqiredPlayers(int count)
    {
        if (count == 1)
        {
            PlayersRequiredUIElement.text = PlayerRequiredText;
        }
        else
        {
            PlayersRequiredUIElement.text = string.Format(PlayersRequiredText, count);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
