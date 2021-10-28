using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace NGS.ExtendableSaveSystem
{
    public class SaveMaster : MonoBehaviour
    {
        protected ISavableComponent[] GetOrderedSavableComponents()
        {
            return FindObjectsOfType(typeof(Component))
                .Where(c => c is ISavableComponent)
                .Select(c => (ISavableComponent)c)
                .OrderBy(c => c.executionOrder)
                .ToArray(); 
        }

        public virtual void Save(string folderPath, string fileName, string fileFormat)
        {
            if (!folderPath.EndsWith("/")) folderPath += "/";
            if (!fileFormat.StartsWith(".")) fileFormat = "." + fileFormat;

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            Dictionary<int, ComponentData> componentsData = new Dictionary<int, ComponentData>();

            foreach (var savableComponent in GetOrderedSavableComponents())
            {
                //Debug.Log("Unique id when saving: " + savableComponent.uniqueID);
                componentsData.Add(savableComponent.uniqueID, savableComponent.Serialize());
            }
                

            BinaryFormatter formatter = new BinaryFormatter();

            if(File.Exists(folderPath + fileName + fileFormat))
            {
                Debug.Log("Deleteing file of save game if existed before");
                File.Delete(folderPath + fileName + fileFormat);
            }
            using (FileStream stream = new FileStream(folderPath + fileName + fileFormat, FileMode.Create))
            formatter.Serialize(stream, componentsData);

            // There is an issue with GetLastWriteTime on Windows, it could not get the corretct Time. It's a feature of windows systems.
            // In small files like here it can work correctly, but beware of it and remember that there could be an issue wiht it in future
            // Windows Cache System has some delay to catch that the file was deleted, here it was something around 5 seconds,
            // copy-paste of new saved file with the same name could work if handled well with this system delay.

            //using (FileStream stream = new FileStream(folderPath + fileName + "A" + fileFormat, FileMode.Create))
            //File.Copy(folderPath + fileName + "A" + fileFormat, folderPath + fileName + fileFormat);
            //Debug.Log("1a: " + File.GetCreationTime(folderPath + fileName + "A" + fileFormat));
            //File.Delete(folderPath + fileName + "A" + fileFormat);
            //Debug.Log("1: " + File.GetCreationTime(folderPath + fileName + fileFormat));
            //Debug.Log("2: " + File.GetLastWriteTime(folderPath + fileName + fileFormat));
        }

        public virtual void Load(string folderPath, string fileName, string fileFormat)
        {
            if (!folderPath.EndsWith("/")) folderPath += "/";
            if (!fileFormat.StartsWith(".")) fileFormat = "." + fileFormat;

            if (!Directory.Exists(folderPath))
            throw new DirectoryNotFoundException("SaveMaster::Directory '" + folderPath + "' not found"); 

            if (File.Exists(folderPath + fileName + fileFormat))
            {
                Dictionary<int, ComponentData> componentsData = null;

                BinaryFormatter formatter = new BinaryFormatter();
                using (FileStream stream = new FileStream(folderPath + fileName + fileFormat, FileMode.Open))
                    componentsData = (Dictionary<int, ComponentData>)formatter.Deserialize(stream);

                foreach (var savableComponent in GetOrderedSavableComponents())
                    if (componentsData.ContainsKey(savableComponent.uniqueID))
                    {
                        savableComponent.Deserialize(componentsData[savableComponent.uniqueID]);
                    }
            }
            else
            {
                Debug.Log("File doesn't exist");
            }
            
            

        }
    }
}
