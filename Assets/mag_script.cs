
using UnityEngine;

public class mag_script : MonoBehaviour
{      public Camera mainCamera; // Assign your main camera in the Inspector
    public Camera magnifierCamera; // Assign the magnifying glass camera
    public Transform magnifyingGlass; // Assign the magnifying glass transform in the Inspector
    public float minFOV = 20f; // Minimum FOV for strong magnification
    public float maxFOV = 60f; // Maximum FOV for weak magnification
    public float baseDistance = 1.0f; // Reference distance for magnification
    
    void Update()
    {
        if (mainCamera != null && magnifierCamera != null && magnifyingGlass != null)
        {
            // Match rotation but maintain Z-axis for proper image orientation
            Quaternion targetRotation = mainCamera.transform.rotation;
            transform.rotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, magnifyingGlass.eulerAngles.z);
            
            // Adjust FOV based on distance to main camera
            float distance = Vector3.Distance(magnifyingGlass.position, mainCamera.transform.position);
            magnifierCamera.fieldOfView = Mathf.Clamp(maxFOV * (distance / baseDistance), minFOV, maxFOV);
        }
    }
}
