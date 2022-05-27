using UnityEngine;

namespace ToastForUnity.Script.Core
{
    public class ToastSetup : ScriptableObject
    {
        [Header("Global Setup : Don't Move This Path")]
        public ToastSettingSheet[] ToastSettingList;
    }
}