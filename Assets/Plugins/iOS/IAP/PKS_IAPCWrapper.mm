//
//  PKSIAPCWrapper.cpp
//  IAPPlugin
//
//  Created by preetminhas on 19/08/13.
//  Copyright (c) 2013 preetminhas. All rights reserved.
//

#include "PKS_IAPCWrapper.h"

static PKS_IAPPlugin *_plugin;

extern "C" {
    bool canMakePayments() {
        return [PKS_IAPHelper canMakePayments];
    }
    
    void assignIdentifiersAndCallbackGameObject(char** identifiers,int identifiersCount, char *gameObjectName) {
        
        NSMutableSet *identifierSet = [NSMutableSet set];
        for (int index = 0; index < identifiersCount; index++) {
            char* identifier = identifiers[index];
            [identifierSet addObject:[PKS_Utility CreateNSString:identifier]];
        }
        if (_plugin) {
            _plugin = nil;
        }
        _plugin = [[PKS_IAPPlugin alloc] initWithProductIdentifiers:identifierSet];
        _plugin.gameObjectName = [PKS_Utility CreateNSString:gameObjectName];
    }
    
    void loadProducts() {
        [_plugin requestProducts];
    }
    
    bool buyProductWithIdentifier(char* identifier, int quantity) {
        return [_plugin buyProductWithIdentifier:[PKS_Utility CreateNSString:identifier] quantity:quantity];
    }
    
    void restoreProducts() {
        [_plugin restoreCompletedTransactions];
    }
    
    StoreKitProduct detailsForProductWithIdentifier(char *identifier) {
        //retrieve storekit product
        StoreKitProduct result = [_plugin detailsForProductWithIdentifier:[PKS_Utility CreateNSString:identifier]];
        
        return result;
    }
    
}