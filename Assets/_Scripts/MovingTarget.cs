using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    public enum MovementPattern
    {
        VerticalBob,      // Moves up and down (Y axis)
        HorizontalSlide,  // Moves left and right relative to camera/plane
        CirclePatrol      // Circles around the spawn point
    }

    [Header("Movement Configuration")]
    [SerializeField] private MovementPattern pattern = MovementPattern.VerticalBob;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private float distance = 0.4f; // Max travel distance from start

    private Vector3 originPosition;
    private float randomOffset;
    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        // Cache the exact spawn position as the center anchor
        originPosition = transform.position;

        // Randomize the starting phase so multiple cubes don't move in sync
        randomOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        float time = (Time.time * speed) + randomOffset;

        switch (pattern)
        {
            case MovementPattern.VerticalBob:
                // Sine wave between (origin) and (origin + distance), staying above the floor
                float yOffset = Mathf.Abs(Mathf.Sin(time)) * distance;
                transform.position = originPosition + new Vector3(0f, yOffset, 0f);
                break;

            case MovementPattern.HorizontalSlide:
                // Smooth back-and-forth slide along local or world X
                float xOffset = Mathf.Sin(time) * distance;
                transform.position = originPosition + (transform.right * xOffset);
                break;

            case MovementPattern.CirclePatrol:
                // Circular motion around spawn point
                float xCircle = Mathf.Cos(time) * distance;
                float zCircle = Mathf.Sin(time) * distance;
                transform.position = originPosition + new Vector3(xCircle, 0f, zCircle);
                break;
        }

        transform.LookAt(mainCam.transform.position, Vector3.up);
        //LookAtCamera();
    }

    private void LookAtCamera()
    {
        // Make the target face the camera/player
        /*
        if (mainCam != null)
        {
            Vector3 directionToCamera = mainCam.transform.position - transform.position;
            directionToCamera.y = 0; // Keep only horizontal rotation
            if (directionToCamera.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToCamera);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }*/
    }

    public void StopMoving()
    {
        enabled = false;
    }
}