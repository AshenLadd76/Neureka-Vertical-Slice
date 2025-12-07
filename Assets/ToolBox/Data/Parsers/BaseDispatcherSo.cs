using System;
using UnityEngine;

 namespace ToolBox.Data.Parsers
 {
     [Serializable]
     public abstract class BaseDispatcherSo : ScriptableObject
     {
         public abstract void Dispatch(string path);
         
     }
 }