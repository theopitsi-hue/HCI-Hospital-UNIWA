using System;
using AYellowpaper.SerializedCollections;
using Unity.IO.LowLevel.Unsafe;
using Unity.Profiling;
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

    //timer ---------------
    private float tickTimerMax = 0.2f;
    private float tickTimer = 0;
    private int Tick = 0;
    private UnityEvent<ScenarioExecutor> OnTick = new();

    //blackboard ---
    public Blackboard blackboard = new();

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


        AddBlackboardValues();
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

    //--blackboard

    private void AddBlackboardValues()
    {
        Debug.Log("Created blackboard variables.");
        blackboard.SetValue(BB.TimeElapsed, new RemoteFloatValue(() =>
        {
            return runtimeState.timeElapsed;
        },
        (ignored) => { }
        ));


        foreach (var fl in runtimeState.flags)
        {
            blackboard.SetValue(fl.Key, new BoolValue(fl.Value));
        }
    }
}