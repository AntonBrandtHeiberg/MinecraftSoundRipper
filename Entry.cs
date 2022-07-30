using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinecraftScraper
{
    internal class Entry
    {
        private string fileLocation; //where I want it to be!
        private string hash;
        private string size;

        private string name;
        private string hashLocation; //where it actually is

        //constructor
        public Entry(string fileLocation, string hash, string size)
        {
            string[] data = fileLocation.Split('/');
            name = data[data.Length - 1].Replace(".ogg", String.Empty);

            //file location should be minecraft\sounds\cave_sounds
            this.fileLocation = fileLocation.Replace("/" + name + ".ogg", String.Empty).Replace('/', '\\');
            this.hash = hash;
            this.size = size;

            hashLocation = System.IO.Path.Combine(program.mineraftFolder, System.IO.Path.Combine(hash.Substring(0,2), hash));
        }

        public string ResultLocation
        {
            get { return System.IO.Path.Combine(fileLocation, Name); }
        }

        public string Name
        {
            get { return name + ".mp3"; }
        }

        public string HashLocation
        {
            get { return hashLocation; }
        }

        public string FileLocation
        { 
            get { return fileLocation; } 
            set { fileLocation = value; }
        }

        public string Hash
        {
            get { return hash; }
            set { hash = value; }
        }

        public string Size
        {
            get { return size; }
            set { size = value; }
        }

        public override string ToString()
        {
            return String.Format("Name: {3} Location: {0}, hash: {1}, size: {2}mb \nResultLocation: {4}\n", FileLocation, Hash, Size, Name, ResultLocation);
        }

    }
}
