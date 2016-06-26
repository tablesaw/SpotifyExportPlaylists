using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyExportPlaylists
{
    class PlaylistTrack
    {
        public string songId { get; set; }
        public string artist { get; set; }
        public string songName { get; set; }
        public string playlist { get; set; }
        public string playlistId { get; set; }
    }
}