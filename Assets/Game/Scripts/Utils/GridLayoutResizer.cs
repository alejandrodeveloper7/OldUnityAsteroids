using UnityEngine;
using UnityEngine.UI;

public class GridLayoutResizer : MonoBehaviour
{
    private GridLayoutGroup _gridLayout;
    private RectTransform _gridLayoutRectTransform;

    private bool _isActive;

    private int _totalCards; 
    private int _columns; 
    private int _rows;

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

    public void RecalculateCellsSize()
    {
        float usableHeight = _gridLayoutRectTransform.rect.height - (_gridLayout.spacing.y * (_rows - 1)) - _gridLayout.padding.top - _gridLayout.padding.bottom;
        float cellHeight = usableHeight / _rows;

        float usableWidth = _gridLayoutRectTransform.rect.width - (_gridLayout.spacing.x * (_columns - 1)) - _gridLayout.padding.left - _gridLayout.padding.right;
        float cellWidth = usableWidth / _columns;

        //If the space for the width is too small for adapt to the height size, use the width instead the height for match in the space.
        if (cellWidth < cellHeight)
            cellHeight = cellWidth;
        else
            cellWidth = cellHeight;

        _gridLayout.cellSize = new Vector2(cellWidth, cellHeight);
    }
}
