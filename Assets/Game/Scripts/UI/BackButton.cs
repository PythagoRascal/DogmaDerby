using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BackButton : MonoBehaviour
{
    private void OnMouseDown()
    {
        GetComponent<AudioSource>().Play();
        SceneManager.LoadScene(0);
    }
}
