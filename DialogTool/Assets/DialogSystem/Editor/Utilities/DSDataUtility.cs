using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KorYmeLibrary.DialogueSystem.Utilities
{
    public static class DSDataUtility
    {
        public static void DeleteGraphElementScriptable(this DSElementData elementData)
            => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(elementData));
    }
}
