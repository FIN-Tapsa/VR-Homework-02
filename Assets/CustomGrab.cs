using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CustomGrab : MonoBehaviour
{
    CustomGrab otherHand = null; // Reference to the other hand's CustomGrab script
    public List<Transform> nearObjects = new List<Transform>(); // List of nearby grabbable objects
    public Transform grabbedObject = null; // Currently grabbed object
    public InputActionReference action; // Input action for grabbing
    bool grabbing = false;
    private Vector3 lastPosition; // Last frame's position for delta calculation
    private Quaternion lastRotation; // Last frame's rotation for delta calculation
    public bool doubleSpeedRotation = false; // Toggle for extra credit feature

    private void Start()
    {
        action.action.Enable();

        // Find the other controller's CustomGrab script
        foreach (CustomGrab c in transform.parent.GetComponentsInChildren<CustomGrab>())
        {
            if (c != this)
                otherHand = c;
        }
    }

    void Update()
    {
        grabbing = action.action.IsPressed();

        if (grabbing)
        {
            // Grab the nearest object or the object held by the other hand
            if (!grabbedObject)
                grabbedObject = nearObjects.Count > 0 ? nearObjects[0] : otherHand.grabbedObject;

            if (grabbedObject)
            {
                // Compute the position and rotation deltas from the last frame
                Vector3 deltaPosition = transform.position - lastPosition;
                Quaternion deltaRotation = transform.rotation * Quaternion.Inverse(lastRotation);

                // If both hands are grabbing the same object
                if (otherHand && otherHand.grabbedObject == grabbedObject)
                {
                    Vector3 otherDeltaPosition = otherHand.transform.position - otherHand.lastPosition;
                    Quaternion otherDeltaRotation = otherHand.transform.rotation * Quaternion.Inverse(otherHand.lastRotation);

                    // Combine translations and rotations from both controllers
                    Vector3 combinedTranslation = (deltaPosition + otherDeltaPosition) / 2.0f;
                    Quaternion combinedRotation = deltaRotation * otherDeltaRotation;

                    // Apply double-speed rotation if enabled
                    if (doubleSpeedRotation)
                    {
                        combinedRotation = Quaternion.Slerp(Quaternion.identity, combinedRotation, 2.0f);
                    }

                    grabbedObject.position += combinedTranslation;
                    grabbedObject.rotation = combinedRotation * grabbedObject.rotation;
                }
                else
                {
                    // Apply single-hand movement
                    if (doubleSpeedRotation)
                    {
                        deltaRotation = Quaternion.Slerp(Quaternion.identity, deltaRotation, 2.0f);
                    }

                    grabbedObject.position += deltaPosition;
                    grabbedObject.rotation = deltaRotation * grabbedObject.rotation;
                }
            }
        }
        else if (grabbedObject)
        {
            // Release the object if the grab button is released
            grabbedObject = null;
        }

        // Save the current position and rotation for delta calculations in the next frame
        lastPosition = transform.position;
        lastRotation = transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Detect nearby grabbable objects
        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Add(t);
    }

    private void OnTriggerExit(Collider other)
    {
        // Remove objects that are no longer nearby
        Transform t = other.transform;
        if (t && t.tag.ToLower() == "grabbable")
            nearObjects.Remove(t);
    }
}
