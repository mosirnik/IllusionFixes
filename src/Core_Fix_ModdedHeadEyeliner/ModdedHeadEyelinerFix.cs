﻿using Common;
using HarmonyLib;
using System;
using System.Linq;
using UnityEngine;

namespace IllusionFixes
{
    public partial class ModdedHeadEyelinerFix
    {
        public const string PluginName = "Head Fix";

        internal void Awake()
        {
            var harmony = Harmony.CreateAndPatchAll(typeof(ModdedHeadEyelinerFix));
            var getTextureMethod = typeof(ChaControl).GetMethod("GetTexture", AccessTools.all);
            if (getTextureMethod == null) throw new ArgumentException("Could not find ChaControl.GetTexture");
            if (getTextureMethod.GetParameters().Any(x => x.Name == "addStr"))
                harmony.Patch(typeof(ChaControl).GetMethod("GetTexture", AccessTools.all), null, new HarmonyMethod(typeof(ModdedHeadEyelinerFix).GetMethod(nameof(GetTexture), AccessTools.all)));
        }

        /// <summary>
        /// Strip the "addStr" and attempt to get the texture again. This fixes head types missing eyeliners if they don't have one defined for that specific head.
        /// </summary>
        public static void GetTexture(ChaListDefine.CategoryNo type, int id, ChaListDefine.KeyType assetBundleKey, ChaListDefine.KeyType assetKey, string addStr, ChaControl __instance, ref Texture2D __result)
        {
            if (__result == null && !addStr.IsNullOrEmpty())
                __result = __instance.GetTexture(type, id, assetBundleKey, assetKey, "");
        }
    }
}