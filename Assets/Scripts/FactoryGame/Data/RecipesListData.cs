using Sirenix.OdinInspector;
using UnityEngine;

namespace FactoryGame.Data
{
    [CreateAssetMenu(menuName = "Factory Data/" + nameof(RecipesListData))]
    public class RecipesListData : ScriptableObject
    {
        [TableList(AlwaysExpanded = true)]
        public Recipe[] recipes;
    }
}