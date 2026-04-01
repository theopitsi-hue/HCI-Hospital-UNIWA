using System;
using System.Collections.Generic;

[Serializable]
public class Rule
{
    public string id;
    public List<Condition> conditions = new();
    public List<Effect> effects = new();

    public bool Evaluate()
    {
        foreach (var con in conditions)
        {
            if (!con.Evaluate())
            {
                return false;
            }
        }
        return true;
    }

    public void ApplyEffects()
    {
        foreach (var con in effects)
        {
            con.Apply();
        }
    }
}