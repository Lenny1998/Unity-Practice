﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShopCustomer {

    void BoughtItem(Item.ItemType itemType);
    bool TrySpendGoldAmount(int goldAmount);

}
