// using System.Globalization;
// using Microsoft.EntityFrameworkCore;
// using NoMercy.Database;
// using NoMercy.Helpers;
// using NoMercy.Server.app.Jobs;
// using NoMercy.Server.Logic;
// using NoMercy.Server.system;

using NoMercy.Helpers;
using NoMercy.Providers.TADB.Client;

namespace NoMercy.Server;

public class Dev
{
    // public class CollectionItem
    // {
    //     public int index { get; set; }
    //     public string type { get; set; }
    //     public string title { get; set; }
    //     public int year { get; set; }
    //     public int[] seasons { get; set; }
    //     public int[] episodes { get; set; }
    // }

    public static void Run()
    {
        // var artistClient = new ArtistClient();
        // var result = artistClient.ByMusicBrainzId(new Guid("056e4f3e-d505-4dad-8ec1-d04f521cbb56")).Result;
        // Logger.App(result?.Descriptions);
        //
        // var releaseGroupClient = new ReleaseGroupClient();
        // var result2 = releaseGroupClient.ByMusicBrainzId(new Guid("f9e8042a-674e-3f01-80ec-7f0ab1c537df")).Result;
        // Logger.App(result2?.Descriptions);
        
        // Task.Run(async () =>
        // {
        // CollectionItem[] items = 
        // [
        //     new CollectionItem{ 
        //         index = 1, 
        //         type = "movie", 
        //         title = "Captain America: The First Avenger",
        //         year = 2011,
        //     },
        //     new CollectionItem{ 
        //         index = 2, 
        //         type = "movie", 
        //         title = "Marvel One-Shot: Agent Carter",
        //         year = 2013,
        //     },
        //     new CollectionItem{ 
        //         index = 3, 
        //         type = "tv", 
        //         title = "Agent Carter",
        //         year = 2015,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 4, 
        //         type = "tv", 
        //         title = "Agent Carter",
        //         year = 2015,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 5, 
        //         type = "movie", 
        //         title = "Captain Marvel",
        //         year = 2019,
        //     },
        //     new CollectionItem{ 
        //         index = 6, 
        //         type = "movie", 
        //         title = "Iron Man",
        //         year = 2008,
        //     },
        //     new CollectionItem{ 
        //         index = 7, 
        //         type = "movie", 
        //         title = "Iron Man 2",
        //         year = 2010,
        //     },
        //     new CollectionItem{ 
        //         index = 8, 
        //         type = "movie", 
        //         title = "The Incredible Hulk",
        //         year = 2008,
        //     },
        //     new CollectionItem{ 
        //         index = 9, 
        //         type = "movie", 
        //         title = "The Consultant",
        //         year = 2011,
        //     },
        //     new CollectionItem{ 
        //         index = 10, 
        //         type = "movie", 
        //         title = "A Funny Thing Happened on the Way to Thor's Hammer",
        //         year = 2011,
        //     },
        //     new CollectionItem{ 
        //         index = 11, 
        //         type = "movie", 
        //         title = "Thor",
        //         year = 2011,
        //     },
        //     new CollectionItem{ 
        //         index = 12, 
        //         type = "movie", 
        //         title = "The Avengers",
        //         year = 2012,
        //     },
        //     new CollectionItem{ 
        //         index = 13, 
        //         type = "movie", 
        //         title = "Item 47",
        //         year = 2012,
        //     },
        //     new CollectionItem{ 
        //         index = 14, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [1],
        //         episodes = [1,2,3,4,5,6,7],
        //     },
        //     new CollectionItem{ 
        //         index = 15, 
        //         type = "movie", 
        //         title = "Thor: The Dark World",
        //         year = 2013,
        //     },
        //     new CollectionItem{ 
        //         index = 16, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [1],
        //         episodes = [8,9,10,11,12,13,14,15,16],
        //     },
        //     new CollectionItem{ 
        //         index = 17, 
        //         type = "movie", 
        //         title = "Iron Man 3",
        //         year = 2013,
        //     },
        //     new CollectionItem{ 
        //         index = 18, 
        //         type = "movie", 
        //         title = "All Hail the King",
        //         year = 2014,
        //     },
        //     new CollectionItem{ 
        //         index = 19, 
        //         type = "movie", 
        //         title = "Captain America: The Winter Soldier",
        //         year = 2014,
        //     },
        //     new CollectionItem{ 
        //         index = 20, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [1],
        //         episodes = [17,18,19,20,21,22],
        //     },
        //     new CollectionItem{ 
        //         index = 21, 
        //         type = "movie", 
        //         title = "Guardians of the Galaxy",
        //         year = 2014,
        //     },
        //     new CollectionItem{ 
        //         index = 22, 
        //         type = "movie", 
        //         title = "Guardians of the Galaxy Vol 2",
        //         year = 2017,
        //     },
        //     new CollectionItem{ 
        //         index = 23, 
        //         type = "tv", 
        //         title = "I Am Groot",
        //         year = 2022,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 24, 
        //         type = "tv", 
        //         title = "I Am Groot",
        //         year = 2022,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 25, 
        //         type = "tv", 
        //         title = "Daredevil",
        //         year = 2015,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 26, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [2],
        //         episodes = [1,2,3,4,5,6,7,8,9,10],
        //     },
        //     new CollectionItem{ 
        //         index = 27, 
        //         type = "tv", 
        //         title = "Jessica Jones",
        //         year = 2015,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 28, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [2],
        //         episodes = [11,12,13,14,15,16,17,18,19],
        //     },
        //     new CollectionItem{ 
        //         index = 29, 
        //         type = "movie", 
        //         title = "Avengers: Age of Ultron",
        //         year = 2015,
        //     },
        //     new CollectionItem{ 
        //         index = 30, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [2],
        //         episodes = [20,21,22],
        //     },
        //     new CollectionItem{ 
        //         index = 31, 
        //         type = "tv", 
        //         title = "Daredevil",
        //         year = 2015,
        //         seasons = [2],
        //         episodes = [1,2,3,4],
        //     },
        //     new CollectionItem{ 
        //         index = 32, 
        //         type = "tv", 
        //         title = "Luke Cage",
        //         year = 2016,
        //         seasons = [1],
        //         episodes = [1,2,3,4],
        //     },
        //     new CollectionItem{ 
        //         index = 33, 
        //         type = "tv", 
        //         title = "Daredevil",
        //         year = 2015,
        //         seasons = [2],
        //         episodes = [5,6,7,8,9,10,11],
        //     },
        //     new CollectionItem{ 
        //         index = 34, 
        //         type = "tv", 
        //         title = "Luke Cage",
        //         year = 2016,
        //         seasons = [1],
        //         episodes = [5,6,7,8],
        //     },
        //     new CollectionItem{ 
        //         index = 35, 
        //         type = "tv", 
        //         title = "Daredevil",
        //         year = 2015,
        //         seasons = [2],
        //         episodes = [12,13],
        //     },
        //     new CollectionItem{ 
        //         index = 36, 
        //         type = "tv", 
        //         title = "Luke Cage",
        //         year = 2016,
        //         seasons = [1],
        //         episodes = [9,10,11,12,13],
        //     },
        //     new CollectionItem{ 
        //         index = 37, 
        //         type = "movie", 
        //         title = "Ant-Man",
        //         year = 2015,
        //     },
        //     new CollectionItem{ 
        //         index = 38, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [3],
        //         episodes = [1,2,3,4,5,6,7,8,9,10],
        //     },
        //     new CollectionItem{ 
        //         index = 39, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [3],
        //         episodes = [11,12,13,14,15,16,17,18,19],
        //     },
        //     new CollectionItem{ 
        //         index = 40, 
        //         type = "tv", 
        //         title = "Iron Fist",
        //         year = 2017,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 41, 
        //         type = "movie", 
        //         title = "Captain America: Civil War",
        //         year = 2016,
        //     },
        //     new CollectionItem{ 
        //         index = 42, 
        //         type = "movie", 
        //         title = "Team Thor",
        //         year = 2016,
        //     },
        //     new CollectionItem{ 
        //         index = 43, 
        //         type = "movie", 
        //         title = "Team Thor: Part 2",
        //         year = 2017,
        //     },
        //     new CollectionItem{ 
        //         index = 44, 
        //         type = "movie", 
        //         title = "Black Widow",
        //         year = 2021,
        //     },
        //     new CollectionItem{ 
        //         index = 45, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [3],
        //         episodes = [20,21,22],
        //     },
        //     new CollectionItem{ 
        //         index = 46, 
        //         type = "tv", 
        //         title = "The Defenders",
        //         year = 2017,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 47, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [4],
        //         episodes = [1,2,3,4,5,6],
        //     },
        //     new CollectionItem{ 
        //         index = 48, 
        //         type = "movie", 
        //         title = "Doctor Strange",
        //         year = 2016,
        //     },
        //     new CollectionItem{ 
        //         index = 49, 
        //         type = "movie", 
        //         title = "Black Panther",
        //         year = 2018,
        //     },
        //     new CollectionItem{ 
        //         index = 50, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [4],
        //         episodes = [7,8],
        //     },
        //     new CollectionItem{ 
        //         index = 51, 
        //         type = "tv", 
        //         title = "Agents of SHIELD: Slingshot",
        //         year = 2016,
        //         seasons = [1],
        //         episodes = [1,2,3,4,5,6],
        //     },
        //     new CollectionItem{ 
        //         index = 52, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [4],
        //         episodes = [9,10,11,12,13,14,15,16,17,18,19,20,21,22],
        //     },
        //     new CollectionItem{ 
        //         index = 53, 
        //         type = "movie", 
        //         title = "Spider-Man: Homecoming",
        //         year = 2017,
        //     },
        //     new CollectionItem{ 
        //         index = 54, 
        //         type = "movie", 
        //         title = "Thor: Ragnarok",
        //         year = 2017,
        //     },
        //     new CollectionItem{ 
        //         index = 55, 
        //         type = "movie", 
        //         title = "Team Darryl",
        //         year = 2018,
        //     },
        //     new CollectionItem{ 
        //         index = 56, 
        //         type = "tv", 
        //         title = "Inhumans",
        //         year = 2017,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 57, 
        //         type = "tv", 
        //         title = "The Punisher",
        //         year = 2017,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 58, 
        //         type = "tv", 
        //         title = "Runaways",
        //         year = 2017,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 59, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [5],
        //         episodes = [1,2,3,4,5,6,7,8,9,10],
        //     },
        //     new CollectionItem{ 
        //         index = 60, 
        //         type = "tv", 
        //         title = "Jessica Jones",
        //         year = 2015,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 61, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [5],
        //         episodes = [11,12,13,14,15,16,17,18],
        //     },
        //     new CollectionItem{ 
        //         index = 62, 
        //         type = "tv", 
        //         title = "Cloak & Dagger",
        //         year = 2018,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 63, 
        //         type = "tv", 
        //         title = "Cloak & Dagger",
        //         year = 2018,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 64, 
        //         type = "tv", 
        //         title = "Luke Cage",
        //         year = 2016,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 65, 
        //         type = "tv", 
        //         title = "Iron Fist",
        //         year = 2017,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 66, 
        //         type = "tv", 
        //         title = "Daredevil",
        //         year = 2015,
        //         seasons = [3],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 67, 
        //         type = "tv", 
        //         title = "Runaways",
        //         year = 2017,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 68, 
        //         type = "tv", 
        //         title = "The Punisher",
        //         year = 2017,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 69, 
        //         type = "tv", 
        //         title = "Jessica Jones",
        //         year = 2015,
        //         seasons = [3],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 70, 
        //         type = "movie", 
        //         title = "Ant-Man and the Wasp",
        //         year = 2018,
        //     },
        //     new CollectionItem{ 
        //         index = 71, 
        //         type = "movie", 
        //         title = "Avengers: Infinity War",
        //         year = 2018,
        //     },
        //     new CollectionItem{ 
        //         index = 72, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [5],
        //         episodes = [19,20,21,22],
        //     },
        //     new CollectionItem{ 
        //         index = 73, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [6],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 74, 
        //         type = "tv", 
        //         title = "Agents of SHIELD",
        //         year = 2013,
        //         seasons = [7],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 75, 
        //         type = "tv", 
        //         title = "Runaways",
        //         year = 2017,
        //         seasons = [3],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 76, 
        //         type = "movie", 
        //         title = "Avengers: Endgame",
        //         year = 2019,
        //     },
        //     new CollectionItem{ 
        //         index = 77, 
        //         type = "tv", 
        //         title = "Loki",
        //         year = 2021,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 78, 
        //         type = "tv", 
        //         title = "Loki",
        //         year = 2021,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 79, 
        //         type = "tv", 
        //         title = "What If...?",
        //         year = 2021,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 80, 
        //         type = "tv", 
        //         title = "What If...?",
        //         year = 2021,
        //         seasons = [2],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 81, 
        //         type = "tv", 
        //         title = "WandaVision",
        //         year = 2021,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 82, 
        //         type = "tv", 
        //         title = "The Falcon and the Winter Soldier",
        //         year = 2021,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 83, 
        //         type = "movie", 
        //         title = "Shang-Chi and the Legend of the Ten Rings",
        //         year = 2021,
        //     },
        //     new CollectionItem{ 
        //         index = 84, 
        //         type = "movie", 
        //         title = "Eternals",
        //         year = 2021,
        //     },
        //     new CollectionItem{ 
        //         index = 85, 
        //         type = "movie", 
        //         title = "Spider-Man: Far From Home",
        //         year = 2019,
        //     },
        //     new CollectionItem{ 
        //         index = 86, 
        //         type = "movie", 
        //         title = "Spider-Man: No Way Home",
        //         year = 2021,
        //     },
        //     new CollectionItem{ 
        //         index = 87, 
        //         type = "movie", 
        //         title = "Doctor Strange in the Multiverse of Madness",
        //         year = 2022,
        //     },
        //     new CollectionItem{ 
        //         index = 88, 
        //         type = "tv", 
        //         title = "Hawkeye",
        //         year = 2021,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 89, 
        //         type = "tv", 
        //         title = "Moon Knight",
        //         year = 2022,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 90, 
        //         type = "movie", 
        //         title = "Black Panther: Wakanda Forever",
        //         year = 2022,
        //     },
        //     new CollectionItem{ 
        //         index = 91, 
        //         type = "tv", 
        //         title = "Echo",
        //         year = 2024,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 92, 
        //         type = "tv", 
        //         title = "She-Hulk: Attorney at Law",
        //         year = 2022,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 93, 
        //         type = "tv", 
        //         title = "Ms Marvel",
        //         year = 2022,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 94, 
        //         type = "movie", 
        //         title = "Thor: Love and Thunder",
        //         year = 2022,
        //     },
        //     new CollectionItem{ 
        //         index = 95, 
        //         type = "movie", 
        //         title = "Werewolf by Night",
        //         year = 2022,
        //     },
        //     new CollectionItem{ 
        //         index = 96, 
        //         type = "movie", 
        //         title = "The Guardians of the Galaxy Holiday Special",
        //         year = 2022,
        //     },
        //     new CollectionItem{ 
        //         index = 97, 
        //         type = "movie", 
        //         title = "Ant-Man and The Wasp: Quantumania",
        //         year = 2023,
        //     },
        //     new CollectionItem{ 
        //         index = 98, 
        //         type = "movie", 
        //         title = "Guardians of the Galaxy Vol 3",
        //         year = 2023,
        //     },
        //     new CollectionItem{ 
        //         index = 99, 
        //         type = "tv", 
        //         title = "Secret Invasion",
        //         year = 2023,
        //         seasons = [1],
        //         episodes = [],
        //     },
        //     new CollectionItem{ 
        //         index = 100, 
        //         type = "movie", 
        //         title = "The Marvels",
        //         year = 2023,
        //     },
        // ];
        //
        // using MediaContext context = new();
        //             
        // var movieLibrary = context.Libraries
        //     .Where(f => f.Type == "movie")
        //     .Include(l => l.FolderLibraries)
        //     .ThenInclude(fl => fl.Folder)
        //     .FirstOrDefault();
        //
        // var tvLibrary = context.Libraries
        //     .Where(f => f.Type == "tv")
        //     .Include(l => l.FolderLibraries)
        //     .ThenInclude(fl => fl.Folder)
        //     .FirstOrDefault();
        //
        // SearchClient client = new();
        // List<int> tvIds = [];
        // List<int> movieIds = [];     
        // List<SpecialItem> specialItems = [];
        //
        // Parallel.ForEachAsync(items, async (item, _) =>
        // {
        //     Logger.App($"Searching for {item.title} ({item.year})");
        //     switch (item.type)
        //     {
        //         case "movie":
        //         {
        //             var result = client.Movie(item.title, item.year.ToString()).Result;
        //             var movie = result?.Results.FirstOrDefault(
        //                 r => r.Title.ToLower().Contains("making of") == false);
        //
        //             if (movie is null) return;
        //             if (movieIds.Contains(movie.Id)) return;
        //
        //             movieIds.Add(movie.Id);
        //
        //             try
        //             {
        //                 await using MovieLogic movieLogic = new(movie.Id, movieLibrary);
        //                 await movieLogic.Process();
        //             }
        //             catch (Exception e)
        //             {
        //                 Console.WriteLine(e);
        //                 throw;
        //             }
        //
        //             break;
        //         }
        //         case "tv":
        //         {
        //             var result = client.TvShow(item.title, item.year.ToString()).Result;
        //             var tv = result?.Results.FirstOrDefault(r => r.Name.ToLower().Contains("making of") == false);
        //
        //             if (tv is null) return;
        //             if (tvIds.Contains(tv.Id)) return;
        //
        //             tvIds.Add(tv.Id);
        //
        //             try
        //             {
        //                 await using TvShowLogic tvShowLogic = new(tv.Id, tvLibrary);
        //                 await tvShowLogic.Process();
        //             }
        //             catch (Exception e)
        //             {
        //                 Console.WriteLine(e);
        //                 throw;
        //             }
        //
        //             break;
        //         }
        //     }
        // });
        //
        // foreach (var item in items)
        // {
        //     Logger.App($"Searching for {item.title} ({item.year})");
        //     switch (item.type)
        //     {
        //         case "movie":
        //         {
        //             var result = client.Movie(item.title, item.year.ToString()).Result;
        //             var movie = result?.Results.FirstOrDefault(r => r.Title.ToLower().Contains("making of") == false);
        //             if (movie is null) continue;
        //
        //             specialItems.Add(new SpecialItem
        //             {
        //                 SpecialId = Ulid.Parse("01HSBYSE7ZNGN7P586BQJ7W9ZB"),
        //                 MovieId = movie.Id,
        //                 Order = specialItems.Count,
        //             });
        //             
        //             break;
        //         }
        //         case "tv":
        //         {
        //             var result = client.TvShow(item.title, item.year.ToString()).Result;
        //             var tv = result?.Results.FirstOrDefault(r => r.Name.ToLower().Contains("making of") == false);
        //             if (tv is null) continue;
        //
        //             if (item.episodes is null)
        //             {
        //                 Logger.App(item);
        //                 throw new Exception("Episodes is null");
        //             }
        //
        //             if (item.episodes.Length == 0)
        //             {
        //                 item.episodes = context.Episodes
        //                     .Where(x => x.TvId == tv.Id)
        //                     .Where(x => x.SeasonNumber == item.seasons[0])
        //                     .Select(x => x.EpisodeNumber)
        //                     .ToArray();
        //             }
        //             
        //             foreach (var episodeNumber in item.episodes ?? [])
        //             {
        //                 var episode = context.Episodes
        //                     .FirstOrDefault(x =>
        //                         x.TvId == tv.Id 
        //                         && x.SeasonNumber == item.seasons[0] 
        //                         && x.EpisodeNumber == episodeNumber);
        //                 
        //                 if (episode is null) continue;
        //
        //                 specialItems.Add(new SpecialItem
        //                 {
        //                     SpecialId = Ulid.Parse("01HSBYSE7ZNGN7P586BQJ7W9ZB"),
        //                     EpisodeId = episode.Id,
        //                     Order = specialItems.Count,
        //                 });
        //             }
        //             
        //             break;
        //         }
        //     }
        // }
        //
        // Logger.App($"Upserting {specialItems.Count} SpecialItems");
        // context.SpecialItems.UpsertRange(specialItems.Where(s => s.MovieId is not null))
        //     .On(x => new { x.SpecialId, x.MovieId })
        //     .WhenMatched((old, @new) => new SpecialItem
        //     {
        //         SpecialId = @new.SpecialId,
        //         MovieId = @new.MovieId,
        //         Order = @new.Order,
        //     })
        //     .Run();
        // context.SpecialItems.UpsertRange(specialItems.Where(s => s.EpisodeId is not null))
        //     .On(x => new { x.SpecialId, x.EpisodeId })
        //     .WhenMatched((old, @new) => new SpecialItem
        //     {
        //         SpecialId = @new.SpecialId,
        //         EpisodeId = @new.EpisodeId,
        //         Order = @new.Order,
        //     })
        //     .Run();

        // });


        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var collections = mediaContext.Collections
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {collections.Count} Collection images");
        //
        //     foreach (var collection in collections)
        //     {
        //         if (collection is not { _colorPalette: "" }) continue;
        //
        //         Logger.Queue($"Fetching color palette for Collection Images {collection.Title}");
        //
        //         var palette =
        //             await ImageLogic2.GenerateColorPalette(collection.Poster, collection.Backdrop, download: true);
        //         collection._colorPalette = palette;
        //
        //         await mediaContext.SaveChangesAsync();
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var people = mediaContext.People
        //         .Where(x => x.Profile != null)
        //         .Where(x => x.Profile != "" && x.Profile != null)
        //         .ToList();
        //
        //     Logger.App($"Processing {people.Count} People images");
        //
        //     foreach (var person in people)
        //     {
        //         if (person is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(person.Id, "person");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 5);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var movies = mediaContext.Movies
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {movies.Count} Movie images");
        //
        //     foreach (var movie in movies)
        //     {
        //         if (movie is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(movie.Id, "movie");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 5);
        //     }
        // });

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var recommendations = mediaContext.Recommendations
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {recommendations.Count} Recommendation images");
        //
        //     foreach (var recommendation in recommendations)
        //     {
        //         if (recommendation is not { _colorPalette: "" }) continue;
        //         
        //         var type = recommendation.MovieFromId.HasValue ? "movie" : "tv";
        //         var id = recommendation.MovieFromId ?? recommendation.TvFromId ?? 0;
        //
        //         var colorPaletteJob = new ColorPaletteJob(id: id, model: "recommendation", type: type);
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 5);
        //     }
        // });
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var similars = mediaContext.Similar
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {similars.Count} Similar images");
        //
        //     foreach (var similar in similars)
        //     {
        //         if (similar is not { _colorPalette: "" }) continue;
        //         
        //         var type = similar.MovieFromId.HasValue ? "movie" : "tv";
        //         var id = similar.MovieFromId ?? similar.TvFromId ?? 0;
        //
        //         var colorPaletteJob = new ColorPaletteJob(id: id, model: "similar", type: type);
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 5);
        //     }
        // });

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var tvs = mediaContext.Tvs
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {tvs.Count} TvShow images");
        //
        //     foreach (var tv in tvs)
        //     {
        //         if (tv is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(tv.Id, "tv");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 4);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var seasons = mediaContext.Seasons
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {seasons.Count} Season images");
        //
        //     foreach (var season in seasons)
        //     {
        //         if (season is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(season.Id, "season");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 3);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var episodes = mediaContext.Episodes
        //         .Where(x => x._colorPalette == "")
        //         .ToList();
        //
        //     Logger.App($"Processing {episodes.Count} Episode images");
        //
        //     foreach (var episode in episodes)
        //     {
        //         if (episode is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new ColorPaletteJob(episode.Id, "episode");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        //     }
        // });

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var images = await mediaContext.Images
        //         .Where(x => x.Iso6391 == "en" || x.Iso6391 == null ||
        //                     x.Iso6391 == CultureInfo.CurrentCulture.TwoLetterISOLanguageName)
        //         .Where(x => x._colorPalette == "" && !x.FilePath!.Contains(".svg"))
        //         .Where(x => !x.ArtistId.HasValue)
        //         .Where(x => !x.AlbumId.HasValue)
        //         .ToListAsync();
        //
        //     Logger.App($"Processing {images.Count} images");
        //
        //     foreach (var image in images)
        //     {
        //         if (image is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob =
        //             new ColorPaletteJob(image.FilePath, "image", image.Iso6391);
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 1);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var albums = await mediaContext.Albums
        //         .Where(x => x.Cover != "" && x.Cover != null && x._colorPalette == "")
        //         .ToListAsync();
        //
        //     foreach (var album in albums)
        //     {
        //         if (album is not { _colorPalette: "" }) continue;
        //
        //         var colorPaletteJob = new MusicColorPaletteJob(album.Id.ToString(), "album");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        //     }
        // });

        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var artists = await mediaContext.Artists
        //         .Where(x => x.Cover != "" && x.Cover != null && x._colorPalette == "")
        //         .ToListAsync();
        //     
        //     foreach (var artist in artists)
        //     {
        //         if (artist is not {_colorPalette: "" }) continue;
        //         
        //         var colorPaletteJob = new MusicColorPaletteJob(id: artist.Id.ToString(), model: "artist");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        //     }
        // });
        //
        // Task.Run(async () =>
        // {
        //     await using MediaContext mediaContext = new();
        //     var tracks = await mediaContext.Tracks
        //         .Include(x => x.AlbumTrack)
        //         .Where(x => x.Cover != "" && x.Cover != null && x._colorPalette == "")
        //         .ToListAsync();
        //     
        //     foreach (var track in tracks)
        //     {
        //         if (track is not {_colorPalette: "" }) continue;
        //
        //         if (track.Cover is null) continue;
        //         
        //         var colorPaletteJob = new MusicColorPaletteJob(id: track.Id.ToString(), model: "track");
        //         JobDispatcher.Dispatch(colorPaletteJob, "image", 2);
        //     }
        // });
    }
}