using UnityEngine;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// User Interface: Simulation controller bridge.
/// Architectural Purpose: Acts as the primary interface between Canvas UI elements (sliders, toggles)
/// and the underlying Core Managers. Enforces decoupled communication by translating user input
/// into parameter updates, subsequently forcing the GOAP planner to discard invalidated plans
/// and recalculate trajectories based on the newly introduced constraints.
/// </summary>
/// 
public class SimulationControllerUI : MonoBehaviour
{
    [SerializeField] private StateManager stateManager;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI energyText;
    [SerializeField] private TextMeshProUGUI hungerText;
    [SerializeField] private TextMeshProUGUI funText;

    private void OnEnable()
    {
        TimeManager.OnTick += UpdateDashboard;
    }

    private void OnDisable()
    {
        TimeManager.OnTick -= UpdateDashboard;
    }

    private void Start()
    {
        UpdateDashboard();
    }

    private void UpdateDashboard()
    {
        if (stateManager == null) return;

        timeText.text = "$Current Time: {TimeManager.Instance.GetInGameTime()}";
        moneyText.text = "$NetWorth: {stateManager.CurrentMoney}";
        energyText.text = "$NetWorth: {stateManager.CurrentEnergy}";
        hungerText.text = "$NetWorth: {stateManager.CurrentMoney}";
        funText.text = "$NetWorth: {stateManager.CurrentMoney}";
    }
}
