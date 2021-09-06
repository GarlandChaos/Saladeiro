using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRoom : MonoBehaviour
{
    [SerializeField]
    ColorPropertySettings colorSettings = null;
    public ColorCode color { get { return colorSettings._ColorCode; } }

    private void Awake()
    {
        SetColorPropertySettings(colorSettings);
    }

    void SetColorPropertySettings(ColorPropertySettings colorPropertySettings)
    {
        foreach (Renderer rend in GetComponentsInChildren<Renderer>())
        {
            rend.sharedMaterial = colorPropertySettings._Material;
        }
    }


#if UNITY_EDITOR
    private void OnValidate()
    {
        SetColorPropertySettings(colorSettings);
    }
#endif
}
