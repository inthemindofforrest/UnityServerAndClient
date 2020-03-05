using System;
using System.Linq;
using System.Text.RegularExpressions;

public static class Utility
{
    public const string EMAIL_PATTERN = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*" + "@" + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
    public const string USERNAME_AND_DISCRIMINATOR_PATTERN = @"^[a-zA-Z0-9]{4,20}#[0-9]{4}$";
    public const string USERNAME_PATTERN = @"^[a-zA-Z0-9]{4,20}$";
    public const string RANDOM_CHARS = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";


    public static bool IsEmail(string _Email)
    {
        if (_Email != null)
        {
            return Regex.IsMatch(_Email, EMAIL_PATTERN);
        }
        else
            return false;
    }
    public static bool IsUsernameAndDiscriminator(string _Username)
    {
        if (_Username != null)
        {
            return Regex.IsMatch(_Username, USERNAME_AND_DISCRIMINATOR_PATTERN);
        }
        else
            return false;
    }
    public static bool IsUsername(string _Username)
    {
        if (_Username != null)
        {
            return Regex.IsMatch(_Username, USERNAME_PATTERN);
        }
        else
            return false;
    }
    public static string GenerateRandom(int _Length)
    {
        Random r = new Random();
        return new string(Enumerable.Repeat(RANDOM_CHARS, _Length).Select(s => s[r.Next(s.Length)]).ToArray());
    }
    public static string Sha256FromString(string _ToEncrypt)
    {
        var Message = System.Text.Encoding.UTF8.GetBytes(_ToEncrypt);
        System.Security.Cryptography.SHA256Managed HashString = new System.Security.Cryptography.SHA256Managed();

        string Hex = "";
        var HashValue = HashString.ComputeHash(Message);
        foreach (byte x in HashValue)
            Hex += String.Format("{0:x2}", x);
        return Hex;
    }
}
