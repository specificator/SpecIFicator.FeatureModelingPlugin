using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SpecIFicator.FeatureModelingPlugin.ViewModels;
using System.Diagnostics;

namespace SpecIFicator.FeatureModelingPlugin.Views
{
    public partial class FeatureModelToolBar
    {
        [Inject]
        private IStringLocalizer<FeatureModelToolBar> L { get; set; }

        [Inject]
        private ISpecIfDataProviderFactory DataProviderFactory { get; set; }

        [Parameter]
        public FeatureClasses SelectedType { get; set; }

        [CascadingParameter]
        public HierarchyViewModel DataContext { get; set; }

        public FeatureModelViewModel FeatureModelViewModel { get; set; }

        private string _projectID = "PRJ-DEFAULT";

        public Key SelectedResourceClassKey {  get; set; }
        public string ProjectID
        {
            get { return _projectID; }
            set { _projectID = value; }
        }

        protected override void OnInitialized()
        {
            FeatureModelViewModel = new FeatureModelViewModel(DataContext);
            SelectedResourceClassKey = new Key("RC-Feature"); 
            DataContext.PropertyChanged += OnPropertyChanged;
            _projectID = DataContext.DataReader.GetProjectIDFromNodeID(DataContext.RootNode.NodeID).ToString();
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            StateHasChanged();
        }

        public void OnNewChildClicked()
        {
            Debug.Print(SelectedType.ToString());
            FeatureModelViewModel.AddNewChildCommand.Execute(SelectedType);
            StateHasChanged();
        }

        public void OnDeleteRessourceClicked()
        {
            DataContext.StartDeleteResourceCommand.Execute(null);
            StateHasChanged();
        }
        public void OnAddVariantClicked()
        {
            FeatureModelViewModel.AddVariantCommand.Execute(null);
            StateHasChanged();
        }
        public void OnSaveClicked()
        {
            FeatureModelViewModel.IsInVariantMode = false;
        }
    }
}