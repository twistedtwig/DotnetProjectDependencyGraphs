using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ProjectReferences.Interfaces;
using ProjectReferences.Models;
using ProjectReferences.Output;
using ProjectReferences.Shared;

namespace ProjectReference
{
    public static class Manager
    {
        /// <summary>
        /// Creates the rootNode collection from the analysisRequest.  Will interigate the solution and proejcts to find all other projects and their relationships.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static RootNode CreateRootNode(AnalysisRequest request)
        {
            if (!File.Exists(request.RootFile))
            {
                throw new FileNotFoundException(request.RootFile);
            }

            var rootFileInfo = new FileInfo(request.RootFile);            

            var rootNode = new RootNode
                {
                    Directory = rootFileInfo.Directory, 
                    File = rootFileInfo, 
                    Name = rootFileInfo.Name, 
                    NodeType = DetermineRootNodeType(rootFileInfo.FullName), 
                    SearchDepth = request.NumberOfLevelsToDig
                };

            return rootNode;
        }

        private static RootNodeType DetermineRootNodeType(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (filePath.ToLower().Trim().EndsWith(".sln"))
            {
                return RootNodeType.SLN;
            }

            if (filePath.ToLower().Trim().EndsWith(".csproj"))
            {
                return RootNodeType.CSPROJ;
            }

            throw new ArgumentOutOfRangeException("unknown file extension");
        }

        public static void Process(RootNode rootNode)
        {
            switch (rootNode.NodeType)
            {
                case RootNodeType.SLN:
                    ProcessSlnRootNode(rootNode);
                    return;
                case RootNodeType.CSPROJ:
                    ProcessCsProjRootNode(rootNode);
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void ProcessSlnRootNode(RootNode rootNode)
        {
            var projectLinks = new SolutionFileManager().FindAllProjectLinks(rootNode);
            ProcessLinks(projectLinks, rootNode);
        }

        private static void ProcessCsProjRootNode(RootNode rootNode)
        {
            ProcessLinks(new List<InvestigationLink> { new InvestigationLink {FullPath = rootNode.File.FullName, Parent = null} }, rootNode);
        }

        private static void ProcessLinks(List<InvestigationLink> linksToBeInvestigated, RootNode rootNode)
        {
            while (linksToBeInvestigated.Any())
            {
                var item = linksToBeInvestigated[0];
                linksToBeInvestigated.RemoveAt(0);

                var link = new ProjectLinkObject {FullPath = item.FullPath};
                var projectDetail = new ProjectFileManager().Create(link);
                if (item.Parent != null)
                {
                    projectDetail.ParentProjects.Add(item.Parent);
                }

                link.Id = projectDetail.Id;

                //get all child links and create link investigations for them
                linksToBeInvestigated.AddRange(projectDetail.ChildProjects.Select(p => new InvestigationLink { FullPath = p.FullPath, Parent = link }).ToList());

                rootNode.ProjectDetails.Add(projectDetail);                
            }
        }


        public static OutputResponse CreateOutPut(AnalysisRequest request, RootNode rootNode)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (rootNode == null) throw new ArgumentNullException("rootNode");

            IOutputProvider outputProvider = OutputFactory.CreateProvider(request.OutPutType);
            return outputProvider.Create(rootNode, request.OutPutFolder);
        }
    }
}
