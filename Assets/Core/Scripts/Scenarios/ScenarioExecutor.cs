using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Events;

public class ScenarioExecutor : MonoBehaviour
{
    [SerializeField]
    private ScenarioObject toPlayScenario;

    // RUNTIME ---------------------
    [SerializeField]
    [HideInInspector]
    private ScenarioObject activeScenario;

    //pull data from the active scenario, for convinience.
    private ScenarioMeta Metadata => activeScenario.scenarioMeta;
    private GlobalRules GlobalRules => activeScenario.globalRules;
    private LogInfo LogInfo => activeScenario.logInfo;

    [SerializeField]
    private ScenarioState runtimeState;
    [SerializedDictionary]
    public SerializedDictionary<string, bool> ActiveFlags => runtimeState.flags;

    //timer ---------------
    private float tickTimerMax = 0.2f;
    private float tickTimer = 0;
    private int Tick;
    private UnityEvent<ScenarioExecutor> OnTick = new();

    private void Awake()
    {
        BeginScenario(toPlayScenario);
        OnTick.AddListener(OnTickUpdate);
    }

    public void BeginScenario(ScenarioObject scenario)
    {
        //clean up previous scenario?

        tickTimer = 0;
        Tick = 0;
        runtimeState.timeElapsed = 0;

        //Activate new scenario
        activeScenario = scenario;
        runtimeState = new ScenarioState(activeScenario.initialState);
    }

    private void Update()
    {
        UpdateTick();
        UpdateRules();
    }
    //called when the system ticks. Currently 20 times a second.
    private void OnTickUpdate(ScenarioExecutor executor)
    {
    }

    private void UpdateRules()
    {
        foreach (var item in GlobalRules.rules)
        {
            if (item.Evaluate(this))
            {
                item.ApplyPassEffects(this);
            }
            else
            {
                item.ApplyFailEffects(this);
            }
        }
    }
    private void UpdateTick()
    {
        tickTimer += Time.deltaTime;
        runtimeState.timeElapsed += Time.deltaTime;
        while (tickTimer >= tickTimerMax)
        {
            tickTimer -= tickTimerMax;
            Tick++;
            OnTick?.Invoke(this);
        }
    }

    public bool GetFlag(String name)
    {
        if (!activeScenario)
        {
            Debug.LogError("No scenario Loaded");
            return false;
        }

        if (ActiveFlags.TryGetValue(name, out var b))
        {
            return b;
        }
        Debug.LogError("Flag doesnt exist: " + name);
        return false;
    }
    public bool SetFlag(String name, bool value)
    {
        if (!activeScenario)
        {
            Debug.LogError("No scenario Loaded");
            return false;
        }
        //: maybe make a list of valid flags?
        ActiveFlags[name] = value;
        return true;
    }
}