using UnityEngine;
using System.Collections;

public class RankingController : ScriptBase
{
    public DigitDisplayManager teamRed;
    public DigitDisplayManager teamGreen;
    public DigitDisplayManager teamBlue;
    public DigitDisplayManager teamPurple;

    private void Start()
    {
        var team0 = PlayerPrefs.GetInt("0");
        var team1 = PlayerPrefs.GetInt("1");
        var team2 = PlayerPrefs.GetInt("2");
        var team3 = PlayerPrefs.GetInt("3");

        teamRed.displayNumber = team0;
        teamGreen.displayNumber = team1;
        teamBlue.displayNumber = team2;
        teamPurple.displayNumber = team3;

        PlayerPrefs.DeleteAll();
    }
}
