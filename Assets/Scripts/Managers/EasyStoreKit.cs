using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public struct StoreKitProduct {
    public string localizedTitle;
    public string localizedDescription;
    public string priceSymbol;
    public string localPrice;
	public string identifier;
	
	public override string ToString() {
		return string.Format("{0}, {1}, {2} {3}, {4}",
			localizedTitle, localizedDescription, priceSymbol, localPrice, identifier);
	}
};

public delegate void ProductsLoadedEventHandler(StoreKitProduct[] products);
public delegate void TransactionPurchasedEventHandler(string productIdentifier);
public delegate void TransactionFailedEventHandler(string productIdentifier,string errorMessage);
public delegate void TransactionRestoredEventHandler(string productIdentifier);
public delegate void TransactionCancelledEventHandler(string productIdentifier);
public delegate void RestoreFailedEventHandler(string errorMessage);
public delegate void RestoreCompletedEventHandler();

public class EasyStoreKit : MonoBehaviour {
	[DllImport ("__Internal")]
	private static extern bool canMakePayments();
	
	[DllImport ("__Internal")]
	private static extern void assignIdentifiersAndCallbackGameObject(string[] identifiers,int identifiersCount, string gameObjectName);
	
	[DllImport ("__Internal")]
	private static extern void loadProducts();	
	
	[DllImport ("__Internal")]
	private static extern bool buyProductWithIdentifier(string identifier, int quantity);
	
	[DllImport ("__Internal")]
	private static extern void restoreProducts();	

	[DllImport ("__Internal")]
	private static extern StoreKitProduct detailsForProductWithIdentifier(string identifier);
	
	
	private static string gameObjectName;
	
	//the events
	public static event ProductsLoadedEventHandler productsLoadedEvent;
	public static event TransactionPurchasedEventHandler transactionPurchasedEvent;
	public static event TransactionFailedEventHandler transactionFailedEvent;
	public static event TransactionCancelledEventHandler transactionCancelledEvent;
	public static event TransactionRestoredEventHandler transactionRestoredEvent;
	public static event RestoreCompletedEventHandler restoreCompletedEvent;
	public static event RestoreFailedEventHandler restoreFailedEvent;
	
	public void Awake() {
		//assign the game object name so that this object can be used correctly
		EasyStoreKit.gameObjectName = gameObject.name;
	}
	
	private static void PrintPlatformNotSupportedMessage() {
		Debug.Log("This plugin works on iOS only.");
	}
	
	public static bool CanMakePayments() {
		bool result = false;
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			result = canMakePayments();
		} else {
			PrintPlatformNotSupportedMessage();
			result = true;
		}
		
		return result;
	}
	
	public static void AssignIdentifiers(string[] identifiers) {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			assignIdentifiersAndCallbackGameObject(identifiers, identifiers.Length, gameObjectName);
		} else {
			PrintPlatformNotSupportedMessage();
		}
	}
	
	public static void LoadProducts() {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			loadProducts();
		} else {
			PrintPlatformNotSupportedMessage();
			//raise event with a dummy product
			StoreKitProduct[] products = new StoreKitProduct[1];
			products[0].localizedTitle = "Dummy product";
			products[0].localizedDescription = "Dummy product description in non-iOS environment";
			products[0].localPrice = "0.00";
			products[0].priceSymbol = "$";
			products[0].identifier = "dummy";
			
			if(productsLoadedEvent != null) {
				productsLoadedEvent(products);
			}
		}
	}
	
	/// <summary>
	/// Buys the product with identifier.
	/// </summary>
	/// <returns>
	/// true if the product is valid and can be bought. False otherwise.
	/// </returns>
	/// <param name='identifier'>
	/// If set to <c>true</c> identifier.
	/// </param>
	/// <param name='quantity'>
	/// If set to <c>true</c> quantity.
	/// </param>
	public static bool BuyProductWithIdentifier(string identifier, int quantity) {
		bool result = false;
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			result = buyProductWithIdentifier(identifier,quantity);
		} else {
			PrintPlatformNotSupportedMessage();
			//call the event
			if(transactionPurchasedEvent != null) {
				transactionPurchasedEvent(identifier);
			}
			
			result = true;
		}
		
		return result;
	}
	
	
	public static void RestoreProducts() {
		if(Application.platform == RuntimePlatform.IPhonePlayer) {
			restoreProducts();
		} else {
			PrintPlatformNotSupportedMessage();
			
			//call the events for restore and complete
			if(transactionRestoredEvent != null) {
				transactionRestoredEvent("dummy");
			}
			if(restoreCompletedEvent != null) {
				restoreCompletedEvent();
			}
			
			
		}
	}
	
/*--- Callbacks from the plugin ---*/
	/// <summary>
	/// Products loaded.
	/// </summary>
	/// <param name='productIdentifiers'>
	/// Product identifiers. Semicolon separated list
	/// </param>
	public void productsLoaded(string productIdentifierString) {
		Debug.Log("Products loaded: "+productIdentifierString);
		
		string[] delimiters = new string[1];
		delimiters[0] = ";";
		string[] identifiers = productIdentifierString.Split(delimiters,System.StringSplitOptions.RemoveEmptyEntries);
		
		StoreKitProduct[] productArray = new StoreKitProduct[identifiers.Length];
		
		int index = 0;
		foreach ( string identifier in identifiers) {
			productArray[index] = detailsForProductWithIdentifier(identifier);
			index++;
		}
		
		//raise an event
		if(productsLoadedEvent != null) {
			productsLoadedEvent(productArray);
		}
	}
	
	public void transactionPurchased(string productIdentifier) {
		Debug.Log("transactionPurchased: "+productIdentifier);
		
		if(transactionPurchasedEvent != null) {
			transactionPurchasedEvent(productIdentifier);
		}
	}
	
	public void transactionFailed(string identifierAndError) {
		Debug.Log("transactionFailed: "+identifierAndError);
		
		string[] delimiters = new string[1];
		delimiters[0] = ";";
		string[] input = identifierAndError.Split(delimiters,System.StringSplitOptions.None);
		string identifier = input[0];
		string error = "";
		if(input.Length == 2) {
			error = input[1];
		}
		
		if(transactionFailedEvent != null) {
			transactionFailedEvent(identifier,error);
		}
	}
	
	public void transactionRestored(string productIdentifier) {
		Debug.Log("transactionRestored: "+productIdentifier);
		
		if(transactionRestoredEvent != null) {
			transactionRestoredEvent(productIdentifier);
		}
	}
	
	public void transactionCancelled(string productIdentifier) {
		Debug.Log("transactionCancelled: "+productIdentifier);
		
		if(transactionCancelledEvent != null) {
			transactionCancelledEvent(productIdentifier);
		}
	}
	
	public void restoreProcessFailed(string errorMessage) {
		Debug.Log ("restoreProcessFailed");
		if(restoreFailedEvent != null) {
			restoreFailedEvent(errorMessage);
		}
	}
	
	public void restoreProcessCompleted(string notUsed) {
		Debug.Log ("restoreProcessCompleted");
		if(restoreCompletedEvent != null) {
			restoreCompletedEvent();
		}
	}
}
