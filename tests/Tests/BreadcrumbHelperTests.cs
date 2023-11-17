using NUnit.Framework;

using Xperience.Core.Breadcrumbs.Tests.Data;

namespace Xperience.Core.Breadcrumbs.Tests.Tests
{
    internal class BreadcrumbHelperTests
    {
        [TestFixture]
        internal class GetBreadcrumbsTests : BaseTest
        {
            private BreadcrumbHelper breadcrumbHelper;
            

            [SetUp]
            public void GetBreadcrumbsTestsSetUp()
            {
                breadcrumbHelper = new BreadcrumbHelper(
                    pageDataContextRetriever,
                    new DefaultBreadcrumbsRenderer(),
                    pageRetriever,
                    new DefaultBreadcrumbItemMapper(pageUrlRetriever),
                    new BreadcrumbsWidgetProperties()
                );
            }

            [Test]
            public void GetBreadcrumbs_StandardPageOnRoot_ReturnsExpectedString()
            {
                SetCurrentPage(FakeNodes.StandardOnRoot);

                var actual = breadcrumbHelper.GetBreadcrumbs().ToString();
                var expected = $"<div class='breadcrumbs-widget'> <span class='breadcrumb-item'><a href='{FakeNodes.SITE_DOMAIN}'>{FakeNodes.DEFAULT_SITE}</a></span> | <span class='breadcrumb-item breadcrumbs-current'>{FakeNodes.StandardOnRoot.DocumentName}</span></div>";

                Assert.That(actual, Is.EqualTo(expected));
            }


            [Test]
            public void GetBreadcrumbs_StandardPageWithFolderParent_ReturnsExpectedString()
            {
                SetCurrentPage(FakeNodes.StandardWithFolderParent);

                var actual = breadcrumbHelper.GetBreadcrumbs().ToString();
                var expected = $"<div class='breadcrumbs-widget'> <span class='breadcrumb-item'><a href='{FakeNodes.SITE_DOMAIN}'>{FakeNodes.DEFAULT_SITE}</a></span> | <span class='breadcrumb-item'>{FakeNodes.FolderOnRoot.DocumentName}</span> | <span class='breadcrumb-item breadcrumbs-current'>{FakeNodes.StandardWithFolderParent.DocumentName}</span></div>";

                Assert.That(actual, Is.EqualTo(expected));
            }


            [Test]
            public void GetBreadcrumbs_StandardPageWithTwoParents_ReturnsExpectedString()
            {
                SetCurrentPage(FakeNodes.StandardWithTwoParents);

                var actual = breadcrumbHelper.GetBreadcrumbs().ToString();
                var expected = $"<div class='breadcrumbs-widget'> <span class='breadcrumb-item'><a href='{FakeNodes.SITE_DOMAIN}'>{FakeNodes.DEFAULT_SITE}</a></span> | <span class='breadcrumb-item'>{FakeNodes.FolderOnRoot.DocumentName}</span> | <span class='breadcrumb-item'><a href='{FakeNodes.SITE_DOMAIN}{FakeNodes.StandardWithFolderParent.NodeAliasPath}'>{FakeNodes.StandardWithFolderParent.DocumentName}</a></span> | <span class='breadcrumb-item breadcrumbs-current'>{FakeNodes.StandardWithTwoParents.DocumentName}</span></div>";

                Assert.That(actual, Is.EqualTo(expected));
            }
        }


        [TestFixture]
        internal class BreadcrumbPropertiesTests : BaseTest
        {
            private BreadcrumbHelper breadcrumbHelper;
            private const string SEPARATOR = ".";
            private const string ITEM_CLASS = "item";
            private const string CURRENT_CLASS = "current";
            private const string CONTAINER_CLASS = "container";
            

            [SetUp]
            public void GetBreadcrumbsTestsSetUp()
            {
                breadcrumbHelper = new BreadcrumbHelper(
                    pageDataContextRetriever,
                    new DefaultBreadcrumbsRenderer(),
                    pageRetriever,
                    new DefaultBreadcrumbItemMapper(pageUrlRetriever),
                    new BreadcrumbsWidgetProperties
                    {
                        ShowSiteLink = false,
                        Separator = SEPARATOR,
                        ShowContainers = false,
                        ContainerClass = CONTAINER_CLASS,
                        CurrentPageClass = CURRENT_CLASS,
                        BreadcrumbItemClass = ITEM_CLASS
                    }
                );
            }

            [Test]
            public void GetBreadcrumbs_CustomProperties_ReturnsModifiedString()
            {
                SetCurrentPage(FakeNodes.StandardWithTwoParents);

                var actual = breadcrumbHelper.GetBreadcrumbs().ToString();
                var expected = $"<div class='{CONTAINER_CLASS}'> <span class='{ITEM_CLASS}'><a href='{FakeNodes.SITE_DOMAIN}{FakeNodes.StandardWithFolderParent.NodeAliasPath}'>{FakeNodes.StandardWithFolderParent.DocumentName}</a></span> {SEPARATOR} <span class='{ITEM_CLASS} {CURRENT_CLASS}'>{FakeNodes.StandardWithTwoParents.DocumentName}</span></div>";

                Assert.That(actual, Is.EqualTo(expected));
            }
        }
    }
}