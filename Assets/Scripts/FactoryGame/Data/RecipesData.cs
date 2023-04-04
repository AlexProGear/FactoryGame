using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryGame.Data
{
    [CreateAssetMenu(menuName = "Factory Data/" + nameof(RecipesData))]
    public class RecipesData : ScriptableObject
    {
        [TableList(AlwaysExpanded = true)]
        public Recipe[] recipes;
    }
}