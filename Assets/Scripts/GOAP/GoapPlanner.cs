using System.Collections.Generic;
using UnityEngine;

// Google Antigravity generated code.
// Needs review
// Goap Planner implementation using A* algorithm to find the optimal sequence of actions to achieve a goal from a given world state, utilizing a custom equality comparer for efficient state comparisons in the open and closed lists.
// Uses a Hashmap to speed up WorldState lookups significantly.
public class DictionaryEqualityComparer : IEqualityComparer<Dictionary<string, int>>
{
    // Google Antigravity generated code.
    // Needs review
    // Checks if two dictionaries are equal by comparing their key-value pairs, ensuring that both dictionaries contain the same keys with the same values, regardless of their order in the dictionary.
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

    // Google Antigravity generated code.
    // Needs review
    // Generates a hash code for a dictionary by XORing the hash codes of its key-value pairs, ensuring that the hash is order-independent for consistent hashing in collections like HashSet and Dictionary.
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

    // Google Antigravity generated code.
    // Needs review
    // Finds the cheapest node in the open list based on f(n) = g(n) + h(n), where g(n) is the running cost and h(n) is the heuristic cost to the goal.
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

    // Google Antigravity generated code.
    // Needs review
    // Calculates the Heuristic cost from the current state to the goal by counting the number of mismatched key-value pairs.
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

    // Google Antigravity generated code.
    // Needs review
    // Checks if the current state satisfies the goal by ensuring all key-value pairs in the goal are present and match in the current state.
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

    // Google Antigravity generated code.
    // Needs review
    // Checks if the preconditions of an action are met in the current state by ensuring all key-value pairs in the preconditions are present and match in the current state.
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

    // Google Antigravity generated code.
    // Needs review
    // Applies the effects of an action to the current state by creating a new state dictionary and updating it with the effects of the action.
    private static Dictionary<string, int> ApplyEffects(Dictionary<string, int> currentState, Dictionary<string, int> effects)
    {
        Dictionary<string, int> newState = new Dictionary<string, int>(currentState);
        foreach (var kvp in effects)
        {
            newState[kvp.Key] = kvp.Value;
        }
        return newState;
    }

    // Google Antigravity generated code.
    // Needs review
    // Using a queue, constructs the path of actions from the end node back to the start node by following the parent references and adding the actions to a list, which is then reversed to get the correct order.
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