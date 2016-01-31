using UnityEngine;
using System.Collections;

public static class Util
{
    public static bool CalculateRandomEvent(float probability)
    {
        var random = Random.Range(0.0f, 1.0f);
        return random < probability || probability == 1;
    }

    public static Box CalculateWorldBox(float offset)
    {
        var lowerX = /*Camera.main.pixelWidth*/ 1280 / -2 + offset;
        var lowerY = /*Camera.main.pixelHeight*/ 720 / -2 + offset;
        var upperX = /*Camera.main.pixelWidth*/ 1280 / 2 - offset;
        var upperY = /*Camera.main.pixelHeight*/ 720 / 2 - offset;

        return new Box(lowerX, lowerY, upperX, upperY);
    }
}
