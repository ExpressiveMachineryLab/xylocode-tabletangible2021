using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class TangibleGameController : MonoBehaviour
{
    public static TangibleGameController Singleton;
    public List<TangibleController> Tangibles = new List<TangibleController>();

    public enum TargetColors
    {
        Red = 0,
        Green = 1,
        Blue = 2,
        Yellow = 3
    }

    public List<Color> RenderColor = new List<Color>(Enum.GetNames(typeof(TargetColors)).Length);
    
    public Dictionary<TargetColors, Color> ColorBinding =  new Dictionary<TargetColors, Color>();
    
    public enum RuleType
    {
        Next,
        Previous,
        Repeat
    }
    public float RotationSpeed = 200f;
    
    public GameObject BulletPrefab;
    
    
    void Awake()
    {
        for (int i = 0; i < RenderColor.Count; i++)
        {
            ColorBinding.Add((TargetColors)i, RenderColor[i]);
        }

        Tangibles.AddRange(FindObjectsOfType<TangibleController>());
    }

    
    // Start is called before the first frame update
    void Start()
    {
        if (Singleton == null)
        {
            Singleton = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    public void AddTangible(TangibleController t)
    {
        Tangibles.Add(t);
    }

    public void RemoveTangible(TangibleController t)
    {
        if (Tangibles.Contains(t))
        {
            Tangibles.Remove(t);
        }
        else
        {
            Debug.Log("CAUTION: Attempt to remove non-existing tangible");
        }
    }

    public static Dictionary<TargetColors, ElemColor> ColorConvertDict = new Dictionary<TargetColors, ElemColor>
    {
        {TargetColors.Red, ElemColor.red},
        {TargetColors.Blue, ElemColor.blue},
        {TargetColors.Yellow, ElemColor.yellow},
        {TargetColors.Green, ElemColor.green}
    };

    public static Color GetColor(ElemColor color)
    {
        switch (color)
        {
            case ElemColor.red:
                return Singleton.ColorBinding[TargetColors.Red];
            case ElemColor.yellow:
                return Singleton.ColorBinding[TargetColors.Yellow];
            case ElemColor.blue:
                return Singleton.ColorBinding[TargetColors.Blue];
            case ElemColor.green:
                return Singleton.ColorBinding[TargetColors.Green];
            default:
                return Color.white;
        }
    }

    public static Dictionary<ElemColor, Color> lightHighlightColor = new Dictionary<ElemColor, Color>()
    {
        { ElemColor.red,  new Color(1f, 0f, 0.23f) },
        { ElemColor.blue, new Color(0f, 0.7f, 1f) },
        { ElemColor.green, new Color(0.77f, 0.1f, 1f) },
        { ElemColor.yellow, new Color(1f, 0.77f, 0f) }
    };

    public static Dictionary<ElemColor, Color> darkHighlightColor = new Dictionary<ElemColor, Color>()
    {
        { ElemColor.red, new Color(0.06f, 0, 0.07f) },
        { ElemColor.blue, new Color(0.1f, 0f, 0.35f) },
        { ElemColor.green, new Color(0f, 0.15f, 0.18f) },
        { ElemColor.yellow, new Color(1f, 0.46f, 0f) }
    };
}
