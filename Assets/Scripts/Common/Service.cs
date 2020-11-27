using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KC.Common {
    public static class Service {
        public static bool CompareLists<T>(List<T> list1, List<T> list2) {
            if (list1 == null || list2 == null) { return false; }
            if (list1.Count != list2.Count) { return false; }
            int size = list1.Count;
            for (int i = 0; i < size; i++) {
                if (!list1[i].Equals(list2[i])) { return false; }
            }
            return true;
        }
    }
}