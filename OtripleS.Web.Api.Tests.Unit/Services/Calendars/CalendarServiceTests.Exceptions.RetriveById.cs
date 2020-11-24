﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
{
    public partial class CalendarServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnRetrieveWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            Guid randomCalendarId = Guid.NewGuid();
            Guid inputCalendarId = randomCalendarId;
            var sqlException = GetSqlException();

            var expectedCalendarDependencyException =
                new CalendarDependencyException(sqlException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<Calendar> retrieveCalendarByIdTask =
                this.calendarService.RetrieveCalendarByIdAsync(inputCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarDependencyException>(() =>
                retrieveCalendarByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedCalendarDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}