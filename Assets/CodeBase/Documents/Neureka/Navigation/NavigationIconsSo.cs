using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Documents.Neureka.Navigation
{
    [CreateAssetMenu(fileName = "NavIcons", menuName = "UI/NavIconsSO")]
    public class NavigationIconsSo : ScriptableObject
    {
        [SerializeField] private List<NavIcon> navIconList;

        public List<NavIcon> NavIconList => navIconList;
        
    }
}
