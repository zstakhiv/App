using EPlast.BussinessLayer.Interfaces;
using EPlast.Controllers;
using EPlast.DataAccess.Entities;
using EPlast.DataAccess.Repositories;
using EPlast.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Xunit;

namespace EPlast.XUnitTest
{
    public class ClubControllerTests
    {
        [Fact]
        public void ClubPageTest()
        {
            //constructor
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var hostingEnvironment = new Mock<IHostingEnvironment>();

            //arrange
            repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
                new List<Club>{
                 new Club {
                     ID = 1,
                     Logo = "null",
                     Description = "Some Text",
                     ClubName = "Клуб номер 1",
                     ClubAdministration = new List<ClubAdministration>{
                         new ClubAdministration{
                             ClubMembers = new ClubMembers{
                                    IsApproved = true,
                                    User = new User
                                    {
                                        LastName = "Andrii",
                                        FirstName = "Ivanenko"
                                    }
                             },
                             StartDate = DateTime.Today,
                             AdminType = new AdminType
                             {
                                 AdminTypeName = "Курінний"
                             }
                         }
                     },
                     ClubMembers = new List<ClubMembers>{
                         new ClubMembers{
                            IsApproved = true,
                            User = new User
                            {
                                LastName = "Andrii",
                                FirstName = "Ivanenko"
                            }
                         },
                         new ClubMembers{
                            IsApproved = false,
                            User = new User
                            {
                                LastName = "Ivan",
                                FirstName = "Ivanenko"
                            }
                         },
                         new ClubMembers{
                            IsApproved = false,
                            User = new User
                            {
                                LastName = "Orest",
                                FirstName = "Ivanenko"
                            }
                         }
                     }

                 }
            }.AsQueryable());
            
            //action
            var controller = new ClubController(repository.Object, usermanager.Object, hostingEnvironment.Object);
            var result = controller.Club(1);
            
            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<ClubViewModel>(viewResult.Model);
        }
        [Fact]
        public void IndexTest()
        {
            //constructor
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var hostingEnvironment = new Mock<IHostingEnvironment>();

            //arrange
            repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
                new List<Club>{
                 new Club {
                     ID = 1,
                     Logo = "null",
                     Description = "Some Text",
                     ClubName = "Клуб номер 1",
                     ClubAdministration = new List<ClubAdministration>{
                         new ClubAdministration{
                             ClubMembers = new ClubMembers{
                                    IsApproved = true,
                                    User = new User
                                    {
                                        LastName = "Andrii",
                                        FirstName = "Ivanenko"
                                    }
                             },
                             StartDate = DateTime.Today,
                             AdminType = new AdminType
                             {
                                 AdminTypeName = "Курінний"
                             }
                         }
                     },
                     ClubMembers = new List<ClubMembers>{
                         new ClubMembers{
                            IsApproved = true,
                            User = new User
                            {
                                LastName = "Andrii",
                                FirstName = "Ivanenko"
                            }
                         },
                         new ClubMembers{
                            IsApproved = false,
                            User = new User
                            {
                                LastName = "Ivan",
                                FirstName = "Ivanenko"
                            }
                         },
                         new ClubMembers{
                            IsApproved = false,
                            User = new User
                            {
                                LastName = "Orest",
                                FirstName = "Ivanenko"
                            }
                         }
                     }

                 }
            }.AsQueryable());

            //action
            var controller = new ClubController(repository.Object, usermanager.Object, hostingEnvironment.Object);
            var result = controller.Index();

            //assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsAssignableFrom<List<ClubViewModel>>(viewResult.Model);
        }
        [Fact]
        public void EditClubTest()
        {
            //for constructor
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var hostingEnvironment = new Mock<IHostingEnvironment>();

            // arrange
            var controller = new ClubController(repository.Object, usermanager.Object, hostingEnvironment.Object);
            var mockFile = new Mock<IFormFile>();
            repository.Setup(r => r.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
                new List<Club>
                {
                    new Club{
                        ClubName = "Club 2",
                        Description = "Some text"
                }
                }.AsQueryable());
            var expected = new Club
            {
               ClubName = "Club 2",
               Description = "Some text"
            };
            // action
            var viewModel = new ClubViewModel { Club = expected };
            var result = controller.EditClub(viewModel, mockFile.Object);
            // assert
            repository.Verify(r => r.Club.Update(It.IsAny<Club>()), Times.Once());
        }
        [Fact]
        public void EditClubTestWithoutClubName()
        {
            //for constructor
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var hostingEnvironment = new Mock<IHostingEnvironment>();

            // arrange
            var controller = new ClubController(repository.Object, usermanager.Object, hostingEnvironment.Object);
            var mockFile = new Mock<IFormFile>();
            repository.Setup(r => r.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
                new List<Club>
                {
                    new Club{
                        ClubName = "Some text",
                        Description = "Some text"
                }
                }.AsQueryable());
            var expected = new Club
            {
                ClubName = "",
                Description = "Some text"
            };
            // action
            var viewModel = new ClubViewModel { Club = expected };
            var result = controller.EditClub(viewModel, mockFile.Object);
            // assert
            repository.Verify(r => r.Club.Update(It.IsAny<Club>()), Times.Once());
        }
        [Fact]
        public void ChangeIsApprovedStatusTest()
        {
            //constructor
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var hostingEnvironment = new Mock<IHostingEnvironment>();

            //arrange
            repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
                new List<Club>{
                 new Club {
                     ID = 1,
                     Logo = "null",
                     Description = "Some Text",
                     ClubName = "Клуб номер 1",
                     ClubMembers = new List<ClubMembers>{
                         new ClubMembers{
                            ID = 1,
                            IsApproved = true,
                            User = new User
                            {
                                LastName = "Andrii",
                                FirstName = "Ivanenko"
                            }
                         }
                     }
                 }
            }.AsQueryable());

            repository.Setup(c => c.ClubMembers.FindByCondition(It.IsAny<Expression<Func<ClubMembers, bool>>>())).Returns(
               new List<ClubMembers>{
                         new ClubMembers{
                            ID = 1,
                            IsApproved = true,
                            User = new User
                            {
                                LastName = "Andrii",
                                FirstName = "Ivanenko"
                            }
                         }
           }.AsQueryable());

            //action
            var controller = new ClubController(repository.Object, usermanager.Object, hostingEnvironment.Object);
            var result = controller.ChangeIsApprovedStatus(1, 1);
            // assert
            repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());

        }

        [Fact]
        public void ChangeIsApprovedStatusClubTest()
        {
            //constructor
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var hostingEnvironment = new Mock<IHostingEnvironment>();

            //arrange
            repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
                new List<Club>{
                 new Club {
                     ID = 1,
                     Logo = "null",
                     Description = "Some Text",
                     ClubName = "Клуб номер 1",
                     ClubMembers = new List<ClubMembers>{
                         new ClubMembers{
                            ID = 1,
                            IsApproved = true,
                            User = new User
                            {
                                LastName = "Andrii",
                                FirstName = "Ivanenko"
                            }
                         }
                     }
                 }
            }.AsQueryable());

            repository.Setup(c => c.ClubMembers.FindByCondition(It.IsAny<Expression<Func<ClubMembers, bool>>>())).Returns(
               new List<ClubMembers>{
                         new ClubMembers{
                            ID = 1,
                            IsApproved = true,
                            User = new User
                            {
                                LastName = "Andrii",
                                FirstName = "Ivanenko"
                            }
                         }
           }.AsQueryable());

            //action
            var controller = new ClubController(repository.Object, usermanager.Object, hostingEnvironment.Object);
            var result = controller.ChangeIsApprovedStatusClub(1, 1);
            // assert
            repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());

        }
        [Fact]
        public void ChangeIsApprovedStatusFollowersTest()
        {
            //constructor
            var repository = new Mock<IRepositoryWrapper>();
            var store = new Mock<IUserStore<User>>();
            var usermanager = new Mock<UserManager<User>>(store.Object, null, null, null, null, null, null, null, null);
            var hostingEnvironment = new Mock<IHostingEnvironment>();

            //arrange
            repository.Setup(c => c.Club.FindByCondition(It.IsAny<Expression<Func<Club, bool>>>())).Returns(
                new List<Club>{
                 new Club {
                     ID = 1,
                     Logo = "null",
                     Description = "Some Text",
                     ClubName = "Клуб номер 1",
                     ClubMembers = new List<ClubMembers>{
                         new ClubMembers{
                            ID = 1,
                            IsApproved = true,
                            User = new User
                            {
                                LastName = "Andrii",
                                FirstName = "Ivanenko"
                            }
                         }
                     }
                 }
            }.AsQueryable());

            repository.Setup(c => c.ClubMembers.FindByCondition(It.IsAny<Expression<Func<ClubMembers, bool>>>())).Returns(
               new List<ClubMembers>{
                         new ClubMembers{
                            ID = 1,
                            IsApproved = true,
                            User = new User
                            {
                                LastName = "Andrii",
                                FirstName = "Ivanenko"
                            }
                         }
           }.AsQueryable());

            //action
            var controller = new ClubController(repository.Object, usermanager.Object, hostingEnvironment.Object);
            var result = controller.ChangeIsApprovedStatusFollowers(1, 1);
            // assert
            repository.Verify(r => r.ClubMembers.Update(It.IsAny<ClubMembers>()), Times.Once());

        }
    }
}
