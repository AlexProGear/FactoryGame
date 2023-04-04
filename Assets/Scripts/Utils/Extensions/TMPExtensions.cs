﻿using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Utils.Extensions
{
    public static class TMPExtensions
    {
        public static void ForceRebuild(this TMP_Text text, RectTransform parent = null)
        {
            if (parent == null)
                parent = text.rectTransform.parent as RectTransform;
            text.Rebuild(CanvasUpdate.Layout);
            LayoutRebuilder.ForceRebuildLayoutImmediate(parent);
        }
    }
}