using MDD4All.SpecIF.ViewModels;
using MDD4All.UI.DataModels.Tree;
using Microsoft.AspNetCore.Components;
using SpecIFicator.FeatureModelingPlugin.ViewModels;

namespace SpecIFicator.FeatureModelingPlugin.Views
{
    public partial class FeatureModelTree
    {
        [CascadingParameter]
        public HierarchyViewModel DataContext { get; set; }

        public FeatureModelViewModel FeatureModelViewModel { get; set; }

        protected override void OnInitialized()
        {
            DataContext.PropertyChanged += OnPropertyChanged;

            FeatureModelViewModel = new FeatureModelViewModel(DataContext);

        }

        void OnSelectionChanged(ITreeNode node)
        {
            FeatureModelViewModel.HierarchyViewModel.SelectedNode = node as NodeViewModel;
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs arguments)
        {
            StateHasChanged();
        }
    }
}