using MDD4All.SpecIF.DataFactory;
using MDD4All.SpecIF.DataModels;
using MDD4All.SpecIF.DataModels.Helpers;
using MDD4All.SpecIF.DataModels.Manipulation;
using MDD4All.SpecIF.DataProvider.Contracts;
using MDD4All.SpecIF.ViewModels;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpecIFicator.FeatureModelingPlugin.ViewModels
{
    public class VariantCreationViewModel
    {
        private ISpecIfDataProviderFactory _specIfDataProviderFactory;

        public string ProjectID { get; set; }

        public VariantCreationViewModel(ISpecIfDataProviderFactory specIfDataProviderFactory,
                                        Key keyToFeatureModel,
                                        string projectID)
        {
            ProjectID = projectID;
            _specIfDataProviderFactory = specIfDataProviderFactory;


            FeatureModelSource = new HierarchyViewModel(specIfDataProviderFactory, keyToFeatureModel, projectID);

            Resource variantModelResource = SpecIfDataFactory.CreateResource(new Key("RC-VariantModel", "1.1"));

            VariantModelRoot = new ResourceViewModel(specIfDataProviderFactory.MetadataReader,
                                                     specIfDataProviderFactory.DataReader,
                                                     specIfDataProviderFactory.DataWriter,
                                                     variantModelResource);
            VariantModelRoot.IsInEditMode = true;
        }

        public HierarchyViewModel FeatureModelSource { get; set; }

        public Node VariantModelTarget { get; set; }

        public bool ShowCreationDialog { get; set; } = true;

        public ResourceViewModel VariantModelRoot { get; set; }

        public void CreateVariantModelFromFeatureModel()
        {
            Resource? variantModelRootResource = VariantModelRoot.Resource;

            if (variantModelRootResource != null)
            {
                _specIfDataProviderFactory.DataWriter.AddResource(variantModelRootResource, ProjectID);


                Node variantModelRootNode = new Node();
                variantModelRootNode.ResourceReference = new Key(variantModelRootResource.ID,
                                                                 variantModelRootResource.Revision);


                foreach (NodeViewModel child in FeatureModelSource.RootNode.Children)
                {
                    CopyNodes(child, variantModelRootNode);
                }

                // hierarchy des Variant models speichern
                _specIfDataProviderFactory.DataWriter.AddHierarchy(variantModelRootNode, ProjectID);

            }

        }

        private void CopyNodes(NodeViewModel currentSource, Node currentTarget)
        {
            // erzeuge neuen Knoten und neue Resource und füge sie dem Target an
            Resource sourceResource = _specIfDataProviderFactory.DataReader.GetResourceByKey(currentSource.ReferencedResource.Key);

            Resource resourceCopy = sourceResource.CreateNewRevisionForEdit(_specIfDataProviderFactory.MetadataReader);
            resourceCopy.ID = SpecIfGuidGenerator.CreateNewSpecIfGUID();

            _specIfDataProviderFactory.DataWriter.AddResource(resourceCopy, ProjectID);

            Node newTargetNode = new Node();

            newTargetNode.ResourceReference = new Key(resourceCopy.ID, resourceCopy.Revision);

            currentTarget.Nodes.Add(newTargetNode);

            
            // rekursiver aufruf für die Kinder von currentSource
            foreach (NodeViewModel childNode in currentSource.Children)
            {
                if (currentSource.Children != null)
                {
                    CopyNodes(childNode, newTargetNode);
                }
            }

        }
    }
}
