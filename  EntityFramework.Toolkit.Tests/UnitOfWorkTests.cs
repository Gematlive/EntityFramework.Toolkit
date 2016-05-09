﻿using System;
using System.Data.Extensions;
using System.Data.Extensions.Exceptions;

using EntityFramework.Toolkit.Tests.Stubs;

using FluentAssertions;

using Moq;

using Xunit;

namespace EntityFramework.Toolkit.Tests
{
    public class UnitOfWorkTests
    {
        [Fact]
        public void ShouldCommitToSingleContext()
        {
            // Arrange
            var unitOfWork = new UnitOfWork();
            var sampleContextMock = new Mock<ISampleContext>();

            unitOfWork.RegisterContext(sampleContextMock.Object);

            // Act
            unitOfWork.Commit();

            // Assert
            sampleContextMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        ////[Fact]
        ////public void ShouldCommitToSingleContextReal()
        ////{
        ////    // Arrange
        ////    var unitOfWork = new UnitOfWork();
        ////    var sampleContextMock = new Mock<ISampleContext>();
        ////    sampleContextMock.Setup(m => m.SaveChanges()).Throws(new InvalidOperationException("SampleContextOne failed to SaveChanges."));
        ////    var sampleContext = new SampleContext();
        ////    sampleContext.SaveCalled += (sender, args) => { };

        ////    unitOfWork.RegisterContext(sampleContext);
        ////    unitOfWork.RegisterContext(sampleContextMock.Object);

        ////    sampleContext.Employees.Add(new Employee());
        ////    sampleContext.Employees.Add(new Employee());
        ////    sampleContext.Employees.Add(new Employee());
        ////    sampleContext.Departments.Add(new Department());
        ////    sampleContext.Departments.Add(new Department());

        ////    // Act
        ////    Action action = () => unitOfWork.Commit();


        ////    // Assert
        ////    action.ShouldThrow<UnitOfWorkException>().WithInnerException<InvalidOperationException>();
        ////    sampleContext.Employees.Should().HaveCount(0);
        ////    sampleContext.Departments.Should().HaveCount(0);
        ////}

        [Fact]
        public void ShouldCommitToMultipleContexts()
        {
            // Arrange
            var unitOfWork = new UnitOfWork();
            var sampleContextOneMock = new Mock<ISampleContext>();
            var sampleContextTwoMock = new Mock<ISampleContextTwo>();

            unitOfWork.RegisterContext(sampleContextOneMock.Object);
            unitOfWork.RegisterContext(sampleContextTwoMock.Object);

            // Act
            unitOfWork.Commit();

            // Assert
            sampleContextOneMock.Verify(x => x.SaveChanges(), Times.Once);
            sampleContextTwoMock.Verify(x => x.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ShouldFailToCommitMultipleContexts()
        {
            // Arrange
            var unitOfWork = new UnitOfWork();
            var sampleContextOneMock = new Mock<ISampleContext>();
            var sampleContextTwoMock = new Mock<ISampleContextTwo>();
            sampleContextTwoMock.Setup(m => m.SaveChanges()).Throws(new InvalidOperationException("SampleContextOne failed to SaveChanges."));

            unitOfWork.RegisterContext(sampleContextOneMock.Object);
            unitOfWork.RegisterContext(sampleContextTwoMock.Object);

            // Act
            Action action = () => unitOfWork.Commit();

            // Assert
            action.ShouldThrow<UnitOfWorkException>().WithInnerException<InvalidOperationException>();
            sampleContextOneMock.Verify(x => x.SaveChanges(), Times.Once);
            sampleContextTwoMock.Verify(x => x.SaveChanges(), Times.Once);
        }
    }
}
