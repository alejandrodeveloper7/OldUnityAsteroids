
using UnityEngine;

public class GameStatsView : ViewBase
{
    #region Fields

    [SerializeField] private GameObject _generalContainer;
    [SerializeField] private GameObject _BackToMainMenuButton;


    
    #endregion

    #region Public Methods

    public void TurnGeneralContainer(bool pState)
    {
        _generalContainer.SetActive(pState);
    }

    public void TurnBackToMainMenuButton(bool pSate)
    {
        _BackToMainMenuButton.SetActive(pSate);
    }

    #endregion
}
