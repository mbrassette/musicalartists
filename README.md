# musicalartists

This project was coded in C# using the Community version of Visual Studio 2017.

You can load the solution file located in the parent directory: https://github.com/mbrassette/musicalartists/blob/master/MusicalArtists.sln with Visual Studio or some other compatiple IDE of your choosing.  This will allow you to see the entire solution and run the application.

I also included an executable you can run as well.  It is located here: https://github.com/mbrassette/musicalartists/blob/master/MusicalArtists/bin/Debug/MusicalArtists.exe

The following defaults are set in the application:
1. NumberOfListsArtistNeedsToBeOn = 200 -- I'll explain my reasoning for his later in the readme.
2. Default directory path where this application will be looking for the input file: c:\Temp\

You can override these two values if you are executing the application in the command prompt.  The first argument specifies the value for NumberOfListsArtistNeedsToBeOn and the second argument tells the application where the input file is located and where the output file will be placed.  Note, the default directory path must end with the '\' character.  For example: c:\Temp\ and not c:\Temp

Change the directory in the command prompt to the directory where MusicalArtists.exe is located and type this command: MusicalArtists 75 c:\Newlocation\ .  This means we lowered the NumberOfListsArtistNeedsToBeOn argument threshold from 200 to 75 and we changed the directory where the input file is located and where the output file will be placed from c:\Temp\ to c:\Newlocation\

Other notes about my application.  My application performs a best guess approach.  If two artists appear in 200 (default value of umberOfListsArtistNeedsToBeOn) users' list, then the application will assume those two artists appear in at least 50 users' lists together and show up as a pairing in the output file.  I chose 200 because it seemed the best happy medium value to set to give us back the "most" (note: not 100 perfect) accurate results possible.   If we set this value higher than 200, then the chance of leaving valid pairings out of the output file increases.  If we set this value lower than 200, then the chance of including invalid pairings in the output file increases.  Change application argument NumberOfListsArtistNeedsToBeOn to be different values.  As it goes higher, fewer pairings show up in the output file.  As it decreases, more pairings show up in the output file. The best guess approach sacrifices 100% accuracy for performance gains.  I could have developed the application differently by guaranteeing 100% accuracy, but then the application would have run much much slower.  You can never have the best of both worlds.
