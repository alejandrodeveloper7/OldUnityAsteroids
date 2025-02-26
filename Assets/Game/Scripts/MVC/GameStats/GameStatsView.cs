using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameStatsView : ViewBase
{
    #region Fields

    [Header("General")]
    [SerializeField] private GameObject _generalContainer;
    [SerializeField] private GameObject _BackToMainMenuButton;

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI _scoreText;

    [Header("Combo")]
    [SerializeField] private Transform _comboContainer;
    private Sequence _comboSequence;
    [Space]
    [SerializeField] private TextMeshProUGUI _comboText;
    [SerializeField] private TextMeshProUGUI _comboScoreText;
    [Space]
    [SerializeField] private TextMeshProUGUI _movementsText;

    [Header("Values")]
    private float _combosScaleDuration;
    private float _combosDisplayDuration;

    #endregion

    #region Initialization

    public void SetData(GameStatsModel pModel)
    {
        _combosScaleDuration = pModel.CombosScaleDuration;
        _combosDisplayDuration = pModel.CombosDisplayDuration;
    }

    #endregion

    #region General Elements Management

    public void TurnGeneralContainer(bool pState)
    {
        _generalContainer.SetActive(pState);
    }
    public void TurnBackToMainMenuButton(bool pSate)
    {
        _BackToMainMenuButton.SetActive(pSate);
    }

    #endregion

    #region StatsManagement

    public void SetScore(int pNewScore)
    {
        _scoreText.text = pNewScore.ToString();
    }

    public void SetComboAmount(int pComboPoints, int pComboMultiplier)
    {
        _comboText.text = string.Format("Combo x{0}", pComboMultiplier.ToString());
        _comboScoreText.text = pComboPoints.ToString();
    }
    public void DisplayComboAnimation()
    {
        if (_comboSequence != null)
            _comboSequence.Kill();

        RestartComboAnimation();

        _comboSequence = DOTween.Sequence()
            .Append(_comboContainer.DOScale(1, _combosScaleDuration))
            .AppendInterval(_combosDisplayDuration)
            .Append(_comboContainer.DOScale(0, _combosScaleDuration));
    }
    public void RestartComboAnimation()
    {
        if (_comboSequence != null)
            _comboSequence.Kill();

        _comboContainer.localScale = Vector3.zero;
    }

    public void SetMovements(int pNewMovements)
    {
        _movementsText.text = pNewMovements.ToString();
    }

    #endregion
}
