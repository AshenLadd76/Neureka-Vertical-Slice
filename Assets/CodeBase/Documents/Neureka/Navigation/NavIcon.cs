using UnityEngine;

namespace CodeBase.Documents.Neureka.Navigation
{
    [System.Serializable]
    public class NavIcon
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Selected { get; set; }
        [field: SerializeField] public Sprite UnSelected { get; set; }
    }
}