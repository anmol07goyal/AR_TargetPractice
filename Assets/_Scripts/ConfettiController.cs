using UnityEngine;

public class ConfettiController : MonoBehaviour
{
    private ParticleSystem confetti;

    private void Awake()
    {
        confetti = GetComponent<ParticleSystem>();
        if (confetti != null)
            confetti.Stop();
    }

    private void OnEnable()
    {
        GameManager.ConfettiStatusChanged += OnConfettiStatusChanged;
    }

    private void OnConfettiStatusChanged(bool status)
    {
        if (confetti == null)
            return;

        if (status)
            confetti.Play();
        else
            confetti.Stop();
    }

    private void OnDisable()
    {
        GameManager.ConfettiStatusChanged -= OnConfettiStatusChanged;
    }
}
