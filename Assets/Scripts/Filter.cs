using UnityEngine;
using System.Collections;

[System.Flags]
public enum Filter
{
    None = 0,
    Red = 1,
    Green = 2,
    Blue = 4,
    RedExcept = Green | Blue,
    GreenExcept = Red | Blue,
    BlueExcept = Red | Green,
    All = None | Red | Green | Blue
}