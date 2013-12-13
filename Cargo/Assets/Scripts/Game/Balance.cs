public class Balance
{
	private int balance;

	public Balance()
	{
		balance = 100;
	}

	public int GetBalance()
	{
		return balance;
	}

	public void deposit(int cash)
	{
		balance += cash;
	}

	public bool withdraw(int cash)
	{
		if (balance < cash) return false;

		balance -= cash;
		return true;
	}
}