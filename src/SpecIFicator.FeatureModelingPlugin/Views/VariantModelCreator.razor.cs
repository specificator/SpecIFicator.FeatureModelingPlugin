using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataProvider.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using SpecIFicator.FeatureModelingPlugin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MDD4All.SpecIF.DataModels.Manipulation;

namespace SpecIFicator.FeatureModelingPlugin.Views
{
    public partial class VariantModelCreator
    {
        [Inject]
        public IStringLocalizer<VariantModelCreator> L { get; set; }

        [Inject]
        private ISpecIfDataProviderFactory DataProviderFactory { get; set; }

        [CascadingParameter]
        public string? KeyAndProjectString { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        public VariantCreationViewModel DataContext { get; set; }

        protected override void OnInitialized()
        {
            string hierarchyKeyString = "";
            string projectID = "PRJ-DEFAULT";
            if (KeyAndProjectString != null)
            {
                if (KeyAndProjectString.Contains("/"))
                {
                    string[] tokens = KeyAndProjectString.Split("/");
                    hierarchyKeyString = tokens[0];
                    projectID = tokens[1];
                }
                else
                {
                    hierarchyKeyString = KeyAndProjectString;
                }

                Key hierarchyRootKey = new Key();
                hierarchyRootKey.InitailizeFromKeyString(hierarchyKeyString);

                DataContext = new VariantCreationViewModel(DataProviderFactory, hierarchyRootKey, projectID);
            }
        }

        private void OnNewVariantClose(bool accepted)
        {
            if(accepted)
            {
                DataContext.CreateVariantModelFromFeatureModel();
            }

            NavigationManager.NavigateTo("/pluginPage/B0E82D33-BE3C-4EF7-A8D3-6DE60AB4AB17/" + DataContext.ProjectID);
            DataContext.ShowCreationDialog = false;
            StateHasChanged();
        }
    }
}
