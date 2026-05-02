using UnityEngine;

public class Action_Work : GoapAction
{
    private StateManager stateManager;

    private int salary = 20;

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Work";
        ActionCost = 5f;
    }

    private void Start()
    { 
        stateManager = GetComponent<StateManager>();
    }

    protected override void SetupEffectsAndPreconditions()
    {
        AddPrecondition("isExhausted", 0);
        AddEffect("hasMoney", 1);
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true; 
    }

    public override void Perform(GameObject agent)
    {
        if (stateManager == null) return;

        stateManager.AddMoney(salary);

        TimeManager.Instance.PerformTick(); // Tick Event Call
    }
}