using UnityEngine;
using UnityEngine.UI;

public class GridLayoutController : MonoBehaviour
{
    #region Fields

    [Header("References")]
    private GridLayoutGroup _gridLayout;
    private RectTransform _gridLayoutRectTransform;

    [Header("States")]
    private bool _isActive;

    [Header("Data")]
    private int _columns;
    private int _rows;
    [Space]
    private int _totalCards;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        _gridLayout = GetComponent<GridLayoutGroup>();
        _gridLayoutRectTransform = _gridLayout.GetComponent<RectTransform>();
    }

    private void OnRectTransformDimensionsChange()
    {
        if (_isActive)
            RecalculateCellsSize();
    }

    #endregion

    #region Management

    public void SetData(float pRows, float pColumns)
    {
        _columns = Mathf.RoundToInt(pColumns);
        _rows = Mathf.RoundToInt(pRows);
        _totalCards = _columns * _rows;

        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _gridLayout.constraintCount = _rows;
    }

    public void SetState(bool pState)
    {
        _isActive = pState;

        if (_isActive)
            RecalculateCellsSize();
    }

    #endregion

    #region Functionality

    public void RecalculateCellsSize()
    {
        if (_totalCards == 0)
            return;

        float unUsableHeight = (_gridLayout.spacing.y * (_rows - 1)) + _gridLayout.padding.top + _gridLayout.padding.bottom;
        float usableHeight = _gridLayoutRectTransform.rect.height - unUsableHeight;
        float cellHeight = usableHeight / _rows;

        float unUsableWidth = (_gridLayout.spacing.x * (_columns - 1)) + _gridLayout.padding.left + _gridLayout.padding.right;
        float usableWidth = _gridLayoutRectTransform.rect.width - unUsableWidth;
        float cellWidth = usableWidth / _columns;

        //If the space for the width is too small for adapt to the height size, use the width instead the height for match in the space.
        if (cellWidth < cellHeight)
            cellHeight = cellWidth;
        else
            cellWidth = cellHeight;

        _gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }

    #endregion
}
