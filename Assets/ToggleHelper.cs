using TMPro;
using UnityEngine;

public class ToggleHelper : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _toggleText;

    public void SetText(string ptext)
    {
        _toggleText.text = ptext;
    }
}
