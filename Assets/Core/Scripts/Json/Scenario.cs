using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScenarioWrapper
{
    public Scenario scenario;
}

[System.Serializable]
public class Scenario
{
    public string id="";
    public string title = "";
    public string description = "";
    public int estimated_duration_minutes = 1;
    public string difficulty = "";
    public string[] learning_goals;

}
