using System;
using UnityEngine;

public interface IPooleableGameObject
{
    bool ReadyToUse { get; set; }
    Transform Parent {  get; set; }
}

public class GameObjectPool
{
    #region Variables

    [Header("Fields")]
    private GameObject[] _availableInstances;
    private readonly GameObject _objectPooled;
    private readonly Transform _parent;
    private int _increment;

    #endregion

    #region Constuctor

    public GameObjectPool(GameObject objectPooled, Transform parent, int initialSize, int increment)
    {
        if (increment <= 0)
            increment = 1;

        _availableInstances = new GameObject[0];
        _objectPooled = objectPooled;
        _parent = parent;
        _increment = increment;

        if (initialSize > 0)
            ExpandPool(initialSize);
    }

    #endregion

    #region public Methods

    public GameObject GetInstance()
    {
        GameObject instance = GetReadyInstance();

        if (instance == null)
        {
            ExpandPool(_increment);
            instance = GetReadyInstance();
        }

        instance.transform.SetParent(null);
        instance.GetComponent<IPooleableGameObject>().ReadyToUse = false;

        return instance;
    }

    #endregion

    #region Internal Logic

    private GameObject GetReadyInstance()
    {
        for (int i = 0; i < _availableInstances.Length; i++)
            if (_availableInstances[i].GetComponent<IPooleableGameObject>().ReadyToUse)
                return _availableInstances[i];

        return null;
    }

    internal GameObject CreateNewInstance()
    {
        GameObject newInstance = UnityEngine.Object.Instantiate(_objectPooled, _parent);
        newInstance.SetActive(false);

        IPooleableGameObject pooleableItem = newInstance.GetComponent<IPooleableGameObject>();
        pooleableItem.ReadyToUse = true;
        pooleableItem.Parent = _parent;

        return newInstance;
    }

    internal void ExpandPool(int pIncrement)
    {
        int firstPosition = _availableInstances.Length;
        Array.Resize(ref _availableInstances, firstPosition + pIncrement);

        for (int i = firstPosition; i < _availableInstances.Length; i++)
            _availableInstances[i] = CreateNewInstance();
    }

    #endregion

}
