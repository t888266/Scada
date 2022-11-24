using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ShowHidePassword : MonoBehaviour
{
    [SerializeField] TMP_InputField passwordIpf;
    public void ShowPassword()
    {
        passwordIpf.contentType = TMP_InputField.ContentType.Standard;
        passwordIpf.ForceLabelUpdate();
    }
    public void HidePassword()
    {
        passwordIpf.contentType = TMP_InputField.ContentType.Password;
        passwordIpf.ForceLabelUpdate();
    }
}
