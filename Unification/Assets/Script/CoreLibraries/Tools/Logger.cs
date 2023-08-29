using System;
using System.Collections.Generic;
using UnityEngine;

public enum LogModule
{
    Common = 1,
}

public enum LogColor
{
    White,
    Red,
    Orange,
    Yellow,
    Green,
    Cyan,
    Blue,
    Purple,
}

public class Logger
{
    private static readonly Dictionary<LogColor, string> ColorDic = new Dictionary<LogColor, string>()
    {
        { LogColor.White, "#ffffff" },
        { LogColor.Red, "#ff0000" },
        { LogColor.Orange, "#ffa500" },
        { LogColor.Yellow, "#ffff00" },
        { LogColor.Green, "#008000" },
        { LogColor.Cyan, "#00ffff" },
        { LogColor.Blue, "#0000ff" },
        { LogColor.Purple, "#800080" },
    };

    public static void Log(object message, LogModule module = LogModule.Common, LogColor color = LogColor.White)
    {
        var modulePrefix = $"[{module}]";
        var colorHex = ColorDic[color];
        var msg = $"<color={colorHex}>{modulePrefix} {message}</color>";
        Debug.Log(msg);
    }

    public static void Warning(object message, string module = null, LogColor color = LogColor.White)
    {
        var modulePrefix = $"[{module}]";
        var colorHex = ColorDic[color];
        var msg = $"<color={colorHex}>{modulePrefix} {message}</color>";
        Debug.LogWarning(msg);
    }

    public static void Error(object message, string module = null, LogColor color = LogColor.White)
    {
        var modulePrefix = $"[{module}]";
        var colorHex = ColorDic[color];
        var msg = $"<color={colorHex}>{modulePrefix} {message}</color>";
        Debug.LogError(msg);
    }
}