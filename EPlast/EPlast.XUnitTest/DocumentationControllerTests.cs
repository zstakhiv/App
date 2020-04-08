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
using Xunit;

namespace EPlast.XUnitTest
{
    public class DocumentationControllerTests
    {
        private DocumentationController CreateDocumentationController()
        {
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanger = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var annualReportVMInitializer = new Mock<IAnnualReportVMInitializer>();
            var decisionVMIitializer = new Mock<IDecisionVMIitializer>();
            var pdfService = new Mock<IPDFService>();
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            var viewAnnualReportsVMInitializer = new Mock<IViewAnnualReportsVMInitializer>();
            repository.Setup(rep => rep.Organization.FindAll()).Returns(GetTestOrganizations());
            repository.Setup(rep => rep.DecesionTarget.FindAll()).Returns(GetTestDecesionTargets());
            repository.Setup(rep => rep.Decesion.Attach(new Decesion()));
            repository.Setup(rep => rep.Decesion.Create(new Decesion()));
            repository.Setup(rep => rep.Save());

            return new DocumentationController(repository.Object, usermanger.Object, annualReportVMInitializer.Object, decisionVMIitializer.Object, pdfService.Object,
                hostingEnvironment.Object, viewAnnualReportsVMInitializer.Object);
        }

        private IQueryable<Organization> GetTestOrganizations()
        {
            var organization = new List<Organization>
            {
                 new Organization{ID=1,OrganizationName="Test1"},
                 new Organization{ID=2,OrganizationName="Test2"},
                 new Organization{ID=3,OrganizationName="Test3"}
            }.AsQueryable();
            return organization;
        }

        private IQueryable<DecesionTarget> GetTestDecesionTargets()
        {
            var organization = new List<DecesionTarget>
            {
                 new DecesionTarget{ID = 1, TargetName = "First DecesionTarget"},
                 new DecesionTarget{ID = 2, TargetName = "Second DecesionTarget"},
                 new DecesionTarget{ID = 3, TargetName = "Third DecesionTarget"}
            }.AsQueryable();
            return organization;
        }

        //[Fact]
        //public void CreateDecesionTest()
        //{
        //    var controller = CreateDocumentationController();

        //    var result = controller.CreateDecesion();

        //    var viewResult = Assert.IsType<ViewResult>(result);

        //    Assert.IsAssignableFrom<DecesionViewModel>(viewResult.Model);
        //}

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
            new object[]{CreateDecesionViewModel(), "CreateDecesion" },
            new object[]{CreateDecesionViewModel(DecesionTargetID: 0), "CreateDecesion" },
            new object[]{null, "CreateDecesion" }
        };

        //[Theory]
        //[MemberData(nameof(TestDecesionViewModel))]
        //public async Task SaveDecesionAsyncTestAsync(DecesionViewModel model, string expected)
        //{
        //    var controller = CreateDocumentationController();

        //    var result = (RedirectToActionResult)await controller.SaveDecesionAsync(model);

        //    Assert.Equal(expected, result.ActionName);
        //}
    }
}