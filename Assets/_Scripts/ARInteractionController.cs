using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARInteractionController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera arCamera;
    [SerializeField] private ARRaycastManager raycastManager;
    [SerializeField] private ARPlaneManager planeManager;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject baseArena;
    private GameObject spawnedObject;

    [Header("Shoot Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float launchVelocity = 15f;

    private static List<ARRaycastHit> arHits = new List<ARRaycastHit>();

    private void Awake()
    {
        if (arCamera == null)
            arCamera = Camera.main;
        if (raycastManager == null)
            raycastManager = FindAnyObjectByType<ARRaycastManager>();
        if (planeManager == null)
            planeManager = FindAnyObjectByType<ARPlaneManager>();
    }

    private void Update()
    {
        Vector2 screenPosition;

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            screenPosition = Touchscreen.current.primaryTouch.position.ReadValue();
        }
        else if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            screenPosition = Mouse.current.position.ReadValue();
        }
        else
        {
            return;
        }

        HandleTouch(screenPosition);
    }

    private void HandleTouch(Vector2 screenPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(screenPosition);

        if (raycastManager.Raycast(screenPosition, arHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = arHits[0].pose;
            var position = hitPose.position + (Vector3.up * 0.25f);

            if (spawnedObject == null)
            {
                spawnedObject = Instantiate(baseArena, position, hitPose.rotation);
                GameManager.Instance.ArenaStable();
            }
        }
    }
}