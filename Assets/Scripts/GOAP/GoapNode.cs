using System.Collections.Generic;

public class GoapNode
{
    public GoapNode Parent { get; private set; }
    public int RunningCost { get; private set; }
    public Dictionary<string, int> State { get; private set; }
    public GoapAction Action { get; private set; }

    public GoapNode(GoapNode parent, int runningCost, Dictionary<string, int> state, GoapAction action)
    {
        Parent = parent;
        RunningCost = runningCost;
        State = state;
        Action = action;
    }
}