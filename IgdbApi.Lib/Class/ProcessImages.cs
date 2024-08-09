namespace IgdbApi.Lib.Class
{
    public class ProcessImages
    {
        /// <summary>
        /// URLs from the API need to be processed in order to acquire larger artwork otherwise a default thumbnail (90x90) version will be returned.
        /// - cover_big = 264 x 374
        /// - 720p = 1280 x 720
        /// - 1080p = 1920 x 1080
        /// </summary>
        /// <param name="coverUrl"></param>
        /// <returns></returns>
        public string ProcessCoverUrl(string coverUrl)
        {
            string[] urlElements = coverUrl.Split('/');

            urlElements[6] = urlElements[6].Remove(urlElements[6].Length - 5, 5);
            urlElements[6] = urlElements[6] + "cover_big";

            return String.Join("/", urlElements);
        }
    }
}