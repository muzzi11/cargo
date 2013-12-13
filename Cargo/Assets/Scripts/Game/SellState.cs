using UnityEngine;

public class SellState : OrderState
{
	public SellState(State returnToState, Balance balance) : base(returnToState, balance)
	{
		this.balance = balance;
		orderCaption = "Selling ";
		confirmOrderCaption = "Sell";
		sumCaption = "<size=24>Profits: ${0}</size>";
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