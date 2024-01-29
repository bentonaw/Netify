﻿using System.Text.Json.Serialization;

namespace NetifyAPI.Models.Viewmodels
{
    public class TrackSearchViewModel
    {
        public string SpotifyTrackId { get; set; }
        public string Title { get; set; }
        public ICollection<TrackArtistViewModel> Artists { get; set; }
    }
}
