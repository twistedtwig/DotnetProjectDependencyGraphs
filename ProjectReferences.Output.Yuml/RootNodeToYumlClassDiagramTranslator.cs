using System;
using System.Collections.Generic;
using ProjectReferences.Models;
using ProjectReferences.Output.Yuml.Models;
using ProjectReferences.Shared;
using YumlOutput.Class;
using YumlOutput.Class.Models;
using YumlOutput.Class.Relationships;

namespace ProjectReferences.Output.Yuml
{
    public class RootNodeToYumlClassDiagramTranslator
    {
        public RootNodeToYumlClassDiagramTranslator()
        {
            Logger.Log("Creating instance of RootNodeToYumlClassDiagramTranslator", LogLevel.High);            
        }

        public YumlClassOutput Translate(RootNode rootNode, bool newlineForEachRelationship = false)
        {
            Logger.Log("Translating rootNode to YumlClassOutput", LogLevel.High);

            var output = new YumlClassOutput();
            output.RootFile = rootNode.File.FullName;

            output.ClassDiagram = GenerateClassDiagram(rootNode, newlineForEachRelationship); 
            return output;
        }

        public YumlClassOutput Translate(ProjectDetail rootProjectDetail, ProjectDetailsCollection parentProjectDetailsCollection, bool newlineForEachRelationship = false)
        {
            Logger.Log("Translating ProjectDetail to YumlClassOutput", LogLevel.High);

            var output = new YumlClassOutput();
            output.RootFile = rootProjectDetail.FullPath;

            output.ClassDiagram = GenerateClassDiagram(rootProjectDetail, parentProjectDetailsCollection, newlineForEachRelationship);
            return output;
        }

        private YumlClassDiagram GenerateClassDiagram(ProjectDetail rootProjectDetail, ProjectDetailsCollection parentProjectDetailsCollection, bool newlineForEachRelationship)
        {
            var classDiagram = new YumlClassDiagram(newlineForEachRelationship);

            GenerateClassDiagramRelationships(rootProjectDetail, parentProjectDetailsCollection, classDiagram.Relationships, newlineForEachRelationship);
            return classDiagram;
        }

        private void GenerateClassDiagramRelationships(ProjectDetail projectDetail, ProjectDetailsCollection parentProjectDetailsCollection, List<YumlRelationshipBase> existingRelationships, bool newlineForEachRelationship)
        {
            AddUnqiueRelationship(existingRelationships, GenerateYumlRelationships(projectDetail, parentProjectDetailsCollection, newlineForEachRelationship));
            foreach (var linkObject in projectDetail.ChildProjects)
            {
                var detail = parentProjectDetailsCollection.GetById(linkObject.Id);
                GenerateClassDiagramRelationships(detail, parentProjectDetailsCollection, existingRelationships, newlineForEachRelationship);
            }
        }

        private YumlClassDiagram GenerateClassDiagram(RootNode rootNode, bool newlineForEachRelationship)
        {
            var classDiagram = new YumlClassDiagram(newlineForEachRelationship);
            foreach (var detail in rootNode.ProjectDetails)
            {
                classDiagram.Relationships.AddRange(GenerateYumlRelationships(detail, rootNode.ProjectDetails, newlineForEachRelationship));
            }
            return classDiagram;
        }

        private List<YumlRelationshipBase> GenerateYumlRelationships(ProjectDetail projectDetail, ProjectDetailsCollection projectDetailsCollection, bool newlineForEachRelationship)
        {
            var relationships = new List<YumlRelationshipBase>();
            var detailModel = GenerateClass(projectDetail);

            foreach (var linkObject in projectDetail.ChildProjects)
            {
                var childModel = GenerateClass(projectDetailsCollection.GetById(linkObject.Id));
                relationships.Add(new SimpleAssociation
                {
                    Parent = detailModel,
                    Child = childModel
                });
            }

            foreach (var dllReference in projectDetail.References)
            {
                var childModel = GenerateClass(dllReference);
                relationships.Add(new SimpleAssociation
                {
                    Parent = detailModel,
                    Child = childModel
                });
            }

            return relationships;
        }

        private YumlClassWithDetails GenerateClass(ProjectDetail projectDetail)
        {
            var detailModel = new YumlClassWithDetails();
            detailModel.Name = projectDetail.Name;
            detailModel.Notes.Add(".Net Version: " + projectDetail.DotNetVersion);

            return detailModel;
        }

        private YumlClassWithDetails GenerateClass(DllReference dllReference)
        {
            var detailModel = new YumlClassWithDetails();
            detailModel.Name = dllReference.AssemblyName;// string.Format("{0}.dll", dllReference.AssemblyName);
            detailModel.Notes.Add(
                string.Format("External Reference{0}", 
                string.IsNullOrWhiteSpace(dllReference.Version) ? "" : string.Format(" ({0})", dllReference.Version)
                )
            );

            return detailModel;
        }

        private void AddUnqiueRelationship(List<YumlRelationshipBase> existingRelationships, IList<YumlRelationshipBase> newRelationships)
        {
            foreach (var relationship in newRelationships)
            {
                if (!existingRelationships.Contains(relationship))
                {
                    existingRelationships.Add(relationship);
                }
            }
        }
    }
}
