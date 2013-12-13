using UnityEngine;

public class BuyState : OrderState
{
	private string insufficientFundsCaption = "Insufficient funds!";
	private string dollazShortCaption = "<size=24>You are {0} dollaz short.</size>";
	private bool sufficientFunds = true;
	private int orderValue;

	public BuyState(State returnToState, Balance balance) : base(returnToState, balance)
	{
		orderCaption = "Buying {0}";
		placeOrder = "Buy";
		sumCaption = "<size=24>Total cost: ${0}</size>";
		confirmOrderCaption = "<size=24>Are you sure you want to buy {0} {1}?</size>";
	}

	public override State UpdateState()
	{	
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}
		
		if(!sufficientFunds) GUI.ModalWindow(1, new Rect(0, height/4, width, height/2), FailWindow, insufficientFundsCaption);
		if(orderPlaced) GUI.ModalWindow(2, new Rect(0, height/4, width, height/2), ConfirmationWindow, confirmationCaption);
		
		if(order != null) GUI.Window(3, new Rect(0, 0, width, height), TransactionWindow, string.Format(orderCaption, order.stack.item.name));
		
		return this;
	}

	protected override int UpdateBalance(int orderValue)
	{
		return balance.GetBalance() - orderValue;
	}

	protected override void ProcessTransaction(int orderValue)
	{
		this.orderValue = orderValue;
		sufficientFunds = returnToPrevState = balance.withdraw(orderValue);
	}

	private void FailWindow(int ID)
	{
		int toShort = orderValue - balance.GetBalance();
		
		GUILayout.BeginVertical();
		{
			GUILayout.Space(40);
			GUILayout.Label(string.Format(dollazShortCaption, toShort), normalLabel);
			GUILayout.FlexibleSpace();

			if(GUILayout.Button("Back")) sufficientFunds = true;
		}
		GUILayout.EndVertical();
	}
}