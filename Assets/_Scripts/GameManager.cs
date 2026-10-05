using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public static event Action<int> OnScoreChanged;
    public static event Action<bool> ConfettiStatusChanged;

    private int score;

    #region Gameobjects

    [SerializeField] private ARPlaneManager planeManager;

    [SerializeField] private GameObject fireBtn;
    [SerializeField] private GameObject crosshair;

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (planeManager == null)
            planeManager = FindAnyObjectByType<ARPlaneManager>();
    }

    private void Start()
    {
        crosshair.SetActive(false);
    }

    public void ArenaStable()
    {
        StopPlaneDetection();
        fireBtn.SetActive(true);
        crosshair.SetActive(true);
    }

    private void StopPlaneDetection()
    {
        if (planeManager == null)
            return;

        planeManager.enabled = false;
        foreach (var plane in planeManager.trackables)
        {
            plane.gameObject.SetActive(false);
        }
    }

    public void UpdateScore()
    {
        score += 10;
        OnScoreChanged?.Invoke(score);

        if (score >= 30)
            GameEnd();
    }

    private void GameEnd()
    {
        fireBtn.SetActive(false);
        crosshair.SetActive(false);
    }
}