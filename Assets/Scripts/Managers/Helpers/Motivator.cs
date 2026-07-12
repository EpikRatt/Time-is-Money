using UnityEngine;

public enum MotivatorState
{
    Stable,   // > 35
    Urgent,   // 6 - 35
    Critical  // 0 - 5
}

[System.Serializable] 
public class Motivator
{
    public string name;
    public int value;
    public int maxValue = 100;

    public int thresholdCritical = 5;
    public int thresholdUrgent = 35;

    public MotivatorState State
    {
        get
        {
            if (value <= thresholdCritical) return MotivatorState.Critical;
            if (value <= thresholdUrgent) return MotivatorState.Urgent;
            return MotivatorState.Stable;
        }
    }

    public void Subtract(int amount) => value = Mathf.Max(0, value - amount);
    public void Add(int amount) => value = Mathf.Min(maxValue, value + amount);
}
