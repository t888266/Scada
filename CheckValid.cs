using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
public abstract class CheckValueValid
{
    protected string data;
    protected string errorMessage;
    protected CheckValueValid(string data)
    {
        this.data = data;
    }

    public abstract bool IsValid();
    public void ShowErrorMessage()
    {
        HelperUI.Instance.ShowErrorUI(errorMessage);
    }
}
public class CheckForFillRequiredField : CheckValueValid
{
    public CheckForFillRequiredField(string data) : base(data)
    {
        errorMessage = "Please fill in a valid value for all required fields!";
    }

    public override bool IsValid()
    {
        return !string.IsNullOrEmpty(data);
    }
}
public class CheckPasswordValid : CheckValueValid
{
    public CheckPasswordValid(string data) : base(data)
    {
        errorMessage = "Password must be at least 6 to 21 characters in length.";
    }

    public override bool IsValid()
    {
        return data.Length >= 6;
    }
}
public class CheckCodeValid : CheckValueValid
{
    public CheckCodeValid(string data) : base(data)
    {
        errorMessage = "Enter the code in correct format with 6 numbers.";
    }

    public override bool IsValid()
    {
        return data.Length == 6;
    }
}
public class CheckEmailValid : CheckValueValid
{
    public CheckEmailValid(string data) : base(data)
    {
        errorMessage = "Enter an email address in correct format, like name@example.com";
    }

    public override bool IsValid()
    {
        if (string.IsNullOrWhiteSpace(data))
        {
            return false;
        }
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(data);
        if (!match.Success)
        {
            return false;
        }
        return true;
    }
}
public class CheckNumber : CheckValueValid
{
    public CheckNumber(string data) : base(data)
    {
        errorMessage = "Please enter a numeric value.";
    }

    public override bool IsValid()
    {
        return float.TryParse(data, out float number);
    }
}
