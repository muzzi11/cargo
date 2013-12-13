using UnityEngine;
using System.Collections.Generic;

public class BuyState : OrderState
{
	public BuyState(State returnToState, Balance balance) : base(returnToState, balance)
	{
		orderCaption = "Buying ";
		confirmOrderCaption = "Buy";
	}

	public override int UpdateBalance(int orderValue)
	{
		return balance.GetBalance() - orderValue;
	}

	public override void ProcessTransaction(int orderValue)
	{
		returnToPrevState = balance.withdraw(orderValue);
	}	
}

public class SellState : OrderState
{
	public SellState(State returnToState, Balance balance) : base(returnToState, balance)
	{
		this.balance = balance;
		orderCaption = "Selling ";
		confirmOrderCaption = "Sell";
	}

	public override int UpdateBalance(int orderValue)
	{
		return balance.GetBalance() + orderValue;
	}

	public override void ProcessTransaction(int orderValue)
	{
		balance.deposit(orderValue);
		returnToPrevState = true;
	}
}