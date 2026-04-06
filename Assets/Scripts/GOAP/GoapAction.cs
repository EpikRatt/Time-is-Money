using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GOAP Framework: The abstract base class for all actions.
/// Architectural Purpose: Defines the strict contract for any action injectible into the GOAP planner.
/// Encapsulates execution logic, world state preconditions required to begin the action,
/// world state effects applied upon completion, and dynamically calculable traversal costs.
/// Ensures all actions adhere uniformly to the planner's A* heuristic requirements.
/// </summary>
public abstract class GoapAction : MonoBehaviour
{
}
