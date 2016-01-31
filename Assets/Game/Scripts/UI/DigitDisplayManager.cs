using UnityEngine;
using System.Collections;

public class DigitDisplayManager : MonoBehaviour {

    [SerializeField]
    private Sprite[] _numberSprites;
    [SerializeField]
    private SpriteRenderer[] _targetSpriteReferences;
    public int displayNumber;

	void Update () {
        int restNumber = displayNumber;
        for (int i = 0; i < _targetSpriteReferences.Length; i++)
        {
            int digit = restNumber % 10;
            _targetSpriteReferences[i].sprite = _numberSprites[digit];

            restNumber = (restNumber - digit) / 10;
        }
	}
}
