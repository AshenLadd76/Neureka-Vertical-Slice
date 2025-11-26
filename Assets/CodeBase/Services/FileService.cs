using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace CodeBase.Services
{
    public class FileService : MonoBehaviour
    {
        public void SaveData(WebData data)
        {
            Logger.Log( $"Saving web data with data id: {data.Id}" );
        }

        public void DeleteData(string dataId)
        {
            Logger.Log( $"Deleting web data with data id: {dataId}" );
        }
    }
}
