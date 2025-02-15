using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    #region Fields

    [Header("References")]
    [SerializeField] private Transform _cardsContainer;
    private GridLayoutController _gridLayoutController;
    [Space]
    private List<CardController> _currentCards = new List<CardController>();
    private CardController _currentRotatedCard;

    [Header("Cards pool")]
    private Transform _pooledCardsParent;
    private GameObjectPool _cardsPool;

    [Header("Data")]
    private SO_BoardSettings _boardSettings;
    private SO_CardSettings _cardSetting;
    private SO_Difficulty _currentDifficulty;
    [Space]
    private List<SO_Card> _availableCards;

    #endregion

    #region Monobehaviour

    private void Awake()
    {
        GetReferences();

        CreatePoolParent();
        CreateCardsPool();
    }

    private void OnEnable()
    {
        EventManager.SubscribeEvent<GameLoad>(OnGameLoad);

        EventManager.SubscribeEvent<StageStart>(OnStageStart);
        EventManager.SubscribeEvent<LeaveStage>(OnLeaveStage);

        EventManager.SubscribeEvent<CardRotated>(OnCardRotated);
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent<GameLoad>(OnGameLoad);

        EventManager.UnsubscribeEvent<StageStart>(OnStageStart);
        EventManager.UnsubscribeEvent<LeaveStage>(OnLeaveStage);

        EventManager.UnsubscribeEvent<CardRotated>(OnCardRotated);
    }

    #endregion

    #region Initialization

    private void GetReferences()
    {
        _gridLayoutController = _cardsContainer.GetComponent<GridLayoutController>();

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

    private void OnGameLoad(GameLoad pGameLoad)
    {
        _currentDifficulty = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(Difficulty => Difficulty.Id == pGameLoad.State.DifficultyId);

        GeneratedLoadedBoard(pGameLoad.State);

        _gridLayoutController.SetData(pGameLoad.State.BoardRows, pGameLoad.State.BoardColumns);
        _gridLayoutController.SetState(true);
    }

    private async void OnStageStart(StageStart pStageStart)
    {
        _currentDifficulty = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(Difficulty => Difficulty.Id == PersistentDataManager.DifficultyId);

        int totalCardsAmount = Mathf.RoundToInt(PersistentDataManager.RowsAmount * PersistentDataManager.ColumnsAmount);
        List<SO_Card> cardsToUse = GetCardsToUse(totalCardsAmount);

        GenerateNewBoard(cardsToUse);

        _gridLayoutController.SetData(PersistentDataManager.RowsAmount, PersistentDataManager.ColumnsAmount);
        _gridLayoutController.SetState(true);

        await Task.Delay((int)(_currentDifficulty.InitialCardsShowTime * 1000));

        //This is to avoid reproduce the rotation sound multiple times at the same time
        EventManager.RaiseEvent(new Generate2DSound() { Sound = _cardSetting.SoundOnRotate });
        foreach (var item in _currentCards)
            item.RotateCard(false, false);
    }

    private void OnLeaveStage(LeaveStage pLeaveStage)
    {
        _gridLayoutController.SetState(false);
        _currentRotatedCard = null;
        CleanBoardCards();
    }

    private void OnCardRotated(CardRotated pCardRotated)
    {
        if (_currentRotatedCard == null)
        {
            _currentRotatedCard = pCardRotated.CardController;
            _currentRotatedCard.IsSelected = true;
            EventManager.RaiseEvent(new MatchStart() { Board = _currentCards });
            EventManager.RaiseEvent(new SaveGame());
        }
        else
        {
            if (_currentRotatedCard.CardData.Id == pCardRotated.CardController.CardData.Id)
                CardsMatchSuccess(pCardRotated.CardController);
            else
                CardsMatchFailed(pCardRotated.CardController);
        }
    }

    #endregion

    #region Board Management

    private List<SO_Card> GetCardsToUse(int pCardsAmount)
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

    private async void GenerateNewBoard(List<SO_Card> pCards)
    {
        CleanBoardCards();

        foreach (SO_Card card in pCards)
            GenerateCard(card);

        EventManager.RaiseEvent(new BoardGeneration() { Board = _currentCards });
        await Task.Delay((int)(_boardSettings.SaveGameDelayAfterNewBoardGeneration * 1000));
        EventManager.RaiseEvent(new SaveGame());
    }
    private void GeneratedLoadedBoard(GameState pGameState) 
    {
        CleanBoardCards();

        foreach (var item in pGameState.Cards)
        {
            CardController newCard = GenerateCard(_availableCards.FirstOrDefault((card) => card.Id == item.CardId), item.IsCleaned, item.IsSelected, false);

            if (item.IsSelected)
                _currentRotatedCard = newCard;
        }
    }   
    private CardController GenerateCard(SO_Card pCard, bool pIsCleaned = false, bool pIsSelected = false, bool pIsNewGame = true)
    {
        CardController newCard = _cardsPool.GetInstance().GetComponent<CardController>();
        newCard.transform.SetParent(_cardsContainer);
        newCard.Initialize(pCard, _cardSetting, pIsCleaned, pIsSelected, pIsNewGame);
        _currentCards.Add(newCard);
        return newCard;
    }

    private void CleanBoardCards()
    {
        foreach (var item in _currentCards)
            item.CleanCard();

        _currentCards.Clear();
    }

    private bool CheckBoardComplete()
    {
        foreach (var item in _currentCards)
            if (item.IsCleaned is false)
                return false;

        return true;
    }

    private async void CardsMatchSuccess(CardController pSecondCardRotated)
    {
        _currentRotatedCard.CardMatched();
        pSecondCardRotated.CardMatched();

        _currentRotatedCard = null;
        EventManager.RaiseEvent(new MatchSuccess() { Difficulty = _currentDifficulty, Board = _currentCards });

        await Task.Delay((int)(_cardSetting.RotationDuration * 1000));
        EventManager.RaiseEvent(new Generate2DSound() { Sound = _boardSettings.SoundOnMatchSuccess });

        bool boardComplete = CheckBoardComplete();

        if (boardComplete)
        {
            EventManager.RaiseEvent(new Generate2DSound() { Sound = _boardSettings.SoundOnStageComplete });
            EventManager.RaiseEvent(new StageFinish());
        }
        else
        {
            EventManager.RaiseEvent(new SaveGame());
        }
    }
    private async void CardsMatchFailed(CardController pSecondCardRotated)
    {
        _currentRotatedCard.CardNotMatched();
        pSecondCardRotated.CardNotMatched();

        _currentRotatedCard = null;

        EventManager.RaiseEvent(new MatchFail() { Board = _currentCards });
        EventManager.RaiseEvent(new SaveGame());

        await Task.Delay((int)(_cardSetting.RotationDuration * 1000));
        EventManager.RaiseEvent(new Generate2DSound() { Sound = _boardSettings.SoundOnMatchFailed });
    }

    #endregion
}
