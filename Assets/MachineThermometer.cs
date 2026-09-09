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
        if (!GameManager.Instance.playerData.KnowsValue(key))
        {
            GameManager.Instance.playerData.AddKnownValue(key);
            print("Lever: " + key.name);
            GameManager.Instance.uiManager.SentToast("Temperature has been recorded. Fill it in the EHR field.", Color.white);
        }
        print("Lever: " + key.name);

    }
}
