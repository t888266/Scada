using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UpdateInfor : MonoBehaviour
{
    [SerializeField] GameObject successfulllyUpdatePanel;
    [SerializeField] TextMeshProUGUI usernameTxt;
    [SerializeField] TextMeshProUGUI emailTxt;
    [SerializeField] TMP_InputField currentPasswordTxt;
    [SerializeField] TMP_InputField newPasswordTxt;
    [SerializeField] TMP_InputField newEmailTxt;
    [SerializeField] TMP_InputField newUsernameTxt;

    public void UpdateInfo()
    {
        if (newPasswordTxt.gameObject.activeInHierarchy)
        {
            UpdatePassword();
        }
        else if (newEmailTxt.gameObject.activeInHierarchy)
        {
            UpdateEmail();
        }
        else
        {
            UpdateUsername();
        }
    }
    void OnUpdateSuccessfully()
    {
        successfulllyUpdatePanel.SetActive(true);
    }
    void UpdatePassword()
    {
        if (!currentPasswordTxt.text.Equals(AccountData.Instance.UserModel.Password))
        {
            HelperUI.Instance.ShowErrorUI("The password you entered is incorrect. Please try again.");
            return;
        }
        CheckValueValid checkPass = new CheckPasswordValid(newPasswordTxt.text);
        if (!checkPass.IsValid())
        {
            checkPass.ShowErrorMessage();
            return;
        }
        StartCoroutine(Helper.AccountHelper.UpdateRequest("password", newPasswordTxt.text, AccountData.Instance.UserModel.UserKey, () =>
        {
            successfulllyUpdatePanel.SetActive(true);
            AccountData.Instance.UserModel.Password = newPasswordTxt.text;
        }));
    }
    void UpdateUsername()
    {
        if (!currentPasswordTxt.text.Equals(AccountData.Instance.UserModel.Password))
        {
            HelperUI.Instance.ShowErrorUI("The password you entered is incorrect. Please try again.");
            return;
        }
        CheckValueValid checkFill = new CheckForFillRequiredField(newUsernameTxt.text);
        if (!checkFill.IsValid())
        {
            checkFill.ShowErrorMessage();
            return;
        }
        StartCoroutine(Helper.AccountHelper.UpdateRequest("username", newUsernameTxt.text, AccountData.Instance.UserModel.UserKey, () =>
        {
            successfulllyUpdatePanel.SetActive(true);
            usernameTxt.text = $"Username:\n{newUsernameTxt.text}";
            AccountData.Instance.UserModel.Username = newUsernameTxt.text;
        }));
    }
    void UpdateEmail()
    {
        if (!currentPasswordTxt.text.Equals(AccountData.Instance.UserModel.Password))
        {
            HelperUI.Instance.ShowErrorUI("The password you entered is incorrect. Please try again.");
            return;
        }
        CheckValueValid checkEmail = new CheckEmailValid(newEmailTxt.text);
        if (!checkEmail.IsValid())
        {
            checkEmail.ShowErrorMessage();
            return;
        }
        StartCoroutine(Helper.AccountHelper.UpdateRequest("email", newEmailTxt.text, AccountData.Instance.UserModel.UserKey, () =>
        {
            successfulllyUpdatePanel.SetActive(true);
            emailTxt.text = $"Email:\n{newEmailTxt.text}";
            AccountData.Instance.UserModel.Email = newEmailTxt.text;
        },
        (rq) =>
        {
            if (rq.responseCode == 409)
            {
                HelperUI.Instance.ShowErrorUI("The email has already been taken");
            }
        }));
    }
    public void Reset()
    {
        currentPasswordTxt.text = string.Empty;
        newPasswordTxt.text = string.Empty;
        newUsernameTxt.text = string.Empty;
        newEmailTxt.text = string.Empty;
    }
    private void OnDisable()
    {
        Reset();
    }
}
