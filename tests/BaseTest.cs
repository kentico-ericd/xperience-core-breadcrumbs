using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.SiteProvider;
using CMS.Tests;

using Kentico.Content.Web.Mvc;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using NSubstitute;
using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.IO;

using Tests.DocumentEngine;

using Xperience.Core.Breadcrumbs.Tests.Data;

namespace Xperience.Core.Breadcrumbs.Tests
{
    internal abstract class BaseTest : UnitTests
    {
        protected TreeNode currentPage;
        protected readonly IPageUrlRetriever pageUrlRetriever = Substitute.For<IPageUrlRetriever>();
        protected readonly IPageDataContext<TreeNode> pageDataContext = Substitute.For<IPageDataContext<TreeNode>>();
        protected readonly IPageRetriever pageRetriever = Substitute.For<IPageRetriever>();
        protected readonly IPageDataContextRetriever pageDataContextRetriever = Substitute.For<IPageDataContextRetriever>();


        [SetUp]
        public void SetUp()
        {
            // Register sites and document types for faking
            DocumentGenerator.RegisterDocumentType<TreeNode>(FakeNodes.DOCTYPE_STANDARD);
            Fake().DocumentType<TreeNode>(FakeNodes.DOCTYPE_STANDARD, dci => {
                dci.ClassFormDefinition = GetClassFormDefinition(FakeNodes.DOCTYPE_STANDARD);
                dci.ClassHasURL = true;
                dci.ClassIsCoupledClass = true;
            });
            DocumentGenerator.RegisterDocumentType<TreeNode>(FakeNodes.DOCTYPE_FOLDER);
            Fake().DocumentType<TreeNode>(FakeNodes.DOCTYPE_FOLDER, dci => {
                dci.ClassFormDefinition = GetClassFormDefinition(FakeNodes.DOCTYPE_FOLDER);
                dci.ClassHasURL = false;
                dci.ClassIsCoupledClass = false;
            });
            Fake<SiteInfo, SiteInfoProvider>().WithData(new SiteInfo
            {
                SiteName = FakeNodes.DEFAULT_SITE,
                DisplayName = FakeNodes.DEFAULT_SITE,
                SitePresentationURL = FakeNodes.SITE_DOMAIN
            });

            // Fake URL for pages
            pageUrlRetriever.Retrieve(Arg.Any<TreeNode>(), Arg.Any<bool>()).Returns(args =>
            {
                var result = new PageUrl();
                var node = args.Arg<TreeNode>();
                var dci = DataClassInfoProvider.GetDataClassInfo(node.ClassName);
                if (!dci.ClassHasURL)
                {
                    result.AbsoluteUrl = String.Empty;
                }
                else
                {
                    result.AbsoluteUrl = $"{args.Arg<TreeNode>().Site.SitePresentationURL}{args.Arg<TreeNode>().NodeAliasPath}";
                }

                return result;
            });

            // Returns all parent pages of the current page
            pageRetriever.RetrieveMultiple(Arg.Any<Action<MultiDocumentQuery>>()).Returns(args =>
            {
                var nodeList = new List<TreeNode>();
                var parentId = currentPage.NodeParentID;
                while (parentId > 0)
                {
                    var parent = FakeNodes.GetById(parentId);
                    nodeList.Add(parent);
                    parentId = parent.NodeParentID;
                }

                return nodeList;
            });
        }


        protected void SetCurrentPage(TreeNode page)
        {
            currentPage = page;
            pageDataContext.Page.Returns(page);
            pageDataContextRetriever.TryRetrieve(out Arg.Any<IPageDataContext<TreeNode>>()).Returns(x =>
            {
                x[0] = pageDataContext;
                return true;
            });
        }


        private string GetClassFormDefinition(string className)
        {
            var json = JsonConvert.DeserializeObject<JObject>(File.ReadAllText("Data/classFormDefinitions.json"));

            return json[className]?.Value<string>() ?? String.Empty;
        }
    }
}
