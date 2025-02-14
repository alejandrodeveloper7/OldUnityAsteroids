
using UnityEngine;

public class GameStatsView : ViewBase
{
    #region Fields

    [SerializeField] private GameObject _generalContainer;

    #endregion

    #region Public Methods

    public void TurnGeneralContainer(bool pState)
    {
        _generalContainer.SetActive(pState);
    }

    #endregion
}
