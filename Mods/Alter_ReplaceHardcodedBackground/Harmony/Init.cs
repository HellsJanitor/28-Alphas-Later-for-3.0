using HarmonyLib;
using System;
using System.IO;
using System.Reflection;
using UnityEngine;
namespace Harmony
{
    public class MenuVideoBackground : IModApi
    {
        public static Texture2D background;
        public void InitMod(Mod _modInstance)
        {
            Log.Out(" Loading Patch: " + GetType());

            var harmony = new HarmonyLib.Harmony(GetType().ToString());
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            string str = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "/Resources/Backgrounds.unity3d";
            AssetBundle bundle = AssetBundle.LoadFromFile(str);
            background = bundle.LoadAsset<Texture2D>("background.png");
        }
    }
    [HarmonyPatch(typeof(MainMenuMono))]
    [HarmonyPatch("Update")]
    public class Update
    {
        public static bool replaced;
        public static void Postfix(MainMenuMono __instance)
        {
            if (!replaced)
            {
                var t = __instance.transform.Find("AnchorCenterCenter/HardcodedLoadingImagery/wdwMainMenuBackground/Texture");
                if (t)
                {
                    var uITexture = t.GetComponent<UITexture>();
                    if (uITexture)
                    {
                        uITexture.mainTexture = MenuVideoBackground.background;
                        replaced = true;
                    }
                }
            }
        }
    }
}