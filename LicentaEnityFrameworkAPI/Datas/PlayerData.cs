﻿namespace LicentaEnityFrameworkAPI.Datas
{
    public class PlayerData
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public long ShirtNumber { get; set; }
        public string Position { get; set; } = null!;
        public long Age { get; set; }
        public string Team { get; set; } = null!;
        public int? Name { get; set; }
    }
}
