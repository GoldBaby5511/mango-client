//
//  IAPHelper.m
//  IAPPlugin
//
//  Created by preetminhas on 19/08/2013.
//  Copyright 2013 Preet Kamal Minhas. All rights reserved.
//

#import "PKS_IAPHelper.h"

@implementation PKS_IAPHelper

- (id)initWithProductIdentifiers:(NSSet *)productIdentifiers {
    if ((self = [super init])) {
        // Store product identifiers
        self.productIdentifiers = productIdentifiers;
        [[SKPaymentQueue defaultQueue] addTransactionObserver:self];
    }
    return self;
}

-(void)dealloc {
    [[SKPaymentQueue defaultQueue] removeTransactionObserver:self];
    self.request = nil;
    self.products = nil;
    self.productIdentifiers = nil;
    self.delegate = nil;
}

+(BOOL) canMakePayments {
    return [SKPaymentQueue canMakePayments];
}

- (void)requestProducts {
    SKProductsRequest *request = [[SKProductsRequest alloc] initWithProductIdentifiers:_productIdentifiers];
    self.request = request;
    self.request.delegate = self;
    [self.request start];
    
}


- (void)buyProduct:(SKProduct *)product quantity:(NSInteger)quantity {
    
    NSLog(@"%@ Buying %@",NSStringFromSelector(_cmd), product.productIdentifier);
    
    SKMutablePayment *payment = [SKMutablePayment paymentWithProduct:product];
    payment.quantity = quantity;
    [[SKPaymentQueue defaultQueue] addPayment:payment];
    
}

- (void) restoreCompletedTransactions {
    NSLog(@"%@",NSStringFromSelector(_cmd));
    [[SKPaymentQueue defaultQueue] restoreCompletedTransactions];
}

#pragma mark - SKProductsRequestDelegate

- (void)productsRequest:(SKProductsRequest *)request didReceiveResponse:(SKProductsResponse *)response {
    NSLog(@"%@",NSStringFromSelector(_cmd));
    self.request = nil;
    [self.delegate productsLoaded:response.products invalidIdentifiers:response.invalidProductIdentifiers];
}

#pragma mark - Private methods
/*
 This method can be overridden to perform server side validations
 Refer: https://developer.apple.com/library/ios/documentation/NetworkingInternet/Conceptual/StoreKitGuide/VerifyingStoreReceipts/VerifyingStoreReceipts.html
 @returns: YES if verification passed. NO otherwise.
 */
-(BOOL) verifyTransaction:(SKPaymentTransaction*)transaction {
    return YES;
}

-(void) transaction:(SKPaymentTransaction*)transaction purchasedOnQueue:(SKPaymentQueue*) queue {
    
    //TODO: perform any server side verification here
    if ([self verifyTransaction:transaction]) {
        //notify delegate
        [self.delegate transactionPurchased:transaction];
        
        //finish transaction
        [queue finishTransaction:transaction];
        
    } else {
        NSLog(@"Transaction verification failed");
        [self.delegate transactionCancelled:transaction];
        //finish transaction
        [queue finishTransaction:transaction];
    }
}

-(void) transaction:(SKPaymentTransaction*)transaction failedOnQueue:(SKPaymentQueue*) queue {
    
    if (transaction.error.code == SKErrorPaymentCancelled ||
        transaction.error.code == SKErrorPaymentNotAllowed) {
        //notify this txn as cancelled
        [self.delegate transactionCancelled:transaction];
    } else {
        //notify delegate
        [self.delegate transactionFailed:transaction];
    }
    //finish txn
    [queue finishTransaction:transaction];
}

-(void) transaction:(SKPaymentTransaction*)transaction restoredOnQueue:(SKPaymentQueue*) queue {
    //notify delegate
    [self.delegate transactionRestored:transaction];
    //finish txn
    [queue finishTransaction:transaction];
}

#pragma mark - SKPaymentTransactionObserver
- (void)paymentQueue:(SKPaymentQueue *)queue updatedTransactions:(NSArray *)transactions
{
    for (SKPaymentTransaction *transaction in transactions) {
        switch (transaction.transactionState) {
            case SKPaymentTransactionStatePurchased:
                [self transaction:transaction purchasedOnQueue:queue];
                break;
            case SKPaymentTransactionStateFailed:
                [self transaction:transaction failedOnQueue:queue];
                break;
            case SKPaymentTransactionStateRestored:
                [self transaction:transaction restoredOnQueue:queue];
            default:
                break;
        }
    }
}

-(void)paymentQueue:(SKPaymentQueue *)queue restoreCompletedTransactionsFailedWithError:(NSError *)error {
    NSLog(@"Restoration failed with error %@ %d", error.localizedDescription, error.code);
    
    //finish all txn in queue
    for (SKPaymentTransaction *transaction in queue.transactions) {
        NSLog(@"restore failed, finishing txn : %@", transaction.transactionIdentifier);
        [queue finishTransaction:transaction];
    }

    [self.delegate restoreProcessFailed:error];

}

-(void)paymentQueueRestoreCompletedTransactionsFinished:(SKPaymentQueue *)queue {
    NSLog(@"restoration completed");
    [self.delegate restoreProcessCompleted];
}

@end
