using System.Collections.Generic;
using UnityEngine;

// Google Antigravity generated code.
// Needs review
// Uses a Hashmap to speed up WorldState lookups significantly.
public class DictionaryEqualityComparer : IEqualityComparer<Dictionary<string, int>>
{
    public bool Equals(Dictionary<string, int> x, Dictionary<string, int> y)
    {
        if (ReferenceEquals(x, y)) return true;
        if (x == null || y == null) return false;
        if (x.Count != y.Count) return false;

        foreach (var kvp in x)
        {
            if (!y.TryGetValue(kvp.Key, out int val) || val != kvp.Value)
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode(Dictionary<string, int> obj)
    {
        if (obj == null) return 0;
        int hash = 0;
        foreach (var kvp in obj)
        {
            // XOR is commutative, ensuring the hash is order-independent
            int kvpHash = kvp.Key.GetHashCode() ^ kvp.Value.GetHashCode();
            hash ^= kvpHash;
        }
        return hash;
    }
}

// Google Antigravity generated code.
// Needs review
// A* Algorithm implementation, needs testing.
public static class GoapPlanner
{
    private static readonly DictionaryEqualityComparer StateComparer = new DictionaryEqualityComparer();

    public static Queue<GoapAction> Plan(GameObject agent, List<GoapAction> availableActions, Dictionary<string, int> worldState, Dictionary<string, int> goal)
    {
        List<GoapNode> openList = new List<GoapNode>();
        HashSet<Dictionary<string, int>> closedList = new HashSet<Dictionary<string, int>>(StateComparer);

        GoapNode startNode = new GoapNode(null, 0, new Dictionary<string, int>(worldState), null);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            GoapNode currentNode = GetCheapestNode(openList, goal);
            openList.Remove(currentNode);

            if (IsGoalReached(currentNode.State, goal))
            {
                return ConstructPath(currentNode);
            }

            closedList.Add(currentNode.State);

            foreach (GoapAction action in availableActions)
            {
                if (!action.CheckProceduralPrecondition(agent))
                    continue;

                if (!ArePreconditionsMet(action.Preconditions, currentNode.State))
                    continue;

                Dictionary<string, int> newState = ApplyEffects(currentNode.State, action.Effects);

                if (closedList.Contains(newState))
                    continue;

                int newRunningCost = currentNode.RunningCost + action.ActionCost;

                GoapNode existingNode = null;
                foreach (var node in openList)
                {
                    if (StateComparer.Equals(node.State, newState))
                    {
                        existingNode = node;
                        break;
                    }
                }

                if (existingNode != null)
                {
                    if (newRunningCost < existingNode.RunningCost)
                    {
                        openList.Remove(existingNode);
                        GoapNode newNode = new GoapNode(currentNode, newRunningCost, newState, action);
                        openList.Add(newNode);
                    }
                }
                else
                {
                    GoapNode newNode = new GoapNode(currentNode, newRunningCost, newState, action);
                    openList.Add(newNode);
                }
            }
        }

        return null; // Return null if no plan is found
    }

    private static GoapNode GetCheapestNode(List<GoapNode> openList, Dictionary<string, int> goal)
    {
        GoapNode cheapest = null;
        int lowestCost = int.MaxValue;

        foreach (GoapNode node in openList)
        {
            int heuristic = CalculateHeuristic(node.State, goal);
            int fCost = node.RunningCost + heuristic;

            if (fCost < lowestCost)
            {
                lowestCost = fCost;
                cheapest = node;
            }
        }

        return cheapest;
    }

    private static int CalculateHeuristic(Dictionary<string, int> state, Dictionary<string, int> goal)
    {
        int cost = 0;
        foreach (var kvp in goal)
        {
            if (!state.ContainsKey(kvp.Key) || state[kvp.Key] != kvp.Value)
            {
                cost++;
            }
        }
        return cost;
    }

    private static bool IsGoalReached(Dictionary<string, int> state, Dictionary<string, int> goal)
    {
        foreach (var kvp in goal)
        {
            // CRITICAL LOGIC RULE: EXACT EQUALITY (== or !=)
            if (!state.ContainsKey(kvp.Key) || state[kvp.Key] != kvp.Value)
            {
                return false;
            }
        }
        return true;
    }

    private static bool ArePreconditionsMet(Dictionary<string, int> preconditions, Dictionary<string, int> state)
    {
        foreach (var kvp in preconditions)
        {
            // CRITICAL LOGIC RULE: EXACT EQUALITY (== or !=)
            if (!state.ContainsKey(kvp.Key) || state[kvp.Key] != kvp.Value)
            {
                return false;
            }
        }
        return true;
    }

    private static Dictionary<string, int> ApplyEffects(Dictionary<string, int> currentState, Dictionary<string, int> effects)
    {
        Dictionary<string, int> newState = new Dictionary<string, int>(currentState);
        foreach (var kvp in effects)
        {
            newState[kvp.Key] = kvp.Value;
        }
        return newState;
    }

    private static Queue<GoapAction> ConstructPath(GoapNode endNode)
    {
        List<GoapAction> path = new List<GoapAction>();
        GoapNode currentNode = endNode;
        
        while (currentNode.Action != null)
        {
            path.Add(currentNode.Action);
            currentNode = currentNode.Parent;
        }
        
        path.Reverse();
        return new Queue<GoapAction>(path);
    }
}