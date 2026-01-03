using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace yarsDialogSkip 
{
    [BepInPlugin(pluginGuid, pluginName, pluginVersion)]
    public class YarsDialogSkip : BaseUnityPlugin
    {
        //  tedious mod admin
        public const string pluginGuid = "yarsRising.YarsDialogSkip";
        public const string pluginName = "Yars Dialog Skip";
        public const string pluginVersion = "0.1.0";

        //  create logger
        internal static new ManualLogSource Log;

        public void Awake()
        {
            //  start up message and logger initialisation 
            Logger.LogInfo("Hey Look Mal, you might know how to swim but I made a mod!");
            Log = base.Logger;

            //  apply patches
            Harmony harmonyInstance = new Harmony("com.Zopheoma.YarsDialogSkip");
            harmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
