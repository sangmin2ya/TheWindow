using System;
using UnityEngine;

[Serializable]
public class ChatSet
{
    public string name;
    [TextArea(10, 100)]
    public string message;
}