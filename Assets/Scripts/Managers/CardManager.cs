using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
	public enum PlayerSelection
	{
		PlayerOne,
		PlayerTwo,
	}

	public enum CardStates
	{
		SelectSubject,
		SelectPredicate,
		SelectCompliment,
		PlayCards,
	}

	CardStates currentState;

    private void Update()
	{
		switch (currentState)
		{
			case CardStates.SelectSubject:
				SelectSubjectCards();
				break;

			case CardStates.SelectPredicate:
				SelectPredicateCards();
				break;

			case CardStates.SelectCompliment:
				SelectComplimentCards();
				break;

			case CardStates.PlayCards:
				PlaySelectedCards();
				break;

		}
	}

	private void SelectSubjectCards()
	{

	}

	private void SelectPredicateCards()
	{

	}

	private void SelectComplimentCards()
	{

	}
	
	private void PlaySelectedCards()
	{
		
	}
}
