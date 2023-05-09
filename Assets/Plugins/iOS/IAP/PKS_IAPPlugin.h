//
//  PKSIAPPlugin.h
//  IAPPlugin
//
//  Created by preetminhas on 19/08/13.
//  Copyright (c) 2013 preetminhas. All rights reserved.
//

#import "PKS_IAPHelper.h"
#import <Foundation/Foundation.h>

typedef struct {
    char* localizedTitle;
    char* localizedDescription;
    char* priceSymbol;
    char* localPrice;
    char* identifier;
} StoreKitProduct;

extern void UnitySendMessage(const char* gameObjectName, const char* methodName, const char* argument);

@interface PKS_IAPPlugin : NSObject <PKSIAPHelperDelegate>

-(void) setGameObjectName:(NSString*)name;

-(id) initWithProductIdentifiers:(NSSet*)identifier;

-(void)requestProducts;
//buy a product
//@return true in case product is valid. false in case product identifier is invalid
- (bool)buyProductWithIdentifier:(NSString *)productIdentifier quantity:(NSInteger)quantity;
//restore product
- (void) restoreCompletedTransactions;

//get details for a product
/*
 *@returns StoreKitProduct
 */
-(StoreKitProduct) detailsForProductWithIdentifier:(NSString*)identifier;
@end
