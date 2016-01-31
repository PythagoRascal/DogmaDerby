using UnityEngine;
using System.Collections;

public class DepthFix : ScriptBase
{
	private void Update()
    {
        var pos = transform.position;
        pos.z = pos.y / (Camera.main.pixelHeight / 2);
        transform.position = pos;
    }
}
