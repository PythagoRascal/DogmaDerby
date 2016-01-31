using UnityEngine;
using System.Collections;

public class Box
{
    public Box(Vector3 lower, Vector3 upper)
    {
        LowerCorner = lower;
        UpperCorner = upper;
    }

    public Box(float lowerX, float lowerY, float upperX, float upperY)
            : this(new Vector3(lowerX, lowerY), new Vector3(upperX, upperY))
    { }

    //Lower left
    public Vector3 LowerCorner { get; set; }

    //Upper right
    public Vector3 UpperCorner { get; set; }

    public Vector3 GetRandomVectorInBox()
    {
        var randomX = Random.Range(LowerCorner.x, UpperCorner.x);
        var randomY = Random.Range(LowerCorner.y, UpperCorner.y);

        return new Vector3(randomX, randomY);
    }

    public void FitBox(Box outerBox)
    {
        var lowerX = Mathf.Clamp(LowerCorner.x, outerBox.LowerCorner.x, outerBox.UpperCorner.x);
        var lowerY = Mathf.Clamp(LowerCorner.y, outerBox.LowerCorner.y, outerBox.UpperCorner.y);
        var upperX = Mathf.Clamp(UpperCorner.x, outerBox.LowerCorner.x, outerBox.UpperCorner.x);
        var upperY = Mathf.Clamp(UpperCorner.y, outerBox.LowerCorner.y, outerBox.UpperCorner.y);

        LowerCorner = new Vector3(lowerX, lowerY);
        UpperCorner = new Vector3(upperX, upperY);
    }
}
