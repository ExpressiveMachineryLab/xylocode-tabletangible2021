using System.Collections;
using UnityEngine;


public class JsonConfig
{
    public string Name;
    public double SensitivityMultiplier;
    public JsonRecovery Recovery;
    public JsonSmoothing Smoothing;
    public int Pending3PointMatchDelay;
}

public struct JsonRecovery
{
    public float Timeout;
    public double TangibleDistance;
    public double PointDistance;
}

public struct JsonSmoothing
{
    public int PositionHistory;
    public int RotationHistory;
}