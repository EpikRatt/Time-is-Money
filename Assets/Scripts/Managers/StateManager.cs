using System;
using UnityEngine;

/// <summary>
/// Core Manager: Agent vitality tracker.
/// Architectural Purpose: Manages global state parameters and tracks aggregate agent vitality.
/// Broadcasts critical thresholds and state changes to subscribed observers, enabling the 
/// GOAP system and UI to react to evolving conditions without tight coupling.
/// </summary>
/// 
public class StateManager : MonoBehaviour
{
    public event Action OnCriticalStateReached;

    public int CurrentMoney { get; private set; } = 100;
    public int CurrentEnergy { get; private set; } = 100;
    public int CurrentHunger { get; private set; } = 100;
    public int CurrentFun { get; private set; } = 100;

    [SerializeField] private int energyDrainPerTick = 2;
    [SerializeField] private int hungerDrainPerTick = 5;
    [SerializeField] private int funDrainPerTick = 3;

    private const int MAX_STAT_VALUE = 100;
    private const int CRITICAL_THRESHOLD = 30;

    private void OnEnable()
    {
        // Subscribe to the global time pulse using a distinct handler name
        TimeManager.OnTick += HandleTick;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks if the object is destroyed
        if (TimeManager.Instance != null)
        {
            TimeManager.OnTick -= HandleTick;
        }
    }

    private void HandleTick()
    {
        // Apply Degradation
        CurrentEnergy -= energyDrainPerTick;
        CurrentHunger -= hungerDrainPerTick;
        CurrentFun -= funDrainPerTick;

        // Clamp values to prevent negative states or exceeding maximums
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MAX_STAT_VALUE);
        CurrentHunger = Mathf.Clamp(CurrentHunger, 0, MAX_STAT_VALUE);
        CurrentFun = Mathf.Clamp(CurrentFun, 0, MAX_STAT_VALUE);

        EvaluateCriticalStates();
    }

    private void EvaluateCriticalStates()
    {
        // If biological requirements fall below the threshold, broadcast the interrupt event
        if (CurrentEnergy <= CRITICAL_THRESHOLD || CurrentHunger <= CRITICAL_THRESHOLD)
        {
            OnCriticalStateReached?.Invoke();
        }
    }

    public void AddMoney(int amount) { CurrentMoney += amount; }
    public void SubtractMoney(int amount) { CurrentMoney -= amount; }
    public void RestoreHunger(int amount) { CurrentHunger = Mathf.Clamp(CurrentHunger + amount, 0, MAX_STAT_VALUE); }
    public void RestoreEnergy(int amount) { CurrentEnergy = Mathf.Clamp(CurrentEnergy + amount, 0, MAX_STAT_VALUE); }
    public void RestoreFun(int amount) { CurrentFun = Mathf.Clamp(CurrentFun + amount, 0, MAX_STAT_VALUE); }
}