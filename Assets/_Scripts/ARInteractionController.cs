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

    [Header("Spawn Settings")]
    [SerializeField] private GameObject cubePrefab;

    [Header("Shoot Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float launchVelocity = 15f;
    [SerializeField] private LayerMask cubeLayer; // Layer assigned to cubes

    private static List<ARRaycastHit> arHits = new List<ARRaycastHit>();

    private void Awake()
    {
        if (arCamera == null)
            arCamera = Camera.main;
        if (raycastManager == null)
            raycastManager = FindAnyObjectByType<ARRaycastManager>();
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

        // 1. PRIORITY CHECK: Did we tap an existing cube?
        if (Physics.Raycast(ray, out var hitInfo, 100f, cubeLayer))
        {
            // Direct hit on a cube -> Shoot projectile towards it
            ShootAtTarget(ray.direction);
            return;
        }

        // 2. FALLBACK CHECK: Did we tap an empty area on an AR Plane?
        if (raycastManager.Raycast(screenPosition, arHits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = arHits[0].pose;
            Instantiate(cubePrefab, hitPose.position, hitPose.rotation);
        }
    }

    private void ShootAtTarget(Vector3 direction)
    {
        Vector3 origin = arCamera.transform.position + (arCamera.transform.forward * 0.2f);
        GameObject projectile = Instantiate(projectilePrefab, origin, Quaternion.LookRotation(direction));
        
        if (projectile.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = direction * launchVelocity;
        }
    }
}