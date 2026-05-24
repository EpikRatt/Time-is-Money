using UnityEngine;

public class Action_Sleep : GoapAction
{
    private StateManager stateManager;

    private int RestRate = 20;

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Sleep in Bed";
        ActionCost = 2; // Sleeping is low effort
    }

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
    }

    protected override void SetupEffectsAndPreconditions()
    {
        // Effect: Sleeping makes your energy stable.
        AddEffect(GoapKeys.EnergyState, (int)MotivatorState.Stable);
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true; 
    }

    public override void Perform(GameObject agent)
    {
        if (stateManager == null) return;
        stateManager.RestoreEnergy(RestRate);
        TimeManager.Instance.PerformTick();
    }
}