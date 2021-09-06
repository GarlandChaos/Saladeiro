using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorCode
{
    Red,
    Green,
    Blue
}

[CreateAssetMenu(fileName = "Color Property Settings", menuName = "Settings/Color Property Settings")]
public class ColorPropertySettings : ScriptableObject
{
    [SerializeField]
    ColorCode colorCode = ColorCode.Red;
    [SerializeField]
    Material material = null;

    public ColorCode _ColorCode { get => colorCode; }
    public Material _Material { get => material; }
}
