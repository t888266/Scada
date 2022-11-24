using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UserProfileView : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI usernameTxt;
    [SerializeField] TextMeshProUGUI emailTxt;
    private void Awake()
    {
        usernameTxt.text += AccountData.Instance.UserModel.Username;
        emailTxt.text += AccountData.Instance.UserModel.Email;
    }
}
