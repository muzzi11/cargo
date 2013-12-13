using System;

public class Order : EventArgs
{
	public ItemStack stack;
	public int value;
}

public interface OrderListener
{
	void ReceivedOrder(Order order);
}