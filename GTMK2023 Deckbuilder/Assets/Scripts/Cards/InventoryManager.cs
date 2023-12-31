using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _allCards;
    private Dictionary<int, int> _currentCards = new Dictionary<int, int>();
    [SerializeField] private Deck _deck;

    // Start is called before the first frame update
    void Start()
    {
		// add 2 of every card to the deck
		for (var i = 0; i < _allCards.Count; i++)
		{
			_currentCards.Add(i, 2);
		}

		// add some of the weaker cards

		var cardObjs = new List<Card>(); // to store all cards

        foreach (KeyValuePair<int, int> entry in _currentCards)
        {
            var total = entry.Value;
            for (var i = 0; i < total; i++)
			{
                var c = Instantiate(_allCards[entry.Key]);
				cardObjs.Add(c.GetComponent<Card>()); // add card to list
			}
		}

        cardObjs = Randomiser.RandomiseCards(cardObjs);

        // once all cards are retrieved, initialise deck with given cards
        _deck.InitialiseCards(cardObjs);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddNewCards()
	{
        // something
	}
}
