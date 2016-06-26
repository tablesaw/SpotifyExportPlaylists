using System.Collections.Generic;

namespace SpotifyExportPlaylists
{
    public class OwnerExternalUrls
    {
        public string spotify { get; set; }
    }

    public class Owner
    {
        public OwnerExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class TracksLink
    {
        public string href { get; set; }
        public int total { get; set; }
    }

    public class Playlist
    {
        public bool collaborative { get; set; }
        public ExternalUrls external_urls { get; set; }
        public string href { get; set; }
        public string id { get; set; }
        public List<Image> images { get; set; }
        public string name { get; set; }
        public Owner owner { get; set; }
        public bool @public { get; set; }
        public string snapshot_id { get; set; }
        public TracksLink tracks { get; set; }
        public string type { get; set; }
        public string uri { get; set; }
    }

    public class PlaylistsResponse
    {
        public string href { get; set; }
        public List<Playlist> items { get; set; }
        public int limit { get; set; }
        public string next { get; set; }
        public int offset { get; set; }
        public object previous { get; set; }
        public int total { get; set; }
    }
}