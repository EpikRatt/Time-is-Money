using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static event Action OnTick;
    
    public static TimeManager Instance { get; private set; }

    private int minutesPerTick = 15;
    private int startHour = 8;

    public int TotalTicks { get; private set; } = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void HandleTick()
    {
        TotalTicks++;
        OnTick?.Invoke(); 
    }

    public string GetInGameTime()
    {
        int totalMinutes = TotalTicks * minutesPerTick;
  
        int hours = (totalMinutes / 60) % 24;
        int minutes = totalMinutes % 60;

        string meridiem = hours < 12 ? "AM" : "PM";
        int civilianHours = hours % 12;
        if (civilianHours == 0) civilianHours = 12;

        return $"{civilianHours}:{minutes:00} {meridiem}";
    }
}