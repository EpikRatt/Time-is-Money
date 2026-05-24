using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI; // CRITICAL: Required to talk to the NavMeshAgent

[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(NavMeshAgent))] // Forces Unity to add the NavMesh component
public class GoapAgent : MonoBehaviour
{
    private StateManager stateManager;
    private NavMeshAgent navAgent;
    private List<GoapAction> availableActions;
    private Queue<GoapAction> actionPlan;
    private GoapAction currentAction;
    
    private bool isExecutingPlan = false;

    private void Start()
    {
        stateManager = GetComponent<StateManager>();
        navAgent = GetComponent<NavMeshAgent>();
        
        // Grab all GoapAction scripts attached to this capsule
        availableActions = GetComponents<GoapAction>().ToList();
        
        // Subscribe to the crisis event (e.g., starving to death)
        stateManager.OnCriticalStateReached += TriggerReplanning;
    }

    private void OnDestroy()
    {
        if (stateManager != null)
            stateManager.OnCriticalStateReached -= TriggerReplanning;
    }

    private void Update()
    {
        // If we are currently executing an action, do nothing
        if (isExecutingPlan && currentAction != null) return;

        // If we have a plan, execute the next step
        if (actionPlan != null && actionPlan.Count > 0)
        {
            currentAction = actionPlan.Dequeue();
            StartCoroutine(ExecuteActionRoutine(currentAction));
        }
        else
        {
            // We have no plan. Time to think.
            EvaluateAndPlan();
        }
    }

    private void EvaluateAndPlan()
    {
        Dictionary<string, int> worldState = stateManager.GetWorldState();

        // 2. Define a Goal. Let's make the default goal to get Money.
        Dictionary<string, int> goal = new Dictionary<string, int>
        {
            { GoapKeys.HasRent, 1 }
        };

        // 3. Ask the Brain for a plan
        actionPlan = GoapPlanner.Plan(gameObject, availableActions, worldState, goal);

        if (actionPlan == null)
        {
            Debug.LogWarning("[Planner] Could not find a valid path to the goal.");
        }
    }

    private void TriggerReplanning()
    {
        Debug.Log("CRITICAL STATE: Interrupting current action and replanning!");
        StopAllCoroutines();
        
        // Stop the physical capsule from moving
        if (navAgent.isOnNavMesh) navAgent.ResetPath(); 
        
        currentAction = null;
        isExecutingPlan = false;
        
        // Change our immediate goal to survival (Restore Energy)
        Dictionary<string, int> worldState = stateManager.GetWorldState();
        Dictionary<string, int> survivalGoal = new Dictionary<string, int>
        {
            { GoapKeys.EnergyState, (int)MotivatorState.Stable }
        };
        
        actionPlan = GoapPlanner.Plan(gameObject, availableActions, worldState, survivalGoal);
    }

    private IEnumerator ExecuteActionRoutine(GoapAction action)
    {
        isExecutingPlan = true;

        if (action.TargetLocation != null)
        {
            Debug.Log($"[Agent] Pathing to target for {action.ActionName}...");
            navAgent.SetDestination(action.TargetLocation.position);

            // Wait until the agent physically reaches the destination
            // pathPending means it's still calculating. remainingDistance checks how close it is.
            while (navAgent.pathPending || navAgent.remainingDistance > 0.1f)
            {
                yield return null; // Wait one frame and check again
            }
        }
        else
        {
            Debug.LogWarning($"[Agent] {action.ActionName} has no TargetLocation set in the Inspector!");
        }

        Debug.Log($"[Agent] Arrived. Performing work/sleep animation...");
        yield return new WaitForSeconds(1.0f); // Simulated animation time

        // Apply physical changes (add money, restore energy, etc.)
        action.Perform(gameObject);

        Debug.Log($"[Agent] Completed {action.ActionName}.");
        
        currentAction = null;
        isExecutingPlan = false;
    }
}