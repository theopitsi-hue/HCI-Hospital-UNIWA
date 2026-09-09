using System.Collections.Generic;
using UnityEngine;

public abstract class MachineUI : MonoBehaviour
{
    [SerializeField]
    protected List<BlackboardKey> keys;
    protected float tick;

    public virtual void Setup(Machine machine)
    {
        DoUITick();
    }

    public virtual void Update()
    {
        tick += Time.deltaTime;
        if (tick > 1.5f)
        {
            tick = 0;
            DoUITick();
        }
    }

    protected virtual void DoUITick()
    {

    }
}