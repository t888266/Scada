using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateDeviceConfigButton : MonoBehaviour
{
    UpdateDeviceConfig updateDeviceConfig;
    PressButtonToOpenPanel pressButtonToOpenPanel;
    OpenPanel openPanel;
    public void SetData(UpdateDeviceConfig updateDeviceConfig, PressButtonToOpenPanel pressButtonToOpenPanel, OpenPanel openPanel)
    {
        this.updateDeviceConfig = updateDeviceConfig;
        this.pressButtonToOpenPanel = pressButtonToOpenPanel;
        this.openPanel = openPanel;
    }
    public void UpdateDeviceConfig()
    {
        updateDeviceConfig.UpdateThisDeviceConfig();
        pressButtonToOpenPanel.HidePanel();
        openPanel.HidePanel();
    }
}
