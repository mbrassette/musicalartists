using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace MusicalArtists
{
    class MusicalArtists
    {
        public struct MusicalArtist
        {
            private String _Name;
            private Int32 _NumberOfListsItsIn;

            // Name of the artist
            public String Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

            // How many users' lists the artists appears in
            public Int32 NumberOfListsItsIn
            {
                get { return _NumberOfListsItsIn; }
                set { _NumberOfListsItsIn = value; }
            }
        }

        public MusicalArtist[] GetMusicalArtistsFromFile(String LocationOfFiles)
        {
            MusicalArtist[] MusicalArtists = null;

            // Set the location of the input file
            String ReadFileLocation = LocationOfFiles+"input.txt";

            // This is a list of musical artists appearing only once--a unique list of artists.
            List<String> lstOfUniqueArtistsInFile = new List<String>();

            // This is a list of musical artists that appear in one or more users' lists.  This list can contain multiple entries of the same artists.
            List<String> lstOfArtistsDumpInFile = new List<String>();

            // Reading data from flat file/CSV file
            using (var Reader = new StreamReader(ReadFileLocation))
            {
                Int32 i = 0;

                // keep reading until the entire file is read
                while (!Reader.EndOfStream)
                {
                    // Read one line of the input file
                    var line = Reader.ReadLine();

                    // Parse/split the line up using the ',' (comma) character
                    var values = line.Split(',');

                    // Stash the array of musical artists read into a list
                    List<String> lstOfReadLine = values.OfType<String>().ToList();

                    // Just store each individual artist found in file in this list.  Duplicates will be removed later
                    lstOfArtistsDumpInFile.AddRange(lstOfReadLine);

                    i++;
                }
            }

            // Remove the duplicates.  We now have our list of unique artists
            lstOfUniqueArtistsInFile = lstOfArtistsDumpInFile.Distinct().ToList();

            // Populate the MusicalArtists Data Structure Array
            MusicalArtists = SetUpDataStructures(lstOfUniqueArtistsInFile, lstOfArtistsDumpInFile);

            return MusicalArtists;
        }

        private MusicalArtist[] SetUpDataStructures(List<String> lstOfUniqueArtistsInFile, List<String> lstOfArtistsDumpInFile)
        {
            Int32 i = 0;
            
            // Create the number of MusicalArists data structures needed based on the number of unique/distinct artists in the input file
            MusicalArtist[] MusicalArtists = new MusicalArtist[lstOfUniqueArtistsInFile.Count];

            // Populate the data structures.  One data structure per unique artist read in the input file.
            foreach (var artist in lstOfUniqueArtistsInFile)
            {
                MusicalArtist MusicalArtist = new MusicalArtist
                {
                    Name = artist,
                    NumberOfListsItsIn = lstOfArtistsDumpInFile.Where(x => x.Equals(artist)).Count()
                };

                // Stash this one musical artist data structure in the array of musical artist data structures
                MusicalArtists[i] = MusicalArtist;
                i++;
            }

            return MusicalArtists;
        }

        public void WriteMusicalArtistsPairsToFile(MusicalArtist[] MusicalArtists, Int32 NumberOfListsArtistNeedsToBeOn, String LocationOfFiles)
        {
            // Set the location of the output file
            String WriteFileLocation = LocationOfFiles+"output.txt";

            // Writing results to text file
            using (StreamWriter Writer = new StreamWriter(WriteFileLocation))
            {
                // Process only the MusicalArtists data structures whose property "NumberOfListsItsIn" meets the minimum number of users' lists it's in.
                // It's on based on the NumberOfListsArtistNeedsToBeOn property.  Default value is set at 200.  It can be overwritten when running this application.

                /*
                   ** The business logic: If two artists meet the minimum value set by NumberOfListsArtistNeedsToBeOn, then those two artists will be included
                   in the ouput file as a pairing.  For example, if NumberOfListsArtistNeedsToBeOn is set to 200, the artist Coldplay is in 300 users' lists, 
                   and the artist Muse is in 250 users' list, then "Coldplay, Muse" will appear as a pairing in the output file.  For another example, if
                   NumberOfListsArtistNeedsToBeOn is set to 200, the artist Coldplay is in 300 users' lists, and the artist Lady Gaga is in 80 users' list,
                   then "Coldplay, Lady Gaga" will NOT appear as a pairing in the output file.  My logic assumes low probability that artists Coldplay
                   and Lady Gaga appear on at least 50 users' lists together.  Hence, that pairing being omitted from the output file.
                */


                // First artist in the pairing
                var firstInThePairingOfMusicalArtists = from item in MusicalArtists
                                                        where item.NumberOfListsItsIn >= NumberOfListsArtistNeedsToBeOn
                                                        orderby item.Name ascending
                                                        select item;

                foreach (var firstMusicalArtist in firstInThePairingOfMusicalArtists)
                {
                    // Second artist in the pairing
                    var secondInThePairingOfMusicalArtists = from item2 in MusicalArtists
                                                             where item2.NumberOfListsItsIn >= NumberOfListsArtistNeedsToBeOn
                                                             // this 2nd where clause makes sure we don't include duplicate pairings.
                                                             // For Example: "The Beatles, "The Beatles"
                                                             // also, this prevents duplicate pairings in the output file
                                                             // For Example: "Coldplay, Muse" and "Muse, Coldplay"
                                                             where String.Compare(firstMusicalArtist.Name, item2.Name) < 0
                                                             orderby item2.Name ascending
                                                             select item2;
                    foreach (var secondMusicalArtist in secondInThePairingOfMusicalArtists)
                    {
                        // Write the pairing to the output file
                        Writer.WriteLine("{0},{1}", firstMusicalArtist.Name, secondMusicalArtist.Name);
                    }
                }
            }
        }

        public void HaltProgram(String LocationOfFiles)
        {
            Console.WriteLine("Your output file can be found in this directory: " + LocationOfFiles + "output.txt.");
            Console.WriteLine("Press any key to close application...");
            Console.ReadKey(true);
        }

        static void Main(string[] args)
        {
            MusicalArtist[] MusicalArtists = new MusicalArtist[1000];
            MusicalArtists MusicalArtistsObject = new MusicalArtists();
            Int32 NumberOfListsArtistNeedsToBeOn = 0;
            String LocationOfFiles = "";

            // Process any external arugments passed into this application
            // Argument 1 is the number of lists that a musical artists must be in to be included as a pairing in the output file
            if (args.Count() >= 1)
            {
                NumberOfListsArtistNeedsToBeOn = Int32.Parse(args[0]);
            }
            else
            {
                // 200 seems to be a good number to set it at from my testing
                // If this number is too high, the possibility of valid pairings will be absent from the output file
                // If this number is too low, the possbility of invalid pairings will be present in the output fule
                NumberOfListsArtistNeedsToBeOn = 200;
            }
            // Argument 2 allows one to set where the input and output files are located
            if (args.Count() >= 2)
            {
                LocationOfFiles = args[1];
            }
            else
            {
                LocationOfFiles = @"C:\Temp\";
            }

            // All the reading of the file and setting up the data is done in this method
            MusicalArtists = MusicalArtistsObject.GetMusicalArtistsFromFile(LocationOfFiles);

            // All the writing to the output file is done in this method
            MusicalArtistsObject.WriteMusicalArtistsPairsToFile(MusicalArtists, NumberOfListsArtistNeedsToBeOn, LocationOfFiles);

            MusicalArtistsObject.HaltProgram(LocationOfFiles);
        }
    }
}
   