using DG.Tweening;
using UnityEngine;

public class GameEndView : ViewBase
{
    #region Fields

    [SerializeField] private GameObject _generalContainer;
    [SerializeField] private Transform _backToMainMenuButton;
  
    #endregion

    #region Public Methods

    public void TurnGeneralContainer(bool pState)
    {
        _generalContainer.SetActive(pState);
    }

    public void TurnBackToMainMenuButton(bool pSate, float pDuration=0)
    {
        if (pSate)
            _backToMainMenuButton.DOScale(Vector3.one, pDuration);
        else
            _backToMainMenuButton.localScale = Vector3.zero;
    }

    #endregion
}
