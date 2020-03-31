using EPlast.BussinessLayer;
using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.Models.ViewModelInitializations.Interfaces;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace EPlast.XUnitTest
{
    public class DocumentationTest
    {
        [Fact]
        public void CreateDecesionTest()
        {
            //create
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanger = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var annualReportVMInitializer = new Mock<IAnnualReportVMInitializer>();
            var decisionVMIitializer = new Mock<IDecisionVMIitializer>();
            var pdfService = new Mock<IPDFService>();
            var hostingEnvironment = new Mock<IHostingEnvironment>();
            //settup
            repository.Setup(rep => rep.Organization.FindAll()).Returns(GetTestOrganizations());
            repository.Setup(rep => rep.DecesionTarget.FindAll()).Returns(GetTestDecesionTargets());
            //action
            var controller = new DocumentationController(repository.Object, usermanger.Object, annualReportVMInitializer.Object, decisionVMIitializer.Object, pdfService.Object,
                hostingEnvironment.Object);
            var result = controller.CreateDecesion();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<DecesionViewModel>(viewResult.Model);
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
    }
}