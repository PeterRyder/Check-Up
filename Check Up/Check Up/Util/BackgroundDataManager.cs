using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Check_Up.Util {
    class BackgroundDataManager {

        public static string BackgroundDataPath = FolderManager.DataDir + "\\" + "BackgroundData.xml";

        public BackgroundDataManager() {

        }

        /// <summary>
        /// Serialize the BackgroundData with the default filename
        /// </summary>
        /// <param name="backgroundData"></param>
        public static void SerializeData(List<BackgroundData> backgroundData) {
            WriteToXmlFile(BackgroundDataPath, backgroundData);
        }

        public static void WriteToXmlFile<T>(string filePath, T objectToWrite, bool append = false) where T : new() {
            TextWriter writer = null;
            try {
                var serializer = new XmlSerializer(typeof(T));
                writer = new StreamWriter(filePath, append);
                serializer.Serialize(writer, objectToWrite);
            }
            finally {
                if (writer != null)
                    writer.Close();
            }
        }

        public static T ReadFromXmlFile<T>(string filePath) where T : new() {
            TextReader reader = null;
            try {
                var serializer = new XmlSerializer(typeof(T));
                reader = new StreamReader(filePath);
                return (T)serializer.Deserialize(reader);
            }
            finally {
                if (reader != null)
                    reader.Close();
            }
        }


    }
}
