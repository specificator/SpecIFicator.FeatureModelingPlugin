using Microsoft.AspNetCore.Components;
using SpecIFicator.FeatureModelingPlugin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecIFicator.FeatureModelingPlugin.Views
{
    public partial class NodeButton
    {
        [Parameter]
        public String Type { get; set; }

        [Parameter]
        public bool IsRoot { get; set; }

        private bool Checked { get; set; } = false;
    }
}
