using UnityEngine;

public class BuyState : OrderState
{
	private string insufficientFundsCaption = "Insufficient funds!";
	private string dollazShortCaption = "<size=24>You are {0} dollaz short.</size>";
	private bool sufficientFunds = true;

	public BuyState(State returnToState, Balance balance) : base(returnToState, balance)
	{
		orderCaption = "Buying ";
		confirmOrderCaption = "Buy";
		sumCaption = "<size=24>Total cost: ${0}</size>";
	}

	public override int UpdateBalance(int orderValue)
	{
		return balance.GetBalance() - orderValue;
	}

	public override void ProcessTransaction(int orderValue)
	{
		sufficientFunds = returnToPrevState = balance.withdraw(orderValue);
	}	

	public override State UpdateState()
	{	
		if (returnToPrevState)
		{
			returnToPrevState = false;
			return returnToState;
		}

		if(!sufficientFunds) GUI.ModalWindow(1, new Rect(0, height/4, width, height/2), FailWindow, insufficientFundsCaption);

		if (order != null)
		{
			GUI.Window(0, new Rect(0, 0, width, height), BuyWindow, orderCaption + order.stack.item.name);
		}
		return this;
	}

	public void FailWindow(int ID)
	{
		GUILayout.FlexibleSpace();
		
		GUILayout.BeginVertical();
		{
			int toShort = orderValue - balance.GetBalance();
			GUILayout.Label(string.Format(dollazShortCaption, toShort), normalLabel);

			GUILayout.FlexibleSpace();

			if(GUILayout.Button("Back")) sufficientFunds = true;
		}
		GUILayout.EndVertical();
	}
}