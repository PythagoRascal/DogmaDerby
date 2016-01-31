using UnityEngine;
using System.Collections;

public class ParticleGenerator : MonoBehaviour {

    public GameObject particleReference;

    private bool _hasGenerated = false;
	
	// Update is called once per frame
	public void Generate (float radius, int ringCount, int particleCount, Color particleColor) {

        float radStep = 360.0f / particleCount;

        for (int r = 0; r < ringCount; r++)
        {
            for (int p = 0; p < particleCount; p++)
            {
                float stepWithShift = (radStep * p) + ((radStep / 2) * (r % 2));
                float ringRadius = (radius / ringCount) * (r + 1);
                Vector3 destination = gameObject.transform.position + (Quaternion.Euler(0, 0, stepWithShift) * new Vector3(ringRadius, 0));

                GameObject particle = Instantiate(particleReference, destination, Quaternion.identity) as GameObject;
                particle.GetComponent<SpriteRenderer>().color = particleColor;
                particle.transform.SetParent(gameObject.transform);
            }
        }

        _hasGenerated = true;
	}

    void Update()
    {
        if (_hasGenerated && gameObject.transform.childCount == 0)
            Destroy(gameObject);
    }
}
