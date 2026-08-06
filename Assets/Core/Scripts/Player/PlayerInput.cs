using UnityEngine;

public class PlayerInput : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.Instance.uiManager.IsActive(UIManager.UIType.PauseMenu))
            {
                GameManager.Instance.uiManager.ActivateOnly(UIManager.UIType.PauseMenu);
            }
            else
            {
                GameManager.Instance.uiManager.ActivateOnly(UIManager.UIType.HUD);
            }
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (!GameManager.Instance.uiManager.IsActive(UIManager.UIType.Help))
            {
                GameManager.Instance.uiManager.ActivateOnly(UIManager.UIType.Help);
            }
            else
            {
                GameManager.Instance.uiManager.ActivateOnly(UIManager.UIType.HUD);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!GameManager.Instance.uiManager.IsActive(UIManager.UIType.Form))
            {
                GameManager.Instance.uiManager.ActivateOnly(UIManager.UIType.Form);
            }
            else
            {
                GameManager.Instance.uiManager.ActivateOnly(UIManager.UIType.HUD);
            }
        }
    }
}