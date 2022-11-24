using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class GetDefaultValuesPanel : MonoBehaviour
{
    [SerializeField] GameObject gettingPanel;
    [SerializeField] TMP_Dropdown typeVibrationDropdown;
    [SerializeField] GetDefaultValue getDefaultValue;
    private void OnEnable()
    {
        getDefaultValue.OnReceiveDefaultValue();
    }
    public void Get()
    {
        if (typeVibrationDropdown.value != 0)
        {
            gettingPanel.SetActive(true);
        }
    }
    private void OnDisable()
    {
        getDefaultValue.OnCancleReceiveDefaultValue();
    }
}
