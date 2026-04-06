using System;
using UnityEngine;

/// <summary>
/// Core Manager: Global tick controller.
/// Architectural Purpose: Operates as a Singleton and subject in the Observer pattern.
/// It drives the passage of time for the entire simulation. This clock is strictly 
/// locked during execution to ensure deterministic AI behavior under pressure.
/// Provides a centralized source of truth for the simulation's tempo.
/// </summary>
/// 
public class TimeManager : MonoBehaviour
{
    [SerializeField] private float tickRate = 1.0f;
    [SerializeField] private int minutePerTick = 15;

    public int TotalTicks { get; private set; } = 0;

    public static event Action OnTick;

    public static TimeManager Instance { get; private set; } 

    private void Awake()
    {
        // Enforce the Singleton pattern
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate TimeManager detected. Destroying instance.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private float timer = 0f;
    private void Update()
    {
        timer += Time.deltaTime;

        while (timer >= tickRate)
        {
            timer -= tickRate;
            TotalTicks++;
            
            OnTick?.Invoke();
        }
    }

    public string GetInGameTime()
    {
        int totalMinutes = TotalTicks / minutePerTick;
  
        int hours = totalMinutes / 60;
        int minutes = totalMinutes % 60;

        string meridiem = hours < 12 ? "AM" : "PM";
        int civilianHours = hours % 12;
        if (civilianHours == 0) civilianHours = 12;

        return $"{civilianHours}:{minutes:00} {meridiem}";
    }
}