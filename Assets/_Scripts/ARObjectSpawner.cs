using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

//[RequireComponent(typeof(ARRaycastManager))]
public class ARObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectToSpawn;
    [SerializeField] private bool spawnOnlyOnce = true;

    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        raycastManager = FindAnyObjectByType<ARRaycastManager>();
    }

    private void Update()
    {
        // 1. Get touch or mouse position using the New Input System
        Vector2 screenPosition;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        // Fallback for testing in the Unity Editor with mouse clicks
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            screenPosition = Mouse.current.position.ReadValue();
        }
        else
        {
            return;
        }

        // 2. Prevent extra spawns if spawnOnlyOnce is enabled
        if (spawnOnlyOnce && spawnedObject != null) return;

        // 3. Perform AR Raycast against detected planes
        if (raycastManager.Raycast(screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);
            }
            else if (!spawnOnlyOnce)
            {
                Instantiate(objectToSpawn, hitPose.position, hitPose.rotation);
            }
        }
    }

    public void ResetSpawner()
    {
        if (spawnedObject != null)
        {
            Destroy(spawnedObject);
            spawnedObject = null;
        }
    }
}