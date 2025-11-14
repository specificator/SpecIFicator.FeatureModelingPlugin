using GalaSoft.MvvmLight.Command;
using MDD4All.SpecIF.DataFactory;
using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataModels.Helpers;
using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.ViewModels;
using MDD4All.UI.DataModels.Tree;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using SpecIFicator.FeatureModelingPlugin.ViewModels;
using MDD4All.SpecIF.DataModels.Manipulation;

namespace SpecIFicator.FeatureModelingPlugin.ViewModels
{
    public class FeatureModelViewModel
    {
        public FeatureClasses SelectedType { get; set; }

        public HierarchyViewModel HierarchyViewModel { get; set; }

        private ISpecIfMetadataReader _metadataReader;

        private ISpecIfDataWriter _specIfDataWriter;

        private ISpecIfDataReader _specIfDataReader;

        public bool IsInVariantMode = false; 

        public FeatureModelViewModel(HierarchyViewModel hierarchyViewModel)
        {
            HierarchyViewModel = hierarchyViewModel;
            InitializeCommands();
            _metadataReader = hierarchyViewModel.MetadataReader;
            _specIfDataReader = hierarchyViewModel.DataReader;
            _specIfDataWriter = hierarchyViewModel.DataWriter;
            hierarchyViewModel.SelectedResourceClassKey = new Key("RC-FeatureModelFeature", "1.1");
        }
        private void InitializeCommands()
        {
            AddNewChildCommand = new RelayCommand<FeatureClasses>(ExecuteAddNewChild);
            AddVariantCommand = new RelayCommand(ExecuteAddVariant);
        }

        public ICommand AddNewChildCommand { get; private set; }
        public ICommand AddVariantCommand { get; private set; }

        private Dictionary<Key, NodeViewModel> ChangedNodes = new Dictionary<Key, NodeViewModel>();

        private void ExecuteAddNewChild(FeatureClasses featureClass)
        {
            Resource newResource = SpecIfDataFactory.CreateResource(HierarchyViewModel.SelectedResourceClassKey,
                                                                    _metadataReader);

            switch(featureClass)
            {
                case FeatureClasses.Mandatory:
                    newResource.SetPropertyValue(new Key("PC-FeatureKind", "1.1"), new Value("V-FeatureKind-Mandatory"));
                    break;
                case FeatureClasses.Alternative:
                    newResource.SetPropertyValue(new Key("PC-FeatureKind", "1.1"), new Value("V-FeatureKind-Alternative"));
                    break;
                case FeatureClasses.Optional:
                    newResource.SetPropertyValue(new Key("PC-FeatureKind", "1.1"), new Value("V-FeatureKind-Optional"));
                    break;
                case FeatureClasses.Or:
                    newResource.SetPropertyValue(new Key("PC-FeatureKind", "1.1"), new Value("V-FeatureKind-Or"));
                    break;
            }

            //newResource.SetPropertyValue()
            Node selectedNode = ((NodeViewModel)HierarchyViewModel.SelectedNode).HierarchyNode;
            
            HierarchyViewModel.ResourceUnderEdit = new ResourceViewModel(_metadataReader,
                                                                         _specIfDataReader,
                                                                         _specIfDataWriter,
                                                                         newResource);

            HierarchyViewModel.EditType = HierarchyViewModel.NEW_CHILD;

            HierarchyViewModel.ConfirmEditResourceCommand.Execute(null);

        }

        private void ExecuteAddVariant()
        {
            IsInVariantMode = true;
        }
    }
}