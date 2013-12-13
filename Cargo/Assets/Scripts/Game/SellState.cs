using UnityEngine;

public class SellState : OrderState
{
	public SellState(State returnToState, Balance balance, Order order) : base(returnToState, balance, order)
	{
		this.balance = balance;
		orderCaption = "Selling ";
		placeOrder = "Sell";
		sumCaption = "<size=24>Profits: ${0}</size>";
	}

	public override State UpdateState()
	{	
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}
		
		if (orderPlaced) GUI.ModalWindow(1, new Rect(0, height/4, width, height/2), ConfirmationWindow, confirmationCaption);
		
		if(order != null) GUI.Window(0, new Rect(0, 0, width, height), TransactionWindow, string.Format(orderCaption, order.name));
		
		return this;
	}
	
	protected override int UpdateBalance(int orderValue)
	{
		return balance.GetBalance() + orderValue;
	}
	
	protected override void ProcessTransaction(int orderValue)
	{
		balance.deposit(orderValue);
		returnToPrevState = true;
	}
}