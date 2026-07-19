using UnityEngine;

public class Action_Eat : GoapAction
{
    private StateManager stateManager;

    // TODO: Add a cost for eating, and a precondition for having money to eat.
    // private int mealCost = 5;
    private int mealNutrition = 100;

    public int MealNutrition { get => mealNutrition; set => mealNutrition = value; }

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Eat";
        ActionCost = 3;
    }

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
    }

    protected override void SetupEffectsAndPreconditions()
    {
        // PART OF TODO!
        //AddPrecondition(GoapKeys.HasMoney, 1);

        AddEffect(GoapKeys.HungerState, (int)MotivatorState.Stable);
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true;
    }

    public override void Perform(GameObject agent)
    {
        if (stateManager == null) return;

        // PART OF TODO!
        //stateManager.SubtractMoney(mealCost);

        stateManager.RestoreHunger(mealNutrition);

        TimeManager.Instance.PerformTick();
    }
}