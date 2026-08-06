using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InteractReceiver))]
//change name to like, observation point or some shit
public class MachineThermometer : MonoBehaviour
{
    InteractReceiver rec;
    [SerializeField]
    BlackboardKey key;

    private void Awake()
    {
        rec = GetComponent<InteractReceiver>();
        rec.onInteracted.AddListener(OnInteracted);
    }

    public void OnInteracted()
    {
        GameManager.Instance.playerData.AddKnownValue(key);
        print("Lever: " + key.name);
    }
}
