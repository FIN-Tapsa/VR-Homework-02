using UnityEngine;

public class flashingChildren : MonoBehaviour
{
    public float glowIntensity = 2f;
    public float flashSpeed = 2f;
    public Color baseColor = Color.red;

    void Update()
    {
        float emissionStrength = (Mathf.Sin(Time.time * flashSpeed) + 1) / 2 * glowIntensity;
        Color emissionColor = baseColor * Mathf.LinearToGammaSpace(emissionStrength);
        
        foreach (Transform child in transform)
        {
            Renderer renderer = child.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", emissionColor);
            }
        }
    }
}