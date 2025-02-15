using System;
using System.Collections.Generic;
using static EventManager;

public static class EventManager
{
    public interface IEvent
    {
    }

    private static readonly Dictionary<Type, Delegate> events = new();

    #region EventsManagement

    public static void SubscribeEvent<T>(Action<T> pCalback) where T : IEvent
    {
        Type type = typeof(T);
        if (!events.ContainsKey(type))
            events[type] = null;

        events[type] = (Action<T>)events[type] + pCalback;
    }

    public static void UnsubscribeEvent<T>(Action<T> pCallback) where T : IEvent
    {
        Type type = typeof(T);
        if (events.TryGetValue(type, out var existingDelegate))
        {
            events[type] = (Action<T>)existingDelegate - pCallback;
            if (events[type] == null) events.Remove(type);
        }
    }

    public static void RaiseEvent<T>(T pData) where T : IEvent
    {
        Type type = typeof(T);
        if (events.TryGetValue(type, out var existingDelegate) && existingDelegate is Action<T> action)
            action.Invoke(pData);
    }

    #endregion
}

#region IEvents

public class GameStart : IEvent
{
}
public class BackMainMenu : IEvent
{
}


public class LeaveStage : IEvent
{
}


public class StageStart : IEvent
{
}
public class StageFinish : IEvent
{
}


public class SaveGame : IEvent
{
}
public class GameLoad : IEvent
{
    public GameState State { get; set; }
}
public class CleanSaveData : IEvent
{
}


public class StatsUpdate : IEvent
{
    public int Score { get; set; }
    public int ComboMultiplier { get; set; }
    public int Movements { get; set; }
}


public class BoardGeneration : IEvent
{
    public List<CardController> Board { get; set; }
}


public class CardRotated : IEvent
{
    public CardController CardController { get; set; }
}


public class MatchStart : IEvent
{
    public List<CardController> Board { get; set; }
}
public class MatchSuccess : IEvent
{
    public List<CardController> Board { get; set; }
    public SO_Difficulty Difficulty { get; set; }
}
public class MatchFail : IEvent
{
    public List<CardController> Board { get; set; }
}


public class Generate2DSound : IEvent
{
    public SO_Sound Sound { get; set; }
}

#endregion
