using UnityEngine;

public class UIRoot : MonoBehaviour
{
    private protected Panel currentPanel;

    protected void OpenPanel(Panel panel)
    {
        if (currentPanel != null)
            currentPanel.DeactivatePanel();

        currentPanel = panel;
        currentPanel.ActivatePanel();

    }

    protected void OpenOtherPanel(Panel panel)
    {
        panel.ActivatePanel();
    }

    protected void CloseOtherPanel(Panel panel)
    {
        panel.DeactivatePanel();
    }
}
