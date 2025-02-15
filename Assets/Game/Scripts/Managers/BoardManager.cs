using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField] private Transform _cardsContainer;
    private GridLayoutResizer _gridLayoutResizer;
    [Space]
    private Transform _pooledCardsParent;
    private GameObjectPool _cardsPool;
    [Space]
    private List<CardController> _currentcards = new List<CardController>();
    private CardController _currentRotatedCard;

    [Header("Data")]
    private SO_BoardSettings _boardSettings;
    private SO_CardSettings _cardSetting;
    private SO_Difficulty _currentDifficulty;
    [Space]
    private List<SO_Card> _availableCards;

    #endregion

    #region monobehaviour

    private void Awake()
    {
        GetReferences();

        CreatePoolParent();
        CreateCardsPool();
    }

    private void OnEnable()
    {
        EventManager.OnGameStateLoaded += GameStateLoaded;

        EventManager.OnStageStarted += StageStarted;
        EventManager.OnStageLeaved += StageLeaved;
        EventManager.OnCardRotated += CardRotated;
    }

    private void OnDisable()
    {
        EventManager.OnGameStateLoaded -= GameStateLoaded;

        EventManager.OnStageStarted -= StageStarted;
        EventManager.OnStageLeaved -= StageLeaved;
        EventManager.OnCardRotated -= CardRotated;
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _gridLayoutResizer = _cardsContainer.GetComponent<GridLayoutResizer>();

        _boardSettings = ResourcesManager.Instance.GetScriptableObject<SO_BoardSettings>(ScriptableObjectKeys.BOARD_SETTINGS_KEY);
        _cardSetting = ResourcesManager.Instance.GetScriptableObject<SO_CardSettings>(ScriptableObjectKeys.CARD_SETTINGS_KEY);

        _availableCards = ResourcesManager.Instance.GetScriptableObject<SO_CardConfiguration>(ScriptableObjectKeys.CARD_CONFIGURATION_KEY).CardsList.Where(Card => Card.IsActive).ToList();
    }

    private void CreatePoolParent()
    {
        _pooledCardsParent = new GameObject(_boardSettings.ParentName).transform;
        _pooledCardsParent.position = _boardSettings.ParentPosition;
    }
    private void CreateCardsPool()
    {
        _cardsPool = new GameObjectPool(_boardSettings.CardPrefab, _pooledCardsParent, _boardSettings.PoolInitialSize, _boardSettings.PoolIncrement);
    }

    #endregion

    #region Event callbacks

    private void GameStateLoaded(GameState pData) 
    {
        _currentDifficulty = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(Difficulty => Difficulty.Id == pData.DifficultyId);
             
        foreach(var item in pData.Cards)
        {
            CardController newCard= GenerateCard(_availableCards.FirstOrDefault((card) => card.Id == item.CardId),item.IsCleaned,item.IsSelected,false);
         
            if(item.IsSelected)
                _currentRotatedCard= newCard;
        }

        _gridLayoutResizer.SetData(pData.BoardRows, pData.BoardColumns);
        _gridLayoutResizer.SetState(true);
    }

    private async void StageStarted(EventManager.StageData pData)
    {
        _currentDifficulty = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(Difficulty => Difficulty.Id == pData.DifficultyId);


        int totalCardsAmount = Mathf.RoundToInt(pData.RowsAmount * pData.ColumnsAmount);
        List<SO_Card> cardsToUse = GetCardToUse(totalCardsAmount);

        GenerateCards(cardsToUse);

        _gridLayoutResizer.SetData(pData.RowsAmount, pData.ColumnsAmount);
        _gridLayoutResizer.SetState(true);

        await Task.Delay((int)(_currentDifficulty.InitialCardsShowTime * 1000));

        //This is to avoid reproduce the rotation sound multiple times at the same time
        EventManager.GenerateSound(_cardSetting.SoundOnRotate);
        foreach (var item in _currentcards)
            item.RotateCard(false, false);
    }

    private void StageLeaved()
    {
        _gridLayoutResizer.SetState(false);
        _currentRotatedCard = null;
        CleanBoardCards();
    }

    private async void CardRotated(CardController pCard)
    {
        if (_currentRotatedCard == null)
        {
            _currentRotatedCard = pCard;
            _currentRotatedCard.IsSelected = true;
            EventManager.MatchStarted(_currentcards);
            EventManager.SaveData();
        }
        else
        {
            if (_currentRotatedCard.CardData.Id == pCard.CardData.Id)
            {

                _currentRotatedCard.CardMatched();
                pCard.CardMatched();

                _currentRotatedCard = null;
                EventManager.MatchSucessed(_currentDifficulty, _currentcards);

                await Task.Delay((int)(_cardSetting.RotationDuration * 1000));
                EventManager.GenerateSound(_boardSettings.SoundOnMatchSuccess);

                bool boardComplete = CheckBoardComplete();

                if (boardComplete) 
                {
                    EventManager.GenerateSound(_boardSettings.SoundOnStageComplete);
                    Debug.Log("- BOARD CLEANED -");

                    EventManager.StageFinished();
                }
                else 
                {
                    EventManager.SaveData();
                }


            }
            else
            {

                _currentRotatedCard.CardNotMatched();
                pCard.CardNotMatched();

                _currentRotatedCard = null;
                EventManager.MatchFailed(_currentcards);

                EventManager.SaveData();

                await Task.Delay((int)(_cardSetting.RotationDuration * 1000));
                EventManager.GenerateSound(_boardSettings.SoundOnMatchFailed);
            }
        }
    }

    #endregion

    #region Board Management

    private List<SO_Card> GetCardToUse(int pCardsAmount)
    {
        List<SO_Card> selectedCards = _availableCards.OrderBy(Card => Random.value).Take(pCardsAmount / 2).ToList();

        List<SO_Card> cardsToUse = new List<SO_Card>();
        foreach (var card in selectedCards)
        {
            cardsToUse.Add(card);
            cardsToUse.Add(card);
        }

        cardsToUse = cardsToUse.OrderBy(Card => Random.value).ToList();
        return cardsToUse;

    }

    private void GenerateCards(List<SO_Card> pCards)
    {
        CleanBoardCards();

        foreach (SO_Card card in pCards)
            GenerateCard(card);

        EventManager.BoardGenerated(_currentcards);
        Task.Delay((int)(_boardSettings.SaveGameDelayAfterNewBoardGeneration * 1000));
        EventManager.SaveData();
    }

    private CardController GenerateCard(SO_Card pCard, bool pIsCleaned=false, bool pIsSelected=false, bool pIsNewGame=true)
    {
        CardController newCard = _cardsPool.GetInstance().GetComponent<CardController>();
        newCard.transform.SetParent(_cardsContainer);
        newCard.Initialize(pCard, _cardSetting,pIsCleaned,pIsSelected,pIsNewGame);
        _currentcards.Add(newCard);
        return newCard;
    }

    private void CleanBoardCards()
    {
        foreach (var item in _currentcards)
            item.CleanCard();

        _currentcards.Clear();
    }

    private bool CheckBoardComplete()
    {
        foreach (var item in _currentcards)
            if (item.IsCleaned is false)
                return false;

        return true;
    }

    #endregion

}
