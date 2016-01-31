using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TeamCountSelectionButton : ScriptBase
{

    [SerializeField]
    private int _teamCount;

    private void OnMouseDown()
    {
        PlayerPrefs.SetInt("TeamCount", _teamCount);
        SceneManager.LoadScene(1);
        GetComponent<AudioSource>().Play();
    }
}
