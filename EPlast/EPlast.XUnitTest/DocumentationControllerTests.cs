using EPlast.BussinessLayer;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EPlast.Wrapper;
using Newtonsoft.Json.Linq;
using Xunit;

namespace EPlast.XUnitTest
{
    public class DocumentationControllerTests
    {
        private static DocumentationController CreateDocumentationController()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var userManager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var annualReportVmInitializer = new Mock<IAnnualReportVMInitializer>();
            var decisionVmInitializer = new Mock<IDecisionVMIitializer>();
            var pdfService = new Mock<IPDFService>();
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            var viewAnnualReportsVmInitializer = new Mock<IViewAnnualReportsVMInitializer>();
            var directoryManager = new Mock<IDirectoryManager>();
            var fileManager = new Mock<IFileManager>();
            repository.Setup(rep => rep.Organization.FindAll()).Returns(GetTestOrganizations());
            repository.Setup(rep => rep.DecesionTarget.FindAll()).Returns(GetTestDecesionTargets());
            repository.Setup(rep => rep.Decesion.Attach(new Decesion()));
            repository.Setup(rep => rep.Decesion.Create(new Decesion()));
            repository.Setup(rep => rep.Save());

            return new DocumentationController(repository.Object, userManager.Object, annualReportVmInitializer.Object, decisionVmInitializer.Object, pdfService.Object,
                hostingEnvironment.Object, viewAnnualReportsVmInitializer.Object, directoryManager.Object, fileManager.Object);
        }

        private static IQueryable<Organization> GetTestOrganizations()
        {
            var organization = new List<Organization>
            {
                 new Organization{ID=1,OrganizationName="Test1"},
                 new Organization{ID=2,OrganizationName="Test2"},
                 new Organization{ID=3,OrganizationName="Test3"}
            }.AsQueryable();
            return organization;
        }

        private static IQueryable<DecesionTarget> GetTestDecesionTargets()
        {
            var organization = new List<DecesionTarget>
            {
                 new DecesionTarget{ID = 1, TargetName = "First DecesionTarget"},
                 new DecesionTarget{ID = 2, TargetName = "Second DecesionTarget"},
                 new DecesionTarget{ID = 3, TargetName = "Third DecesionTarget"}
            }.AsQueryable();
            return organization;
        }

        [Fact]
        public void CreateDecesionTest()
        {
            var controller = CreateDocumentationController();

            var result = controller.CreateDecesion();

            Assert.IsType<DecesionViewModel>(result);
        }

        private static DecesionViewModel CreateDecesionViewModel(int DecesionTargetID = 1, bool haveFile = false) => new DecesionViewModel
        {
            Decesion = new Decesion
            {
                ID = 1,
                Name = "Test Decesion",
                DecesionStatusType = DecesionStatusType.InReview,
                DecesionTarget = new DecesionTarget { ID = DecesionTargetID, TargetName = "Test Decesion target" },
                Description = "Test Decesion Description",
                Organization = new Organization { ID = 1, OrganizationName = "Test Decesion Organization" },
                Date = System.DateTime.Now,
                HaveFile = haveFile
            }
        };

        public static IEnumerable<object[]> TestDecesionViewModel =>
        new List<object[]> {
            new object[]{CreateDecesionViewModel(), true },
            new object[]{CreateDecesionViewModel(DecesionTargetID: 0), true },
            new object[]{null, false}
        };

        //[Theory]
        //[MemberData(nameof(TestDecesionViewModel))]
        //public async Task SaveDecesionAsyncTestAsync(DecesionViewModel model, bool expected)
        //{
        //    var controller = CreateDocumentationController();

        //    var result = JObject.FromObject(await controller.SaveDecesionAsync(model)).First.Values();

        //    Assert.Equal(expected, result.Value<bool>());
        //}
    }
}