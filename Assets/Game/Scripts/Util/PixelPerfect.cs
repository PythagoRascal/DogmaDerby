using UnityEngine;
using System.Collections;

public class PixelPerfect : ScriptBase
{
    private void Update()
    {
        Camera.main.orthographicSize = ((Camera.main.pixelHeight) / (Constants.PPUScale * Constants.PPU)) * 0.5f;
    }
}
