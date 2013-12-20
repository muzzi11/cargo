public static class StringTable
{
	public static readonly string
		//General
		backCaption = "Back",
		okCaption = "OK",

		//Styles
		boxStyle = "box",
		normalLabelStyle = "normalLabel",
		tableItemStyle = "tableItem",
		leftAlignedLabelStyle = "leftAlignedLabel",

		//Auction house & inventory
		buyCaption = "Buy",
		sellCaption = "Sell",
		auctionHouseCaption = "Auction house",
		inventoryCaption = "Inventory",
		balanceCaption = "Balance: {0}",
		cargoCaption = "Cargo space: {0}",
		quantityCaption = "Quantity: x{0}",
		totalVolumeCaption = "Total volume: {0}",
		remainingSpaceCaption = "Cargo space: {0}",
		nameHeaderCaption = "Name:",
		quantityHeaderCaption = "Qnty:",
		purchasePriceHeaderCaption = "Purchase price:",
		priceHeaderCaption = "Price",
		originHeaderCaption = "Origin:",
		volumeHeaderCaption = "Volume:",
		weightHeaderCaption = "Weight:",
		confirmOrderCaption = "Are you sure you want to {0} {1} {2}?",
		dollazShortCaption = "You are {0} dollaz short.",
		cargoSpaceShortCaption = "You need additional {0} cubic meters of free cargo space.",
		summationCaption = "Transaction completed: {0} {1} have been {2} your cargo hold.",

		//Battles
		attackCaption = "Attack",
		fleeCaption = "Flee",
		absorbCaption = "Absorb",
		hullCaption = "HULL {0}/{1}",
		shieldCaption = "SHIELD {0}/{1}",
		statusText = "The enemy is charging weapons.",
		hullLabelStyle = "hullLabel",
		shieldLabelStyle = "shieldLabel",

		//Game over
		windowCaption = "GameOver",
		windowText = "Your ship was destroyed, you failed...";
	
	public static readonly string[] 
		//Auction house
		orderWindowTitles = new string[] 
		{
			"Buying {0}",
			"Selling {0}" 
		},
		orderCaptions = new string[]
		{
			"Buy",
			"Sell"
		},		
		priceCaptions = new string[]
		{ 
			"Total cost: ${0}",
			"Profits: ${0}"
		},
		transactionCaptions = new string[]
		{ 
			"added to",
			"removed from"
	 	},
		orderDialogTitles = 
		{ 
			"Insufficient funds!",
			"Insufficient cargo space!",
			"Please confirm your transaction.",
			"Congratulations!"
		},

		//Battles
		battleWindowTitles = new string[]
		{
			"A wild raider apears!", 
			"Fleeing failed"
		},
		battleWindowTexts = new string[]
		{
			"Destroy the enemy before it destroys you or make an attempt to flee.",
			"Your attempt to flee has failed."
		},

		planetNames = new string[]
		{
			"Boclite",
			"Covis",
			"Eskosie",
			"Edreshan",
			"Otrade",
			"Gahiri",
			"Peynides",
			"Bolara",
			"Fophus",
			"Asnion",
			"Eflora",
			"Ustarvis",
			"Soyanus"
		};

}