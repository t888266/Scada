using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOrUpdatePanel : MonoBehaviour
{
    private void OnDisable()
    {
        Helper.DeviceConfigHelper.ResetAllValue();
    }
}
