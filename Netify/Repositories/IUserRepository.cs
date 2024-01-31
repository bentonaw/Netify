﻿using System.Xml.Linq;
using Microsoft.EntityFrameworkCore;
using NetifyAPI.Data;
using NetifyAPI.Models;
using NetifyAPI.Models.Dtos;
using NetifyAPI.Models.Viewmodels;

namespace NetifyAPI.Repositories
{
    public interface IUserRepository
    {
        // User

        User? GetUser(int userId);
        User? GetUserTracks(int userId);
        List<User> ListAllUsers();

        public void SaveTrack(string spotifyTrackId, string trackTitle, int userId, List<Artist> artists);
        public void SaveArtist(string spotifyArtistId, string artistName, int userId, int popularity, List<string> genres);

        void SaveUserToDatabase(UserDto userDto);



    }

    public class DbUserHandlerRepository : IUserRepository
    {
        private readonly NetifyContext _context;

        public DbUserHandlerRepository(NetifyContext context)
        {
            _context = context;
        }
        public List<User> ListAllUsers()
        {
            return _context.Users.ToList();
        }

        public User? GetUser(int userId)
        {
            User? user = _context.Users.
                Where(u => u.UserId == userId)
                //.Include(u => u.Genres)
                .Include(u => u.Artists)
                .Include(u => u.Tracks)
                .SingleOrDefault();
            return user;
        }

        public User? GetUserTracks(int userId)
        {
            User? user = _context.Users.
               Where(u => u.UserId == userId)
               .Include(u => u.Tracks)
                   .ThenInclude(t => t.Artists)
               .SingleOrDefault();
            return user;
        }

        public void SaveUserToDatabase(UserDto userDto)
        {
            try
            {
                _context.Users.Add(new User()
                {
                    Username = userDto.Username
                });
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to save to the database.", ex);
            }
        }

        // Saves a selected track to the database and connects to an user. Also saves the artists if they aren't already saved.
        public void SaveTrack(string spotifyTrackId, string trackTitle, int userId, List<Artist> artists)
        {
            // Iterates through the list of artists and adds each one to the database if they don't already exist.
            foreach (var artist in artists)
            {
                Artist existingArtist = _context.Artists.FirstOrDefault(a => a.SpotifyArtistId == artist.SpotifyArtistId);

                if (existingArtist == null)
                {
                    try
                    {
                        existingArtist = new Artist
                        {
                            SpotifyArtistId = artist.SpotifyArtistId,
                            ArtistName = artist.ArtistName
                        };

                        _context.Artists.Add(existingArtist);
                        _context.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Couldn't save artist to database. Exception: {ex}");
                    }
                }
            }

            // Checks if the track already exists in the database, if not, it is added.
            Track newTrack;

            if (_context.Tracks.Any(t => t.SpotifySongId == spotifyTrackId))
            {
                newTrack = _context.Tracks.Include(t => t.Artists).Where(t => t.SpotifySongId == spotifyTrackId).Single();
            }
            else
            {
                newTrack = new Track
                {
                    SpotifySongId = spotifyTrackId,
                    Title = trackTitle,
                    Artists = artists.Select(a => _context.Artists.Single(existingArtist => existingArtist.SpotifyArtistId == a.SpotifyArtistId)).ToList()
                };

                try
                {
                    _context.Tracks.Add(newTrack);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Couldn't save track to database. Exception: {ex}");
                }
            }

            // Selects the current user and connects the selected tracks in the database, given that the track isn't already connected.
            var user = _context.Users
                .Where(u => u.UserId == userId)
                .Include(u => u.Tracks)
                .SingleOrDefault();

            if (user != null && !user.Tracks.Contains(newTrack))
            {
                try
                {
                    user.Tracks?.Add(newTrack);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to add track. Exception: {ex}");
                }
            }
        }

        // Saves a selected track to the database and connects to an user. Also saves the artists if they aren't already saved.
        public void SaveArtist(string spotifyArtistId, int userId, string artistName, int popularity, List<string> genres)
        {

            
        }

        public void SaveArtist(string spotifyArtistId, string artistName, int userId, int popularity, List<string> genres)
        {
            Artist newArtist = _context.Artists.FirstOrDefault(a => a.SpotifyArtistId == spotifyArtistId);
            Genre genre = new Genre();
            genre.Name = genres.FirstOrDefault();

            if (newArtist == null)
            {
                try
                {
                    newArtist = new Artist
                    {
                        SpotifyArtistId = spotifyArtistId,
                        ArtistName = artistName,
                        Popularity = popularity,
                        MainGenre = genre
                    };

                    _context.Artists.Add(newArtist);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Couldn't save artist to database. Exception: {ex}");
                }
            }

            // Selects the current user and connects the selected tracks in the database, given that the artist isn't already connected.
            var user = _context.Users
                .Where(u => u.UserId == userId)
                .Include(u => u.Artists)
                .Include(u => u.Genres)
                .SingleOrDefault();

            // Adds the main genre of the artists to the users favorites.
            if (user != null)
            {
                try
                {
                    user.Genres.Add(genre);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to save genre. Exception: {ex}");
                }
            }

            if (user != null && !user.Artists.Contains(newArtist))
            {
                try
                {
                    user.Artists?.Add(newArtist);
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unable to add artist. Exception: {ex}");
                }
            }
        }
    }
}
