//
//  PKSIAPPlugin.m
//  IAPPlugin
//
//  Created by preetminhas on 19/08/13.
//  Copyright (c) 2013 preetminhas. All rights reserved.
//

#import "PKS_IAPPlugin.h"
#import "PKS_Utility.h"

@interface PKS_IAPPlugin() {
    PKS_IAPHelper *_iapHelper;
    NSMutableDictionary *_identifierProductMapping;
}
@end

@implementation PKS_IAPPlugin
-(id)initWithProductIdentifiers:(NSSet *)identifiers {
    self = [super init];
    if (self) {
        _iapHelper = [[PKS_IAPHelper alloc] initWithProductIdentifiers:identifiers];
        _iapHelper.delegate = self;
        
        _identifierProductMapping = [[NSMutableDictionary alloc] init];
    }
    
    return self;
}


#pragma mark - Public methods
-(void)requestProducts {
    [_iapHelper requestProducts];
}

- (bool)buyProductWithIdentifier:(NSString *)productIdentifier quantity:(NSInteger)quantity{
    bool result = false;
    
    SKProduct *product = [_identifierProductMapping valueForKey:productIdentifier];
    
    if (product) {
        result = true;
        [_iapHelper buyProduct:product quantity:quantity];
    } else {
        NSLog(@"Invalid identifier %@",productIdentifier);
    }
    
    return result;
}

//restore product
- (void) restoreCompletedTransactions {
    [_iapHelper restoreCompletedTransactions];
}

-(StoreKitProduct) detailsForProductWithIdentifier:(NSString*)identifier {
    SKProduct *product = [_identifierProductMapping valueForKey:identifier];
    StoreKitProduct result;
    if (product) {
        result.localizedTitle = [PKS_Utility CStringCopy:product.localizedTitle];
        result.localizedDescription = [PKS_Utility CStringCopy:product.localizedDescription];
        NSLocale *priceLocale = product.priceLocale;
        NSNumberFormatter *formatter = [[NSNumberFormatter alloc] init];
        formatter.locale = priceLocale;
        result.priceSymbol = [PKS_Utility CStringCopy:[formatter currencySymbol]];
        result.localPrice = [PKS_Utility CStringCopy:[product.price stringValue]];
        result.identifier = [PKS_Utility CStringCopy:product.productIdentifier];
    }
    
    return result;
}

#pragma mark -
static char* sGameObjectCString;
-(void)setGameObjectName:(NSString *)gameObjectName {
    if (sGameObjectCString != NULL) {
        free(sGameObjectCString);
        sGameObjectCString = NULL;
    }
    if (gameObjectName != nil) {
        sGameObjectCString = [PKS_Utility CStringCopy:gameObjectName];
    }
}

#pragma mark - PKSIAPHelperDelegate
-(void) productsLoaded:(NSArray*)products invalidIdentifiers:(NSArray*)invalidIdentifiers {
    //set up the mapping dictionary
    for (SKProduct *product in products) {
        [_identifierProductMapping setValue:product forKey:product.productIdentifier];
    }
    
    //signal unity.
    //Send a semicolon separated list of valid product identifiers
    NSArray *productIdentifierArray = [_identifierProductMapping allKeys];
    NSMutableString *string = [NSMutableString string];
    
    for (int index = 0; index < productIdentifierArray.count; index++) {
        
        [string appendString:[productIdentifierArray objectAtIndex:index]];
        
        //append ; after every identifier except for the last element
        if (index != productIdentifierArray.count - 1) {
            [string appendString:@";"];
        }
    }
    
    UnitySendMessage(sGameObjectCString, "productsLoaded", [PKS_Utility CStringCopy:string]);
}

-(void) transactionPurchased:(SKPaymentTransaction*)transaction {
    //=======================================================================//
    // 验证凭据，获取到苹果返回的交易凭据,appStoreReceiptURL iOS7.0增加的，购买交易完成后，会将凭据存放在该地址
    NSURL *receiptURL = [[NSBundle mainBundle] appStoreReceiptURL];
    // 从沙盒中获取到购买凭据
    NSData *receiptData = [NSData dataWithContentsOfURL:receiptURL];
    NSString *encodeStr = [receiptData base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed];
    NSString *payload = [NSString stringWithFormat:@"{\"receipt-data\" : \"%@\"}", encodeStr];
    
    NSString *productIdentifier = [NSString stringWithFormat:@"%@%@%@",transaction.payment.productIdentifier,@"$",payload];
    
    //发送消息到unity
    UnitySendMessage(sGameObjectCString, "transactionPurchased", [PKS_Utility CStringCopy:productIdentifier]);
}
//
//#pragma mark 验证购买凭据
//-(void)verifyPruchase
//{
//    // 验证凭据，获取到苹果返回的交易凭据
//    // appStoreReceiptURL iOS7.0增加的，购买交易完成后，会将凭据存放在该地址
//    NSURL *receiptURL = [[NSBundle mainBundle] appStoreReceiptURL];
//    // 从沙盒中获取到购买凭据
//    NSData *receiptData = [NSData dataWithContentsOfURL:receiptURL];
//    
//    // 发送网络POST请求，对购买凭据进行验证
//    NSURL *url = [NSURL URLWithString:@"https://sandbox.itunes.apple.com/verifyReceipt"];
//    // 国内访问苹果服务器比较慢，timeoutInterval需要长一点
//    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:url cachePolicy:NSURLRequestUseProtocolCachePolicy timeoutInterval:10.0f];
//    
//    request.HTTPMethod = @"POST";
//    
//    // 在网络中传输数据，大多情况下是传输的字符串而不是二进制数据
//    // 传输的是BASE64编码的字符串
//    /**
//     BASE64 常用的编码方案，通常用于数据传输，以及加密算法的基础算法，传输过程中能够保证数据传输的稳定性
//     BASE64是可以编码和解码的
//     */
//    NSString *encodeStr = [receiptData base64EncodedStringWithOptions:NSDataBase64EncodingEndLineWithLineFeed];
//    
//    NSString *payload = [NSString stringWithFormat:@"{\"receipt-data\" : \"%@\"}", encodeStr];
//    NSData *payloadData = [payload dataUsingEncoding:NSUTF8StringEncoding];
//    
//    request.HTTPBody = payloadData;
//    
//    // 提交验证请求，并获得官方的验证JSON结果
//    NSData *result = [NSURLConnection sendSynchronousRequest:request returningResponse:nil error:nil];
//    
//    // 官方验证结果为空
//    if (result == nil) {
//        NSLog(@"验证失败");
//    }
//    
//    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:result options:NSJSONReadingAllowFragments error:nil];
//    
//    NSLog(@"%@", dict);
//    
//    if (dict != nil) {
//        // 比对字典中以下信息基本上可以保证数据安全
//        // bundle_id&application_version&product_id&transaction_id
//        NSLog(@"验证成功");
//    }
//}



-(void) transactionFailed:(SKPaymentTransaction*)transaction {
    NSString *errorDescription = transaction.error.localizedDescription;
    NSLog(@"transaction failed %@",errorDescription);
    
    NSString *productIdentifier = transaction.payment.productIdentifier;
    
    NSString *string = [NSString stringWithFormat:@"%@;%@",productIdentifier,errorDescription];
    
    //send the identifier & error semicolon separated
    UnitySendMessage(sGameObjectCString, "transactionFailed", [PKS_Utility CStringCopy:string]);
}

//called when transaction is cancelled or not allowed or server verification failed
-(void)transactionCancelled:(SKPaymentTransaction *)transaction {
    NSString *productIdentifier = transaction.payment.productIdentifier;
    UnitySendMessage(sGameObjectCString, "transactionCancelled", [PKS_Utility CStringCopy:productIdentifier]);
}

-(void) transactionRestored:(SKPaymentTransaction*)transaction {
    NSString *productIdentifier = transaction.originalTransaction.payment.productIdentifier;
    //send the identifier
    UnitySendMessage(sGameObjectCString, "transactionRestored", [PKS_Utility CStringCopy:productIdentifier]);
}

-(void) restoreProcessFailed:(NSError *)error {
    UnitySendMessage(sGameObjectCString, "restoreProcessFailed",[PKS_Utility CStringCopy: error.localizedDescription]);
}

-(void) restoreProcessCompleted {
    UnitySendMessage(sGameObjectCString, "restoreProcessCompleted","");    
}
@end
