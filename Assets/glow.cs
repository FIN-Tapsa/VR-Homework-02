using UnityEngine;

public class glow : MonoBehaviour
{

    public Renderer cubeRenderer;
    public Color baseColor = Color.red;
    public float glowIntensity = 2f;
    public float flashSpeed = 2f;

    private Material cubeMaterial;

    void Start()
    {
        if (cubeRenderer == null)
            cubeRenderer = GetComponent<Renderer>();

        cubeMaterial = cubeRenderer.material;
        cubeMaterial.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        float emissionStrength = (Mathf.Sin(Time.time * flashSpeed) + 1) / 2 * glowIntensity;
        Color emissionColor = baseColor * Mathf.LinearToGammaSpace(emissionStrength);
        cubeMaterial.SetColor("_EmissionColor", emissionColor);
    }
}


