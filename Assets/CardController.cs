using DG.Tweening;
using JetBrains.Annotations;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardController : MonoBehaviour, IPooleableGameObject, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields

    public bool ReadyToUse { get; set; }
    public Transform Parent { get; set; }

    [SerializeField] private Image _cardImage;

    private SO_Card _cardData; public SO_Card CardData { get { return _cardData; } }
    private SO_CardSettings _cardSettings;

    private bool _isActive;
    private bool _isCleaned; public bool IsCleaned { get { return _isCleaned; } }
    private bool _isSelected; public bool IsSelected { get { return _isSelected; } set { _isSelected=value; } }


    private Vector3 _faceUpRotation = new Vector3(0, 180, 0);
    private Vector3 _faceDownRotation = Vector3.zero;

    #endregion

    #region Intialization

    public void Initialize(SO_Card pData, SO_CardSettings pSettings, bool pIsCleaned = false, bool pIsSelected = false, bool pIsNewGame=true)
    {
        _cardData = pData;
        _cardSettings = pSettings;

        _cardImage.sprite = pData.Image;


        _isSelected = pIsSelected;
        if (_isSelected)
            RotateInstantly(true);
        else
            RotateInstantly(false);

        if (pIsNewGame)
            RotateInstantly(true);

        _isCleaned = pIsCleaned;
        if (_isCleaned)
        {
            _isActive = false;
            transform.localScale = Vector3.zero;
        }
        else
        {
            _isActive = true;
            transform.localScale = Vector3.one;
        }

        ReadyToUse = false;
        gameObject.SetActive(true);
    }

    #endregion

    #region Mouse Event Handlers

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_isActive)
        {
            RotateToFaceup();
            _isActive = false;
            EventManager.CardRotated(this);
        }
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

    public async void CardMatched()
    {
        _isSelected = false;
        _isCleaned = true;
        await Task.Delay((int)(_cardSettings.TimeBeforeDissapear * 1000));
        DisappearCard();
    }

    public async void CardNotMatched()
    {
        _isSelected = false;
        _isActive = true;
        await Task.Delay((int)(_cardSettings.TimeBeforeBackToFaceDown * 1000));
        RotateToFaceDown();
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


    public void RotateCard(bool pFaceUp, bool pPlaySound=true) 
    {
        if (pFaceUp)
            RotateToFaceup(pPlaySound);
        else
            RotateToFaceDown(pFaceUp);
    }

    private void RotateToFaceup(bool pPlaySound = true)
    {
        if (pPlaySound)
            EventManager.GenerateSound(_cardSettings.SoundOnRotate);

        transform.DORotate(_faceUpRotation, _cardSettings.RotationDuration).OnUpdate
            (() =>
            {
                if (transform.rotation.eulerAngles.y > 90)
                    _cardImage.enabled = true;
            });
    }

    private void RotateToFaceDown(bool pPlaySound = true)
    {
        if (pPlaySound)
            EventManager.GenerateSound(_cardSettings.SoundOnRotate);

        transform.DORotate(_faceDownRotation, _cardSettings.RotationDuration).OnUpdate
             (() =>
             {
                 if (transform.rotation.eulerAngles.y < 90)
                     _cardImage.enabled = false;
             });
    }

    public void RotateInstantly(bool pFaceUp) 
    {
        if (pFaceUp) 
        {
            transform.rotation = Quaternion.Euler(_faceUpRotation);
            _cardImage.enabled = true;
        }
        else 
        {
            transform.rotation = Quaternion.Euler(_faceDownRotation);
            _cardImage.enabled = false;
        }
    }

    private void DisappearCard()
    {
        _isActive = false;
        transform.DOScale(Vector3.zero, _cardSettings.DissaparitionDuration);
    }

    #endregion

}
