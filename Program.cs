using System.Diagnostics;

namespace MinecraftScraper
{
    class program
    {
        //IMPORTANT that this path is correct, change "C:\Users\anton\AppData\Roaming\.minecraft" to where YOUR copy of minecraft exists!!!
        public static string mineraftFolder = System.IO.Path.Combine(@"C:\Users\anton\AppData\Roaming\.minecraft", @"assets\objects");
        
        //IMPORTANT, this path is where everything will end up!
        public static string baseLocation = @"C:\Users\anton\source\repos\MinecraftFileScraper";

        public static Dictionary<String, int> entryValues = new Dictionary<string, int>
        {
            {"fileLocation", 1 },
            {"hash", 3 },
            {"size", 5 }
        };

        public static void Main(string[] args)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            //read from file
            string indexesLocation = @"C:\Users\anton\source\repos\MinecraftFileScraper\1.19.txt";
            string text = File.ReadAllText(indexesLocation);
            
            
            //process text
            string[] stringEntries = text.Split("}");

            //first one is an exceptional case
            stringEntries[0] = ", " + stringEntries[0].Replace("{\"objects\": {", String.Empty);

            List<Entry> entries = new List<Entry>();

            //make entries, the last 3 in stringEntries are just empty, so ignore them by subtracting 3
            for(int i = 0; i < stringEntries.Length-3; i++)
            {
                string[] data = stringEntries[i].Replace("\"", String.Empty).Replace(",", String.Empty).Replace(":", String.Empty).Split(" ");

                string location = data[entryValues["fileLocation"]];
                string hash = data[entryValues["hash"]];
                string size = data[entryValues["size"]];

                if (location.EndsWith(".ogg"))
                {
                    Entry entry = new Entry(location, hash, size);
                    entries.Add(entry);
                    Console.WriteLine(entry);
                }
            }
            Console.WriteLine("Time to make Entries: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Restart();

            for(int i = 0; i < entries.Count; i++)
            {
                CallFFMPEG(entries[i]);
            }
            Console.WriteLine("Time to make mp3s: " + stopwatch.ElapsedMilliseconds + "ms");
            stopwatch.Stop();
        }

        public static void MakeDirectory(string fileLocation)
        {
            //time to make folders
            string folderLocation = fileLocation;

            string newFolder = System.IO.Path.Combine(baseLocation, folderLocation);

            if (!System.IO.Directory.Exists(newFolder))
            {
                System.IO.Directory.CreateDirectory(newFolder);
            }
        }

        public static string CallFFMPEG(Entry entry)
        {
            //make the directory to make sure the folder exists
            MakeDirectory(entry.FileLocation);

            //call ffmpeg via cmd
            ProcessStartInfo psi = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                Arguments = "/C" + "C:\\ffmpeg\\bin\\ffmpeg.exe -i \"" + entry.HashLocation + "\" -q:a 0 \"" + System.IO.Path.Combine(baseLocation, entry.ResultLocation) + "\""
            };

            //variables for debugging
            string errors;
            string result;

            using (Process process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                result = process.StandardOutput.ReadToEnd();
            }

            return result;
        }
    }
}