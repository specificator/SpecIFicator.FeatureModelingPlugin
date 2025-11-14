using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.ViewModels;
using SpecIFicator.FeatureModelingPlugin.Views;
using System.Runtime.CompilerServices;
using MDD4All.SpecIF.DataModels.Manipulation;

namespace SpecIFicator.FeatureModelingPlugin.ViewModels
{
    public static class NodeViewModelExtensions
    {
        public static Dictionary<Key, FeatureClasses> FeatureTypeDictionary;

        public static FeatureClasses FeatureClass(this NodeViewModel nodeViewModel)
        {
            FeatureClasses result = FeatureClasses.Unknown;

            if (nodeViewModel.Parent == null)
            {
                result = FeatureClasses.Mandatory;
            }

            Resource featureResource = nodeViewModel.ReferencedResource.Resource;
            string featureKind = featureResource.GetSingleStringPropertyValue(new Key("PC-FeatureKind", "1.1"));

            switch(featureKind)
            {
                case "V-FeatureKind-Mandatory":
                    result = FeatureClasses.Mandatory;
                    break;
                case "V-FeatureKind-Alternative":
                    result = FeatureClasses.Alternative;
                    break;
                case "V-FeatureKind-Optional":
                    result = FeatureClasses.Optional;
                    break;
                case "V-FeatureKind-Or":
                    result = FeatureClasses.Or;
                    break;
            }

            return result;
        }

        public static void SetFeatureClass(this NodeViewModel nodeViewModel)
        {
            if(FeatureTypeDictionary.ContainsKey(nodeViewModel.HierarchyKey))
            {
                FeatureTypeDictionary.Remove(nodeViewModel.HierarchyKey);
                FeatureTypeDictionary.Add(nodeViewModel.HierarchyKey, nodeViewModel.FeatureClass());
            }
            else
            {
                FeatureTypeDictionary.Add(nodeViewModel.HierarchyKey, nodeViewModel.FeatureClass());
            }
        }
    }
}
