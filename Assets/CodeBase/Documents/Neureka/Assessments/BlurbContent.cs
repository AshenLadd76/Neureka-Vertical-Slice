namespace CodeBase.Documents.Neureka.Assessments
{
    public class BlurbContent
    {
        public BlurbContent(string id, string blurb, string imagePath)
        {
            Id = id;
            Blurb = blurb;
            ImagePath = imagePath;
        }
        
        public string Id { get; set; }
        public string Blurb { get; set; }
        public string ImagePath { get; set; }
    }
}