using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI; // CRITICAL: Required to talk to the NavMeshAgent

[RequireComponent(typeof(StateManager))]
[RequireComponent(typeof(NavMeshAgent))] // Forces Unity to add the NavMesh component

// Spade a Spade... This is some GEMINI genius, needs some personal review.
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
        Dictionary<string, int> goal = new Dictionary<string, int>();

        if (worldState.ContainsKey(GoapKeys.EnergyState) && worldState[GoapKeys.EnergyState] > (int)MotivatorState.Stable)
        {
            Debug.Log("[Planner] Agent is tired. Setting goal to Sleep.");
            goal.Add(GoapKeys.EnergyState, (int)MotivatorState.Stable);
        }
        else if (worldState.ContainsKey(GoapKeys.HungerState) && worldState[GoapKeys.HungerState] > (int)MotivatorState.Stable)
        {
            Debug.Log("[Planner] Agent is hungry. Setting goal to Eat.");
            goal.Add(GoapKeys.HungerState, (int)MotivatorState.Stable);
        }
        else if (worldState.ContainsKey(GoapKeys.FunState) && worldState[GoapKeys.FunState] > (int)MotivatorState.Stable)
        {
            Debug.Log("[Planner] Agent is bored. Setting goal to Fun.");
            goal.Add(GoapKeys.FunState, (int)MotivatorState.Stable);
        }
        else
        {
            // 2. If all biological needs are met, pursue the overarching goal: Wealth
            Debug.Log("[Planner] Needs are met. Setting goal to Work.");
            goal.Add(GoapKeys.HasMoney, 1);
        }

        // Ask the Brain for a plan based on the chosen goal
        actionPlan = GoapPlanner.Plan(gameObject, availableActions, worldState, goal);

        if (actionPlan == null || actionPlan.Count == 0)
        {
            Debug.LogWarning("[Planner] Could not find a valid path to the goal. Agent is stuck!");
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
        
        Dictionary<string, int> worldState = stateManager.GetWorldState();
        Dictionary<string, int> survivalGoal = new Dictionary<string, int>();

        // Dynamically set survival goal based on whichever biological needs are not stable
        if (worldState.ContainsKey(GoapKeys.HungerState) && worldState[GoapKeys.HungerState] > (int)MotivatorState.Stable)
        {
            survivalGoal.Add(GoapKeys.HungerState, (int)MotivatorState.Stable);
        }
        if (worldState.ContainsKey(GoapKeys.EnergyState) && worldState[GoapKeys.EnergyState] > (int)MotivatorState.Stable)
        {
            survivalGoal.Add(GoapKeys.EnergyState, (int)MotivatorState.Stable);
        }

        if (survivalGoal.Count > 0)
        {
            actionPlan = GoapPlanner.Plan(gameObject, availableActions, worldState, survivalGoal);
        }
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