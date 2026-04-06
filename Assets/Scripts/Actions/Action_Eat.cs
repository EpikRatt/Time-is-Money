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
}
