//
//  IAPHelper.h
//  IAPPlugin
//
//  Created by preetminhas on 19/08/2013.
//  Copyright 2013 Preet Kamal Minhas. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "StoreKit/StoreKit.h"

@protocol PKSIAPHelperDelegate <NSObject>
//contains array of SKProduct* and string
-(void) productsLoaded:(NSArray*)products invalidIdentifiers:(NSArray*)invalidIdentifiers;
-(void) transactionPurchased:(SKPaymentTransaction*)transaction;
-(void) transactionFailed:(SKPaymentTransaction*)transaction;
-(void) transactionCancelled:(SKPaymentTransaction*)transaction;
-(void) transactionRestored:(SKPaymentTransaction*)transaction;
-(void) restoreProcessFailed:(NSError*)error;
-(void) restoreProcessCompleted;
@end

@interface PKS_IAPHelper : NSObject <SKProductsRequestDelegate, SKPaymentTransactionObserver>

@property (nonatomic, retain) NSSet *productIdentifiers;
@property (nonatomic, retain) NSArray * products;
@property (nonatomic, retain) SKProductsRequest *request;
@property (nonatomic,assign) id<PKSIAPHelperDelegate> delegate;

//must check if user can make payments
+(BOOL) canMakePayments;

//designated initializer
- (id)initWithProductIdentifiers:(NSSet *)productIdentifiers;
//request the products from the store
- (void)requestProducts;
//buy a product
- (void)buyProduct:(SKProduct *)product quantity:(NSInteger)quantity;
//restore product
- (void) restoreCompletedTransactions;

@end
