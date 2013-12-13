using System;

public class Order : EventArgs
{
	public ItemStack stack;
	public int value;
}

public class OrderListener
{
	private AuctionHouseState state;
	
	public Order order;
	
	public OrderListener(AuctionHouseState state)
	{
		this.state = state;
	}
	
	public void Subscribe(Table table)
	{
		table.orderPlaced += new Table.OrderHandler (ReceivedOrder);
	}
	
	private void ReceivedOrder(Order order)
	{
		this.order = order;
		state.orderPlaced = true;
	}	
}
