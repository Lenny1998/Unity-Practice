using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IItemHolder {

    void RemoveItem(Item item);
    void AddItem(Item item);
    bool CanAddItem();

}
