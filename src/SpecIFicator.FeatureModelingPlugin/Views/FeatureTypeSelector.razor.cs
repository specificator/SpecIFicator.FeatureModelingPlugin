using Microsoft.AspNetCore.Components;
using SpecIFicator.FeatureModelingPlugin.ViewModels;

namespace SpecIFicator.FeatureModelingPlugin.Views
{
    public partial class FeatureTypeSelector
    {
        private FeatureClasses _selectedType;
        [Parameter]
        public FeatureClasses SelectedType { get; set; }

        [Parameter]
        public EventCallback<FeatureClasses> SelectedTypeChanged { get; set; }

        private async Task OnSelectedTypeChanged(ChangeEventArgs args)
        {
             string selection = args.Value.ToString();
            foreach (FeatureClasses type in Enum.GetValues(typeof(FeatureClasses)))
            {
            if(type.ToString().Equals(selection))
                {
                    _selectedType = type;
                }
            }
             await SelectedTypeChanged.InvokeAsync(_selectedType);
        }
    }
}