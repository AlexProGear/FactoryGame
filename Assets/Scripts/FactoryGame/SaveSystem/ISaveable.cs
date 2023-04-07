using System;

namespace FactoryGame.SaveSystem
{
    public interface ISavable
    {
        public Action ForceSave { get; set; }
        public string GetSaveData();
        public void LoadSaveData(string data);
    }
}