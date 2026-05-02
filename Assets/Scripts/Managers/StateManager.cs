using System;
using System.Collections.Generic; // CRITICAL: Required for Dictionary
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public event Action OnCriticalStateReached;

    [field: SerializeField] public Motivator Money { get; private set; } = new Motivator { name = "Money", value = 100 };    
    [field: SerializeField] public Motivator Energy { get; private set; } = new Motivator { name = "Energy", value = 100 };
    [field: SerializeField] public Motivator Hunger { get; private set; } = new Motivator { name = "Hunger", value = 100 };
    [field: SerializeField] public Motivator Fun { get; private set; } = new Motivator { name = "Fun", value = 100 };

    [Header("Cost of Living Settings")]
    [field: SerializeField] public int energyDrainPerTick { get; private set; } = 2;
    [field: SerializeField] public int hungerDrainPerTick { get; private set; } = 5;
    [field: SerializeField] public int funDrainPerTick { get; private set; } = 3;

    private void OnEnable()
    {
        TimeManager.OnTick += HandleTick;
    }

    private void OnDisable()
    {
        TimeManager.OnTick -= HandleTick;
    }

    private void HandleTick()
    {
        ApplyCostOfLiving();
        EvaluateCriticalStates();
    }

    private void ApplyCostOfLiving()
    {
        ConsumeEnergy(energyDrainPerTick);
        ConsumeHunger(hungerDrainPerTick);
        ConsumeFun(funDrainPerTick);
    }

    public Dictionary<string, int> GetWorldState()
    {
        Dictionary<string, int> worldState = new Dictionary<string, int>
        {
            { GoapKeys.MoneyState, (int)Money.State },
            { GoapKeys.EnergyState, (int)Energy.State },
            { GoapKeys.HungerState, (int)Hunger.State },
            { GoapKeys.FunState, (int)Fun.State },
        };
        return worldState;
    }

    private void EvaluateCriticalStates()
    {
        // If biological requirements fall below the threshold, broadcast the interrupt event
        if (Energy.State == MotivatorState.Critical || Hunger.State == MotivatorState.Critical || 
            Energy.State == MotivatorState.Urgent || Hunger.State == MotivatorState.Urgent)
        {
            OnCriticalStateReached?.Invoke();
        }
    }

    public void AddMoney(int amount) { Money.Add(amount); }
    public void SubtractMoney(int amount) { Money.Subtract(amount); }

    public void ConsumeEnergy(int amount) { Energy.Subtract(amount); }
    public void RestoreEnergy(int amount) { Energy.Add(amount); }

    public void ConsumeHunger(int amount) { Hunger.Subtract(amount); }
    public void RestoreHunger(int amount) { Hunger.Add(amount); }

    public void ConsumeFun(int amount) { Fun.Subtract(amount); }
    public void RestoreFun(int amount) { Fun.Add(amount); }
}