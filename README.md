# Time is Money: Autonomous GOAP Agent

[![Play in Browser](https://img.shields.io/badge/Play_Now-WebGL-blue)](#Link goes here)

## Overview
"Time is Money" is a deterministic artificial intelligence simulation built in Unity. It demonstrates an autonomous agent utilizing Goal-Oriented Action Planning (GOAP) to optimize resource management. The agent evaluates competing priorities—maximizing money generation while strictly managing degrading biological states (Hunger and Energy) against the strict constraint of Time(Ticks).

## System Architecture
* **Engine:** Unity (WebGL Target)
* **Language:** C#
* **AI Framework:** Custom GOAP implementation.
* **Pathfinding:** Unity NavMesh integrated with action execution.

## Core Systems
### 1. State Space Management
The agent's decision-making is driven by four strictly typed variables:
* **Money:** The primary maximization objective.
* **Time:** A discrete tick-based resource. Actions consume time, forcing the agent to evaluate opportunity cost.
* **Hunger & Energy:** Degrading biological constraints. If either reaches a critical threshold, the agent enters a failure state.

### 2. The Decision Engine (GOAP)
Instead of hardcoded reactive logic, the agent relies on an algorithmic planner:
* **Actions:** Modular C# classes defining strict `Preconditions` and `Effects`.
* **Graph Search:** The planner utilizes an **A* search algorithm** to build an optimal sequence of actions backward from its goal state to its current reality.
* **Dynamic Edge Weights:** The time and monetary cost of an action act as edge weights during the A* search, ensuring the agent selects the most efficient execution path.

## Contact
* **Developer:** Matthew Bridge
* **GitHub:** [@EpikRatt](https://github.com/EpikRatt
* **LinkedIn:** www.linkedin.com/in/matthew-l-bridge