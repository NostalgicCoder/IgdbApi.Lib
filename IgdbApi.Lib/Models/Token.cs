﻿namespace IgdbApi.Lib.Models
{
    public class Token
    {
        public string access_token { get; set; }
        public Int32 expires_in { get; set; }
        public string token_type { get; set; }
    }
}