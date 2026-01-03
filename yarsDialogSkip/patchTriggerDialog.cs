using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Yars;
using Yars.Dialog;
using Yars.Sequence;


namespace yarsDialogSkip
{
    [HarmonyPatch(typeof(SequenceManager), nameof(SequenceManager.TriggerDialog))]
    internal class patchTriggerDialog
    {
        //  create fields for accessing private variables of SequenceManager
        private static FieldInfo DialogStarted;
        private static FieldInfo IsCurrentlySkipping;
        private static FieldInfo ImportantEventDialog;

        [HarmonyPrepare]
        private static void Prepare()
        {
            //  provide access to private variables of SequenceManager
            DialogStarted = typeof(SequenceManager).GetField("m_dialogStarted", AccessTools.allDeclared);
            IsCurrentlySkipping = typeof(SequenceManager).GetField("m_IsCurrentlySkipping", AccessTools.allDeclared);
            ImportantEventDialog = typeof(SequenceManager).GetField("m_ImportantEventDialog", AccessTools.allDeclared);
        }

        [HarmonyPrefix]
        private static bool TriggerDialogPatch(SequenceManager __instance, DialogScript_Data _dialog)
        {
            //  get instance values of required private variables
            bool m_dialogStarted = (bool)DialogStarted.GetValue(__instance);
            bool m_IsCurrentlySkipping = (bool)IsCurrentlySkipping.GetValue(__instance);
            DialogScript_Data m_ImportantEventDialog = (DialogScript_Data)ImportantEventDialog.GetValue(__instance);

            //  single return path, default to running TriggerDialog
            bool rtnFalue = true;

            if (!m_dialogStarted)
            {
                DialogScript_Data importantEventDialog = m_ImportantEventDialog;
                if (((importantEventDialog != null) ? importantEventDialog.GetID : null) == _dialog.GetID)
                {
                    //  set return value to true to run TriggerDialog for important dialog
                    //  might change this in the future but need to find what's considered important and if skipping it has any consequences 
                    YarsDialogSkip.Log.LogInfo("Mal, hey listen, this is important event dialog");
                    rtnFalue = true;
                }
                else
                {
                    if (!m_IsCurrentlySkipping)
                    {
                        //  set return value to false to prevent running TriggerDialog for standard dialog
                        YarsDialogSkip.Log.LogInfo("Mal? Mal??? Can you hear me Mal????????????????????????");
                        rtnFalue = false;
                    }
                }
            }

            return rtnFalue;
        }
    }
}
