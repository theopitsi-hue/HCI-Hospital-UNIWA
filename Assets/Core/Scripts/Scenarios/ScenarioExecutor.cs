using System;
using System.Collections;
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
    private Textmap Dialogue => activeScenario.textmap;

    [SerializeField]
    private ScenarioState runtimeState;

    //timer ---------------
    private float tickTimerMax = 0.2f;
    private float tickTimer = 0;
    private int Tick = 0;
    private UnityEvent<ScenarioExecutor> OnTick = new();

    //blackboard ---
    public Blackboard blackboard = new();

    //Nodemap
    public NodeManager nodeManager = new();

    private void Awake()
    {
        //print(toPlayScenario.Serialize());

        //BeginScenario(toPlayScenario);
        OnTick.AddListener(OnTickUpdate);
    }

    public void BeginScenario(ScenarioObject scenario)
    {
        //
        print("Starting: " + scenario.name);

        //clean up previous scenario?
        tickTimer = 0;
        Tick = 0;
        runtimeState.timeElapsed = 0;


        //Activate new scenario
        activeScenario = scenario;
        nodeManager.LoadScenarioNodes(this, activeScenario.nodemap);
        runtimeState = new ScenarioState(activeScenario.initialState);

        AddBlackboardValues();

        nodeManager.TryTransition(this, activeScenario.nodemap.entryNodeID);
    }

    private void Update()
    {
        if (activeScenario == null) return;

        UpdateTick();
        UpdateRules();
        nodeManager.Update(this);//move to tick update?
    }
    //called when the system ticks. Currently 20 times a second.
    private void OnTickUpdate(ScenarioExecutor executor)
    {
        //print(blackboard.GetValue("HeartRate").GetValue().ToString());
        //print(blackboard.GetValue("TimeElapsed").GetValue().ToString());
    }

    private void UpdateRules()
    {
        for (int i = GlobalRules.rules.Count - 1; i >= 0; i--)
        {
            var item = GlobalRules.rules[i];

            if (item.Evaluate(this))
            {
                item.ApplyPassEffects(this);
                if (item.TriggerOnce)
                {
                    GlobalRules.Disable(item);
                }
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

        blackboard.SetValue(BB.HeartRate, new RemoteFloatValue(() =>
           {
               return runtimeState.vitals.heartRate;
           }, x =>
           {
               runtimeState.vitals.heartRate = x;
           }
           ));

        blackboard.SetValue(BB.BloodOxygenSat, new RemoteFloatValue(() =>
           {
               return runtimeState.vitals.bloodOxygenSaturation;
           }, x =>
           {
               runtimeState.vitals.bloodOxygenSaturation = x;
           }
           ));

        blackboard.SetValue(BB.BloodPressDiastolic, new RemoteFloatValue(() =>
                   {
                       return runtimeState.vitals.bloodPressureDiastolic;
                   }, x =>
                   {
                       runtimeState.vitals.bloodPressureDiastolic = x;
                   }
                   ));

        blackboard.SetValue(BB.BloodPressSystolic, new RemoteFloatValue(() =>
            {
                return runtimeState.vitals.bloodPressureSystolic;
            }, x =>
            {
                runtimeState.vitals.bloodPressureSystolic = x;
            }
            ));

        blackboard.SetValue(BB.Temperature, new RemoteFloatValue(() =>
       {
           return runtimeState.vitals.bodyTemperature;
       }, x =>
       {
           runtimeState.vitals.bodyTemperature = x;
       }
       ));

        blackboard.SetValue(BB.SkinState, new RemoteFloatValue(() =>
                  {
                      return runtimeState.vitals.skinType;
                  }, x =>
                  {
                      runtimeState.vitals.skinType = (int)x;
                  }
                  ));

        blackboard.SetValue(BB.BreathRate, new RemoteFloatValue(() =>
       {
           return runtimeState.vitals.breathRate;
       }, x =>
       {
           runtimeState.vitals.breathRate = x;
       }
       ));


        foreach (var fl in runtimeState.flags)
        {
            blackboard.SetValue(fl.Key, new BoolValue(fl.Value));
        }
    }

    public void ChangeValueOverTime(BlackboardKey key, float floatValueChange, float timeUntilApex)
    {
        StartCoroutine(IncreaseValue(key, floatValueChange, timeUntilApex));
    }

    private IEnumerator IncreaseValue(BlackboardKey key, float amount, float duration)
    {
        FloatValue floatValue = (FloatValue)blackboard.GetValue(key);
        float startValue = (float)floatValue.GetValue();
        float targetValue = startValue + amount;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / duration;
            floatValue.SetValue((float)Mathf.Lerp(startValue, targetValue, t));

            yield return null;
        }

        floatValue.SetValue((float)targetValue);
    }
}