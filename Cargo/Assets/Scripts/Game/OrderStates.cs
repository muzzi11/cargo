using UnityEngine;
using System.Collections.Generic;

public class BuyState : OrderState
{
	public BuyState(State returnToState) : base(returnToState)
	{
		orderCaption = "Buying ";
	}
}

public class SellState : OrderState
{
	public SellState(State returnToState) : base(returnToState)
	{
		orderCaption = "Selling ";
	}
}