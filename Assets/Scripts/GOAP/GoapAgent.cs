using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GOAP Framework: The agent state machine.
/// Architectural Purpose: Resides on the physical GameObject representing the actor.
/// Acts as the bridge between the conceptual GOAP planner and the Unity environment.
/// Owns the active goals, requests plan formulations from the GoapPlanner, and drives
/// the execution sequence of the returned GoapActions via a finite state machine.
/// </summary>
public class GoapAgent : MonoBehaviour
{
}
