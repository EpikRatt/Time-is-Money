using UnityEngine;
using TMPro;

public class DashboardUI : MonoBehaviour
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

        timeText.text = $"Current Time: {TimeManager.Instance.GetInGameTime()}";
        moneyText.text = $"Money: {stateManager.Money.value}";
        energyText.text = $"Energy: {stateManager.Energy.value}";
        hungerText.text = $"Hunger: {stateManager.Hunger.value}";
        funText.text = $"Fun: {stateManager.Fun.value}";
    }
}
