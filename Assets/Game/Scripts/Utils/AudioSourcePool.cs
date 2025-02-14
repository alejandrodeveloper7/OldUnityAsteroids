using System;
using Unity.VisualScripting;
using UnityEngine;

public class AudioSourcePool
{
    //This is a variable of the Gameobject pool, adapted to create a object and add to it audiosources for the 2DSound, I should
    //create inheritance and combine the common parts in an abstract parent class, but copy the other class and modify it is quicker
    //for the test

    private AudioSource[] _availableInstances;
    private Transform _parent;
    private int _increment;

    public AudioSourcePool(int initialSize, int increment)
    {
        if (increment <= 0)
            increment = 1;

        _increment = increment;

        _availableInstances = new AudioSource[0];
        CreateParent();

        if (initialSize > 0)
            ExpandPoolSize(initialSize);
    }


    #region public Methods

    public AudioSource GetInstance()
    {
        AudioSource instance = ObteinReadyInstance();

        if (instance == null)
        {
            ExpandPoolSize(_increment);
            instance = ObteinReadyInstance();
        }

        return instance;
    }

    #endregion

    #region Internal Logic

    private void CreateParent()
    {
        _parent = new GameObject("Pooled2DAudioSources").transform;
    }

    private AudioSource ObteinReadyInstance()
    {
        foreach (AudioSource audioSource in _availableInstances)
            if (audioSource.isPlaying is false)
                return audioSource;

        return null;
    }

    internal AudioSource CreateNewInstance()
    {
        AudioSource newInstance = _parent.AddComponent<AudioSource>();
        return newInstance;
    }

    internal void ExpandPoolSize(int pIncrement)
    {
        int firstPosition = _availableInstances.Length;
        Array.Resize(ref _availableInstances, firstPosition + pIncrement);

        for (int i = firstPosition; i < _availableInstances.Length; i++)
            _availableInstances[i] = CreateNewInstance();
    }
    #endregion
}
