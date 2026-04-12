using UnityEngine;

public class Action_Work : GoapAction
{
    private StateManager stateManager;

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Work at Terminal";
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

        // 1. Apply bulk stat changes for this 1 turn
        stateManager.AddMoney(20);

        // 2. The Action explicitly commands time to move forward
        TimeManager.Instance.HandleTick();
    }
}