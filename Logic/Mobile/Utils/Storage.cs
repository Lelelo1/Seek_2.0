using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Newtonsoft.Json;
using XamarinLogic.Models.Analytics;
using System.Threading.Tasks;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Concurrent;

namespace XamarinLogic.Utils
{
    /*
    public class Storage<Base>
    {
        string FilePath { get; }
        JsonSerializerSettings Settings { get; }
        ConcurrentDictionary<string, object> Locks { get; }
        public Storage(string fileName)
        {
            // path is said to be mostly private:https://docs.microsoft.com/en-us/dotnet/api/xamarin.essentials.filesystem.appdatadirectory?view=xamarin-essentials
            FilePath = Path.Combine(FileSystem.AppDataDirectory, fileName);
            Settings = new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.All
            };

            Locks = new ConcurrentDictionary<string, object>();
            Locks.TryAdd(FilePath, new object());
        }

        void Write(List<Base> t)
        {
            var json = JsonConvert.SerializeObject(t, Settings);
            lock(Locks[FilePath])
            {
                File.WriteAllText(FilePath, json);
            }
        }

        public void WriteOrAppend(Base t)
        {

            var list = Read();
            list.Add(t);
            Write(list);
        }

        public List<Base> Read()
        {


            var json = ReadFile();
            var list = JsonConvert.DeserializeObject<List<Base>>(json, Settings);

            return list;
        }

        public string ReadFile()
        {
            try
            {
                string text = "";
                lock (Locks[FilePath])
                {
                    text = File.ReadAllText(FilePath);
                }
                return text;
            }
            catch(Exception exc)
            {
                Console.WriteLine("exccc: " + exc.Message);
                var list = new List<Base>();
                Write(list);
                return JsonConvert.SerializeObject(list);
            }
        }
    }
    */
}
