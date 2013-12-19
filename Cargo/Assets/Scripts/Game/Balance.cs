public class Balance
{
	private int balance;

	public Balance()
	{
		balance = 99999;
	}

	public int GetBalance()
	{
		return balance;
	}

	public void Deposit(int cash)
	{
		balance += cash;
	}

	public bool Withdraw(int cash)
	{
		if (balance < cash) return false;

		balance -= cash;
		return true;
	}
}