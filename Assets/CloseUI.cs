using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI : MonoBehaviour
{
    public void close()
    {
        GameManager.Instance.uiManager.GoToHUD();
    }
}
