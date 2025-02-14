
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : ViewBase
{
    #region Fields

    [Header("General")]
    [SerializeField] private GameObject _generalContainer;

    [Header("Sliders")]
    [SerializeField] private Slider _columnsSlider;
    [SerializeField] private TextMeshProUGUI _columnsText;
    public float ColumnsAmount { get { return _columnsSlider.value; } }
    [Space]
    [SerializeField] private Slider _rowsSlider;
    [SerializeField] private TextMeshProUGUI _rowsText;
    public float RowsAmount { get { return _rowsSlider.value; } }
    [Space]
    private int _minColumnsValue;
    private int _columnsMinSelectableValue;
    private int _maxColumnsValue;
    [Space]
    private int _minRowsValue;
    private int _rowsMinSelectableValue;
    private int _maxRowsValue;

    [Header("Difficulty Toggles")]
    [SerializeField] private ToggleGroup _difficultyToggleGroup;
    [SerializeField] private Transform _difficultyTogglesContainer;
    [Space]
    private GameObject _difficultyTogglePrefab;
    [Space]
    private List<Toggle> _currentDifficultyToggles= new List<Toggle>();
    public int DifficultyIndexSelected 
    { 
        get 
        {
            for (int i = 0; i < _currentDifficultyToggles.Count; i++)
            {
                if (_currentDifficultyToggles[i].isOn)
                    return i;
            }

            Debug.LogError("No difficulty Selected");
            return 0;
        }
    }

    [Header("Buttons")]
    [SerializeField]private Button _playButton;

    #endregion

    #region Monobehaviour

    private void OnEnable()
    {
        _rowsSlider.onValueChanged.AddListener(RowsSliderValueChanged);
        _columnsSlider.onValueChanged.AddListener(ColumnsSliderValueChanged);
    }

    private void OnDisable()
    {
        _rowsSlider.onValueChanged.RemoveListener(RowsSliderValueChanged);
        _columnsSlider.onValueChanged.RemoveListener(ColumnsSliderValueChanged);
    }

    #endregion

    #region Public Methods

    public void TurnGeneralContainer(bool pState)
    {
        _generalContainer.SetActive(pState);
    }

    public void SetData(MainMenuModel pModel)
    {
        _minColumnsValue = pModel.MinColumnsValue;
        _maxColumnsValue = pModel.MaxColumnsValue;
        _columnsMinSelectableValue = pModel.ColumnsMinSelectebleValue;

        _minRowsValue = pModel.MinRowsValue;
        _maxRowsValue = pModel.MaxRowsValue;
        _rowsMinSelectableValue = pModel.RowsMinSelectebleValue;

        _difficultyTogglePrefab = pModel.DifficultyTogglePrefab;
    }

    public void ConfigureSliders()
    {
        _columnsSlider.minValue = _minColumnsValue;
        _columnsSlider.maxValue = _maxColumnsValue;
        _columnsSlider.value = _columnsMinSelectableValue;

        _rowsSlider.minValue = _minRowsValue;
        _rowsSlider.maxValue = _maxRowsValue;
        _rowsSlider.value = _rowsMinSelectableValue;

        CheckCardsAmountInBoard();
    }

    public void GenerateDifficultyToggles(List<SO_Difficulty> pDifficultys)
    {
        for (int i = 0; i < pDifficultys.Count; i++)
        {
            Toggle newToggle = Instantiate(_difficultyTogglePrefab, _difficultyTogglesContainer).GetComponent<Toggle>();
            newToggle.group = _difficultyToggleGroup;
            newToggle.GetComponent<ToggleHelper>().SetText(pDifficultys[i].Name);

            if (i == 0)
                newToggle.isOn = true;

            _currentDifficultyToggles.Add(newToggle);
        }
    }

    #endregion

    #region Private Methods

    private void ColumnsSliderValueChanged(float pValue) 
    {
        if (pValue < _columnsMinSelectableValue)
            _columnsSlider.value = _columnsMinSelectableValue;

        _columnsText.text = _columnsSlider.value.ToString();
        CheckCardsAmountInBoard();
    }
    private void RowsSliderValueChanged(float pValue)
    {
        if (pValue < _rowsMinSelectableValue)
            _rowsSlider.value = _rowsMinSelectableValue;

        _rowsText.text = _rowsSlider.value.ToString();
        CheckCardsAmountInBoard();
    }

    private void CheckCardsAmountInBoard()
    {
        if ((_columnsSlider.value * _rowsSlider.value) % 2 == 0)
            _playButton.interactable = true;
        else
            _playButton.interactable = false;
    }

    #endregion
}
