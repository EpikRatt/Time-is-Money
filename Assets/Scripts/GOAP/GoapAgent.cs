using System.Collections;
using UnityEngine;

public class GoapAgent : MonoBehaviour
{
    /// <summary>
    /// Public entry point to be called by the Planner when a sequence is decided.
    /// </summary>
    public void ExecuteAction(GoapAction targetAction)
    {
        StartCoroutine(ActionRoutine(targetAction));
    }

    private IEnumerator ActionRoutine(GoapAction currentAction)
    {
        Debug.Log($"Agent intent: {currentAction.ActionName}. Navigating...");

        // 1. Pathfinding Phase
        // Placeholder: Wait 1 second to simulate walking to the interaction spot.
        // In production, this will be a while loop checking NavMeshAgent.remainingDistance.
        yield return new WaitForSeconds(1.0f);

        Debug.Log($"Agent arrived. Executing {currentAction.ActionName}.");

        for (int i = 0; i < currentAction.DurationTicks; i++)
        {
            yield return new WaitForSeconds(1.0f);

            currentAction.PerformTick(gameObject);

            if (TimeManager.Instance != null)
            {
                TimeManager.Instance.AdvanceSingleTick();
            }
        }

        Debug.Log($"{currentAction.ActionName} complete.");
        
        // 3. End Phase
        // Here you will eventually call GoapPlanner.Recalculate() to find the next action.
    }
}