using UnityEngine;

/// <summary>
/// Concrete Action: Eat.
/// Architectural Purpose: A specific implementation of GoapAction.
/// Defines the preconditions, effects, and costs associated with consuming food.
/// Conceptually restores the agent's Hunger metric while consuming Money and Time.
/// Evaluated dynamically by the GoapPlanner to satisfy survival-oriented goals.
/// </summary>
public class Action_Eat : GoapAction
{
    private StateManager stateManager;

    protected override void Awake()
    {
        base.Awake();
        ActionName = "Eat a Meal";
        ActionCost = 5f; // We will make this dynamic later!
    }

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
    }

    protected override void SetupEffectsAndPreconditions()
    {
        // We cast the Enum to an int for the Dictionary
        // EFFECT: After eating, the agent's Hunger State will be Stable.
        AddEffect("HungerState", (int)NeedState.Stable);
        
        // PRECONDITION: You could add something like AddPrecondition("hasFood", 1);
    }

    public override bool CheckProceduralPrecondition(GameObject agent)
    {
        return true; 
    }

    public override void Perform(GameObject agent)
    {
        if (stateManager == null) return;

        stateManager.hunger.Add(50);

        TimeManager.Instance.HandleTick();
    }
}
