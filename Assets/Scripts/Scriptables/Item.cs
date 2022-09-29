using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Proplexity
{
    public class Item : ScriptableObject
    {
        [Header("Information")]
        public Sprite itemIcon;
        public string itemName;
    }
}
