public class Balance
{
	private int balance;

	public Balance()
	{
		balance = 1000;
		if(SaveGame.gameData != null)
		{
			balance = SaveGame.gameData.balance;
		}
	}

	public int GetBalance()
	{
		return balance;
	}

	public void Deposit(int cash)
	{
		balance += cash;
		SaveGame.SaveBalance(balance);
	}

	public bool Withdraw(int cash)
	{
		if (balance < cash) return false;

		balance -= cash;
		SaveGame.SaveBalance(balance);

		return true;
	}
}