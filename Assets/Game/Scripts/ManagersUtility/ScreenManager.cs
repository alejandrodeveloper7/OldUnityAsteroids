using UnityEngine;

public static class ScreenManager
{
    public static void FixFrameRate(int pValue)
    {
        Application.targetFrameRate = pValue;
    }
}
