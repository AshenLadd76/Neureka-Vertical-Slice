using System;
 using UnityEngine;
 using Logger = ToolBox.Utils.Logger;
 
 namespace ToolBox.Data.Parsers
 {
     [Serializable]
     public abstract class BaseDispatcherSo : ScriptableObject
     {
         public abstract void Dispatch(string path);
         
     }
 }