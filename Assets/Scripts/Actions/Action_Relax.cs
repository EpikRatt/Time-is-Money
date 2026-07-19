using UnityEngine;

public class Action_Relax : GoapAction
{
    private StateManager stateManager;
    private int funRestoration = 20;

    public int FunRestoration { get => funRestoration; set => funRestoration = value; }

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Relax";
        ActionCost = 1;
    }

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
    }

    protected override void SetupEffectsAndPreconditions()
    {
        AddEffect(GoapKeys.FunState, (int)MotivatorState.Stable);
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override void Perform(GameObject agent)
    {
        if (stateManager == null) return;
        stateManager.RestoreFun(funRestoration);
        TimeManager.Instance.PerformTick();
    }
}