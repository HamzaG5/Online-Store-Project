namespace Domain.Models
{
    public class Image
    {
        public string ImageName { get; set; }

        public string ImageUrl { get; set; }

        public Image(string imageName, string imageUrl)
        {
            ImageName = imageName;
            ImageUrl = imageUrl;
        }
    }
}