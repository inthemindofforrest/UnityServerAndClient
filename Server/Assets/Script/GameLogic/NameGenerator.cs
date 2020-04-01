using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NameGenerator
{
    static string[] Names = new string[] { "Jill", "Bob" };
    public static string GetRandomName
    {
        get
        {
            return (Names[Random.Range(0, Names.Length)]);
        }
    }
}
