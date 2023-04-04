using System;
using Sirenix.OdinInspector;

namespace FactoryGame.Data
{
    [Serializable]
    public class Recipe
    {
        [ListDrawerSettings(Expanded = true)]
        public ItemData[] inputs;
        [ListDrawerSettings(Expanded = true)]
        public ItemData[] outputs;
    }
}