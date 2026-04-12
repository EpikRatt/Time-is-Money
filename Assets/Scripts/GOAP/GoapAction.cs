using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour
{
    public string ActionName { get; protected set; } = "Unnamed Action";
    public float ActionCost { get; protected set; } = 1f;

    public Dictionary<string, int> Preconditions { get; private set; }
    public Dictionary<string, int> Effects { get; private set; }

    protected virtual void Awake()
    {
        Preconditions = new Dictionary<string, int>();
        Effects = new Dictionary<string, int>();
        SetupEffectsAndPreconditions();
    }

    public abstract void Perform(GameObject agent);

    protected abstract void SetupEffectsAndPreconditions();

    public abstract bool CheckProceduralPrecondition(GameObject agent);

    protected void AddPrecondition(string key, int value) { Preconditions[key] = value; }
    protected void RemovePrecondition(string key) { if (Preconditions.ContainsKey(key)) Preconditions.Remove(key); }
    
    protected void AddEffect(string key, int value) { Effects[key] = value; }
    protected void RemoveEffect(string key) { if (Effects.ContainsKey(key)) Effects.Remove(key); }
}