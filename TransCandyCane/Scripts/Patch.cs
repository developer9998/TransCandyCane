using HarmonyLib;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace TransCandyCane.Scripts
{
    [HarmonyPatch(typeof(VRRig), "Start", MethodType.Normal)]
    public class Patch
    {
        public static async void Postfix(VRRig __instance)
        {
            await Task.Delay(500);

            var Holdables = __instance.GetComponent<BodyDockPositions>().allObjects;
            var DefaultCandyCane = Holdables.First(a => a.name == "CANDY CANE");
            var BigCandyCane = Holdables.First(a => a.name == "LMACL.");

            try { Main.RegisterCandyCanes(new TransferrableObject[2] { DefaultCandyCane, BigCandyCane }); }
            catch (Exception exception) { Debug.LogException(exception); }
        }
    }
}
