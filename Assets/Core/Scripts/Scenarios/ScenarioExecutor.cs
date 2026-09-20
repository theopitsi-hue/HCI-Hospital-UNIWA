using System;
using System.Collections;
using System.Text;
using AYellowpaper.SerializedCollections;
using TMPro;
using Unity.IO.LowLevel.Unsafe;
using Unity.Profiling;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.tvOS;

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
    private RuleManager GlobalRuleManager => activeScenario.globalRules;
    private LogInfo LogInfo => activeScenario.logInfo;
    public Textmap TextMap => activeScenario.staticData.textmap;
    public ScenarioStaticData activeStaticData => activeScenario.staticData;

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

    //docu gate
    private bool hasActiveGate = false;
    public bool HasActiveDocumentationGate => hasActiveGate;
    public UnityEvent<DocumentationGate> OnDocumentationGateSet;
    public UnityEvent<DocumentationGate> OnDocumentationGateCompleted;

    //completion
    private bool scenarioCompleted;

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
        CleanBlackboardValues();


        //Activate new scenario
        activeScenario = scenario;
        nodeManager.LoadScenarioNodes(this, activeScenario.nodemap);
        runtimeState = new ScenarioState(activeScenario.initialState);
        runtimeState.timeElapsed = 0;

        AddBlackboardValues();

        nodeManager.TryTransition(this, activeScenario.nodemap.entryNodeID);
    }

    public void StopScenario()
    {
        activeScenario = null;
        nodeManager.Clear();
    }

    private void CleanBlackboardValues()
    {
        blackboard.Clear();
    }

    private void Update()
    {
        if (activeScenario == null) return;

        scenarioCompleted = (bool)GameManager.Instance.sceneExecutor.blackboard.GetValue("ScenarioCompleted").GetValue();
        if (scenarioCompleted && GameManager.Instance.loadedLevelScene != null)
        {
            GameManager.Instance.uiManager.ActivateOnly(UIManager.UIType.Score);
            return;
        }

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
        GlobalRuleManager.EvaluateAll(this);
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
        blackboard.RegisterValue("ScenarioCompleted", new BoolValue());

        //todo: get rid of this ewwwwww!!!!
        GameLogger.Log("Created blackboard variables.");
        blackboard.RegisterValue(BB.TimeElapsed, new RemoteFloatValue(() =>
        {
            return runtimeState.timeElapsed;
        },
        (ignored) => { }
        ));

        blackboard.RegisterValue(BB.HeartRate, new RemoteFloatValue(() =>
           {
               return runtimeState.vitals.heartRate;
           }, x =>
           {
               runtimeState.vitals.heartRate = x;
           }
           ));

        blackboard.RegisterValue(BB.BloodOxygenSat, new RemoteFloatValue(() =>
           {
               return runtimeState.vitals.bloodOxygenSaturation;
           }, x =>
           {
               runtimeState.vitals.bloodOxygenSaturation = x;
           }
           ));

        blackboard.RegisterValue(BB.Temperature, new RemoteFloatValue(() =>
       {
           return runtimeState.vitals.bodyTemperature;
       }, x =>
       {
           runtimeState.vitals.bodyTemperature = x;
       }
       ));

        blackboard.RegisterValue(BB.SkinState, new RemoteFloatValue(() =>
                  {
                      return runtimeState.vitals.skinType;
                  }, x =>
                  {
                      runtimeState.vitals.skinType = (int)x;
                  }
                  ));

        blackboard.RegisterValue(BB.BreathRate, new RemoteFloatValue(() =>
       {
           return runtimeState.vitals.breathRate;
       }, x =>
       {
           runtimeState.vitals.breathRate = x;
       }
       ));

        blackboard.RegisterValue(BB.hasActiveDocumentationGate, new RemoteBoolVariable(() =>
                 {
                     return hasActiveGate;
                 }, ignored =>
                 {

                 }
                 ));

        blackboard.RegisterValue(BB.OxygenTankFuel, new RemoteFloatValue(() =>
        {
            return runtimeState.vitals.oxygenTankFuel;
        }, x =>
        {
            runtimeState.vitals.oxygenTankFuel = x;
        }
      ));

        foreach (var hs in activeScenario.ActiveHotspots)
        {
            blackboard.RegisterValue(hs.Key, new BoolValue(hs.Value));
        }

        foreach (var fl in runtimeState.initialBoolKeys)
        {
            blackboard.RegisterValue(fl.Key, new BoolValue(fl.Value));
        }

        foreach (var ft in runtimeState.initialNumberKeys)
        {
            blackboard.RegisterValue(ft.Key, new FloatValue(ft.Value));
        }
    }

    public void ChangeValueOverTime(BlackboardKey key, float floatValueChange, float timeUntilApex, bool total = false)
    {
        StartCoroutine(IncreaseValue(key, floatValueChange, timeUntilApex, total));
    }

    private IEnumerator IncreaseValue(BlackboardKey key, float amount, float duration, bool set)
    {
        FloatValue floatValue = (FloatValue)blackboard.GetValue(key);
        float startValue = (float)floatValue.GetValue();
        float targetValue = set ? amount : (startValue + amount);
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

    public void SetActiveDocumentationGate(DocumentationGate gate)
    {
        if (runtimeState.activeDocumentationGate == gate) return;
        if (runtimeState.IsGateCompleted(gate)) return;

        runtimeState.activeDocumentationGate = gate;
        hasActiveGate = true;
        OnDocumentationGateSet?.Invoke(gate);
        GameLogger.Log("Set docu gate: " + gate);
    }

    public DocumentationGate GetActiveDocumentationGate()
    {
        return runtimeState.activeDocumentationGate;
    }

    public void ClearActiveDocumentationGate(DocumentationGate gate, bool completed = false)
    {
        runtimeState.activeDocumentationGate = null;
        hasActiveGate = false;

        if (completed)
        {
            GameManager.Instance.uiManager.SendUIToast("Form Submitted!", Color.green);
            runtimeState.MarkGateCompleted(gate);
        }
        OnDocumentationGateCompleted?.Invoke(gate);

    }

    public bool IsGateCompleted(DocumentationGate gate)
    {
        return runtimeState.IsGateCompleted(gate);
    }

    public bool TryGetDocumentationGate(string name, out DocumentationGate gate)
    {
        if (activeStaticData.documentationGates.TryGetValue(name, out gate))
        {
            return true;
        }
        else
        {
            GameLogger.LogError($"No such documentation gate found: '{name}', are you sure it exists in the scenario's static data?");

            return false;
        }
    }

    public bool CanUseHotSpot(string interactionName)
    {
        return (bool)blackboard.GetValue(interactionName).GetValue();
    }

    public void AddScore(int score, string reason)
    {
        runtimeState.AddScore(score, reason);
    }

    public string GetScoreReport()
    {
        StringBuilder sb = new();
        foreach (var ss in runtimeState.scoreReasons)
        {
            sb.Append(ss.Item2);
            sb.Append(" ");
            if (ss.Item1 != 0)
            {
                if (ss.Item1 < 0)
                {
                    sb.Append("<color=#8B0000>");

                }
                else
                {
                    sb.Append("<color=#006400>");
                }
                if (ss.Item1 > 0)
                {
                    sb.Append("+");

                }
                sb.Append(ss.Item1);
                sb.Append("</color>");

                sb.Append(" ");
            }

            sb.Append("\n");
        }
        return sb.ToString();
    }

    public int GetTotalScore()
    {
        int fin = 0;
        foreach (var ss in runtimeState.scoreReasons)
        {
            fin += ss.Item1;
        }
        return fin;
    }

    public int GetTotalPossibleScore()
    {
        return runtimeState.PossibleMaxScore;
    }

    public void AddNodePathTrack(string nodeName)
    {
        if (!runtimeState.nodePathSelected.Contains(nodeName))
            runtimeState.nodePathSelected.Add(nodeName);
    }

    public string GetDecisionReport()
    {
        StringBuilder sb = new();
        foreach (var ss in runtimeState.nodePathSelected)
        {
            sb.Append(ss);
            sb.Append("\n");
        }
        return sb.ToString();
    }

    public void AddActionToRecord(string actionid, string label)
    {
        runtimeState.possibleActionsToRecord.Add(actionid, label);
    }

    public void ClearActionsToRecord()
    {
        runtimeState.possibleActionsToRecord.Clear();
    }

    public SerializedDictionary<string, string> GetPossibleActionsToRecord()
    {
        return runtimeState.possibleActionsToRecord;
    }
}