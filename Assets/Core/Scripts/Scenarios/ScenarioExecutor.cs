using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

public class ScenarioExecutor : MonoBehaviour
{
    [SerializeField]
    private ScenarioObject toPlayScenario;
    [SerializeField]
    private ScenarioObject activeScenario;

    //pull data from the active scenario, for convinience.
    private ScenarioMeta Metadata => activeScenario.scenarioMeta;
    private GlobalRules GlobalRules => activeScenario.globalRules;
    private LogInfo LogInfo => activeScenario.logInfo;

    [SerializeField]
    private ScenarioState runtimeState;
    [SerializedDictionary]
    public SerializedDictionary<string, bool> ActiveFlags => runtimeState.flags;

    private void Awake()
    {
        BeginScenario(toPlayScenario);
    }

    public void BeginScenario(ScenarioObject scenario)
    {
        //clean up previous scenario? dunno

        activeScenario = scenario;
        runtimeState = new ScenarioState(activeScenario.initialState);
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

    //todo: make a heartbeat so we dont check every frame
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

    private void Update()
    {
        UpdateRules();
    }

}