using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IPooleableGameObject, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields

    public bool ReadyToUse { get; set; }
    public Transform Parent { get; set; }

    [SerializeField] private Image _cardImage;

    private SO_Card _cardData;
    private SO_CardSettings _cardSettings;

    private bool _isActive;
    private bool _isFaceup;

    private Vector3 _faceUpRotation = new Vector3(0, 180, 0);
    private Vector3 _faceDownRotation = Vector3.zero;

    #endregion

    #region Intialization

    public void Initialize(SO_Card pData, SO_CardSettings pSettings)
    {
        _cardData = pData;
        _cardSettings = pSettings;

        _cardImage.sprite = pData.Image;

        _isActive = true;
        //_isSelected = false;

        ReadyToUse = false;
        transform.rotation = Quaternion.Euler(_faceUpRotation);
        gameObject.SetActive(true);
    }

    #endregion

    #region Mouse Event Handlers

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isActive)
            Debug.Log("CLICK");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ApplyHoverState(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ApplyHoverState(true);
    }


    #endregion

    #region CardManagement

    public void CleanCard()
    {
        transform.DOKill();
        //Stop and reset
        RecicleInstance();
    }

    private void RecicleInstance()
    {
        transform.SetParent(Parent);
        transform.localPosition = Vector3.zero;
        ReadyToUse = false;
        gameObject.SetActive(false);
    }

    #endregion


    #region Movement Management

    private void ApplyHoverState(bool pSate)
    {
        if (pSate)
            transform.DOScale(_cardSettings.HoverScale, _cardSettings.HoverDuration);
        else
            transform.DOScale(Vector3.one, _cardSettings.HoverDuration);

    }

    public void RotateToFaceup()
    {
        transform.DORotate(_faceUpRotation, _cardSettings.RotationDuration).OnUpdate
            (() =>
            {
                if (transform.rotation.eulerAngles.y > 90)
                    _cardImage.enabled = true;
            });
    }

    public void RotateToFaceDown()
    {
        transform.DORotate(_faceDownRotation, _cardSettings.RotationDuration).OnUpdate
             (() =>
             {
                 if (transform.rotation.eulerAngles.y < 90)
                     _cardImage.enabled = false;
             });
    }

    private void DisappearCard()
    {
        _isActive = false;
        transform.DOScale(Vector3.zero, _cardSettings.DissaparitionDuration);
    }

    #endregion

}
