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
    private List<CardController> _currentcards= new List<CardController>();

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
        EventManager.OnMatchStarted += MatchStarted;
    }

    private void OnDisable()
    {
        EventManager.OnMatchStarted -= MatchStarted;
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

    private async void MatchStarted(EventManager.MatchData pData)
    {
        _currentDifficulty = ResourcesManager.Instance.GetScriptableObject<SO_DifficultyConfiguration>(ScriptableObjectKeys.DIFFICULTY_CONFIGURATION_KEY).DifficultyList.FirstOrDefault(Difficulty => Difficulty.Id == pData.DifficultyId);

        int totalCardsAmount = Mathf.RoundToInt(pData.RowsAmount * pData.ColumnsAmount);
        List<SO_Card> cardsToUse = GetCardToUse(totalCardsAmount);

        GenerateCards(cardsToUse);

        _gridLayoutResizer.SetData(pData.RowsAmount, pData.ColumnsAmount);
        _gridLayoutResizer.SetState(true);

        await Task.Delay((int)(_currentDifficulty.InitialCardsShowTime * 1000));

        foreach (var item in _currentcards)
            item.RotateToFaceDown();
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
    }

    private void GenerateCard(SO_Card pCard)
    {
        CardController newCard = _cardsPool.GetInstance().GetComponent<CardController>();
        newCard.transform.SetParent(_cardsContainer);
        newCard.Initialize(pCard,_cardSetting);
        _currentcards.Add(newCard);
    }

    private void CleanBoardCards()
    {
        foreach (var item in _currentcards)
            item.CleanCard();

        _currentcards.Clear();
    }

    #endregion
}
