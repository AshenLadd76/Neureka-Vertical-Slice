using System;

namespace ToolBox.Services.Web
{
    public interface IFormData
    {
        public string Title { get;  }
        string JsonData { get;  }
        string Url { get;  }
  
    }

    public class FormData : IFormData
    {
        public string JsonData { get;  }
        public string Url { get;  }
        public string Title { get;  }

        public FormData( string title, string url, string jsonData )
        {
            // Validate parameters
            if (string.IsNullOrEmpty(title) || string.IsNullOrEmpty(url) || string.IsNullOrEmpty( jsonData ))
                throw new ArgumentException("All parameters must be non-null and non-empty.");
            
            
            this.JsonData = jsonData;
            this.Url = url;
            this.Title = title;
        }
    }
}