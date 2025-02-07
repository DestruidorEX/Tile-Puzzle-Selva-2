using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownTimer : MonoBehaviour
{
    public float startTime = 60f; // tempo inicial em segundos
    private float currentTime; // tempo atual do cronômetro
    public Text timerText; // pega o texto e usa
    [SerializeField] private GameObject PanelDerrota;

    private bool isRunning = false; 

    private void Start()
    {
        StartCountdown(startTime); // inicia a contagem regressiva
        PanelDerrota.SetActive(false); // esconde o painel no início
    }

    public void StartCountdown(float time)
    {
        if (!isRunning) // para não iniciar varias vezes
        {
            currentTime = time;
            isRunning = true;
            StartCoroutine(UpdateTimer());
        }
    }

    private IEnumerator UpdateTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
            yield return null;
        }

        currentTime = 0;
        UpdateTimerDisplay();
        TimerEnded();
    }

    private void UpdateTimerDisplay() //faz minutos e segundos
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private void TimerEnded()
    {
        
        isRunning = false; // quando acaba o tempo acaba o jogo
       
        if (isRunning == false) 
        {
            PanelDerrota.SetActive(true);

        }
    }
}
