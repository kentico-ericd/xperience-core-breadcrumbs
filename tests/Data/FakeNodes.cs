using CMS.DataEngine;
using CMS.DocumentEngine;
using CMS.SiteProvider;

using System.Collections.Generic;
using System.Linq;

namespace Xperience.Core.Breadcrumbs.Tests.Data
{
    internal static class FakeNodes
    {
        public const string DEFAULT_SITE = "TestSite";
        public const string DOCTYPE_STANDARD = "Test.Standard";
        public const string DOCTYPE_FOLDER = "Test.Folder";
        public const string SITE_DOMAIN = "https://testsite";

        private static int nodeCount = 0;
        private static TreeNode standardOnRoot;
        private static TreeNode folderOnRoot;
        private static TreeNode standardWithFolderParent;
        private static TreeNode standardWithTwoParents;


        public static TreeNode StandardOnRoot
        {
            get
            {
                if (standardOnRoot == null)
                {
                    standardOnRoot = CreateNode("/s1", "s1");
                }

                return standardOnRoot;
            }
        }


        public static TreeNode FolderOnRoot
        {
            get
            {
                if (folderOnRoot == null)
                {
                    folderOnRoot = CreateNode("/f1", "f1", DOCTYPE_FOLDER);
                }

                return folderOnRoot;
            }
        }


        public static TreeNode StandardWithFolderParent
        {
            get
            {
                if (standardWithFolderParent == null)
                {
                    standardWithFolderParent = CreateNode("/f1/s2", "s2", nodeLevel: 2);
                    standardWithFolderParent.NodeParentID = FolderOnRoot.NodeID;
                }

                return standardWithFolderParent;
            }
        }


        public static TreeNode StandardWithTwoParents
        {
            get
            {
                if (standardWithTwoParents == null)
                {
                    standardWithTwoParents = CreateNode("/f1/s2/s3", "s3", nodeLevel: 3);
                    standardWithTwoParents.NodeParentID = StandardWithFolderParent.NodeID;
                }

                return standardWithTwoParents;
            }
        }


        private static IEnumerable<TreeNode> AllNodes => new List<TreeNode> {
            StandardOnRoot,
            FolderOnRoot,
            StandardWithFolderParent,
            StandardWithTwoParents
        };


        public static TreeNode GetById(int nodeId)
        {
            return AllNodes.FirstOrDefault(n => n.NodeID == nodeId);
        }


        private static TreeNode CreateNode(string nodeAliasPath, string name, string contentType = DOCTYPE_STANDARD, int nodeLevel = 1, string culture = "en-US", string site = DEFAULT_SITE)
        {
            nodeCount++;
            var nodeSite = SiteInfo.Provider.Get(site);
            var node = TreeNode.New(contentType).With(p =>
            {
                p.DocumentName = name;
                p.DocumentCulture = culture;
                p.SetValue(nameof(TreeNode.NodeID), nodeCount);
                p.SetValue(nameof(TreeNode.NodeLevel), nodeLevel);
                p.SetValue(nameof(TreeNode.DocumentID), nodeCount);
                p.SetValue(nameof(TreeNode.NodeSiteID), nodeSite.SiteID);
                p.SetValue(nameof(TreeNode.NodeAliasPath), nodeAliasPath);
            });

            return node;
        }
    }
}
