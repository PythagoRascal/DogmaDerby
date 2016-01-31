using UnityEngine;
using System.Collections;

public class ScriptBase : MonoBehaviour
{
	protected GameObject FindSubObject(string name)
    {
        foreach (Transform t in transform)
        {
            if (t.name == name)
            {
                return t.gameObject;
            }
        }

        Debug.LogErrorFormat("Could not find object with name {}", name);

        return null;
    }

    protected GameObject FindObject(string name)
    {
        var target = GameObject.Find(name);

        if (target == null)
        {
            Debug.LogErrorFormat("Could not find object with name {}", name);
        }

        return target;
    }
}
