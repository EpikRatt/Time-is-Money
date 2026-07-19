using UnityEngine;

public class Action_Sleep : GoapAction
{
    private StateManager stateManager;

    private int restRate = 15;

    public int RestRate { get => restRate; set => restRate = value; }

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Sleep in Bed";
        ActionCost = 2;
    }

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
    }

    protected override void SetupEffectsAndPreconditions()
    {
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