using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace SpotifyExportPlaylists
{
    class Program
    {
        const string getPlaylistsForCurrentUserUri = "https://api.spotify.com/v1/me/playlists";
        const string token = ""; // https://developer.spotify.com/web-api/console/get-current-user-playlists/ use web interface to log into spotify and generate oAuth token
        //maybe need playlist-read-collaborative scope also?
        const string filePathToSave = @""; //example:  C:\users\me\myfile.txt

        static void Main(string[] args)
        {
            List<PlaylistTrack> playlistTracks = new List<PlaylistTrack>();

            var playlists = GetAllPlaylists();

            foreach (var playlist in playlists)
            {
                var tracks = GetAllTracksForPlaylist(playlist.tracks.href);

                foreach (var track in tracks)
                {
                    playlistTracks.Add(new PlaylistTrack()
                    {
                        artist = track.artists[0].name,
                        songName = track.name,
                        playlist = playlist.name,
                        songId = track.id,
                        playlistId = playlist.id
                    });
                }
            }

            //or do something more exciting than writing to a file.
            TextWriter tw = new StreamWriter(filePathToSave, true); //has to be existing file
            foreach (var playlistTrack in playlistTracks)
            {
                string line = string.Format("{0}\t{1}\t{2}\t{3}\t{4}", playlistTrack.playlistId, playlistTrack.songId, playlistTrack.playlist, playlistTrack.songName, playlistTrack.artist);
                tw.WriteLine(line);
            }

            tw.Close();
            Console.ReadLine();
        }

        public static PlaylistsResponse GetPlaylists(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Bearer " + token);

            try
            {
                WebResponse response = request.GetResponse();

                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    var y = reader.ReadToEnd();

                    var w = JsonConvert.DeserializeObject<PlaylistsResponse>(y);
                    return w;
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        public static TracksForPlaylistResponse GetTracksForPlaylist(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("Authorization", "Bearer " + token);

            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return JsonConvert.DeserializeObject<TracksForPlaylistResponse>(reader.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

        public static List<Playlist> GetAllPlaylists()
        {
            var playlists = new List<Playlist>();

            string nextUrl = getPlaylistsForCurrentUserUri;

            while (nextUrl != null)
            {
                var response = GetPlaylists(nextUrl);
                playlists.AddRange(response.items);
                nextUrl = response.next;
            }
            return playlists;
        }

        public static List<Track> GetAllTracksForPlaylist(string initialTracksurl)
        {
            var tracks = new List<Track>();

            string nextUrl = initialTracksurl;

            while (nextUrl != null)
            {
                var response = GetTracksForPlaylist(nextUrl);
                tracks.AddRange(response.items.Select(x => x.track).ToList());
                nextUrl = response.next;
            }
            return tracks;
        }
    }
}
