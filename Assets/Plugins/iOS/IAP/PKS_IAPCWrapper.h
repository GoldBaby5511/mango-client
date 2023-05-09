//
//  PKSIAPCWrapper.h
//  IAPPlugin
//
//  Created by preetminhas on 19/08/13.
//  Copyright (c) 2013 preetminhas. All rights reserved.
//

#ifndef __IAPPlugin__PKSIAPCWrapper__
#define __IAPPlugin__PKSIAPCWrapper__

#include <iostream>
#include "PKS_IAPHelper.h"
#import "PKS_Utility.h"
#include "PKS_IAPPlugin.h"

extern "C" {
    
    bool canMakePayments();
    //params: the identifiers and the gameobject name which will receive the callbacks
    void assignIdentifiersAndCallbackGameObject(char** identifiers,int identifiersCount, char *gameObjectName);
    void loadProducts();
    //returns true in case the identifier is a valid product
    bool buyProductWithIdentifier(char* identifier, int quantity);
    void restoreProducts();
    StoreKitProduct detailsForProductWithIdentifier(char *identifier);
}

#endif /* defined(__IAPPlugin__PKSIAPCWrapper__) */
