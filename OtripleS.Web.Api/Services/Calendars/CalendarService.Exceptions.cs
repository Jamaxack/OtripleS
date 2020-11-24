﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;

namespace OtripleS.Web.Api.Services.Calendars
{
    public partial class CalendarService
    {
        private delegate ValueTask<Calendar> ReturningCalendarFunction();

        private async ValueTask<Calendar> TryCatch(ReturningCalendarFunction returningCalendarFunction)
        {
            try
            {
                return await returningCalendarFunction();
            }
            catch (InvalidCalendarInputException invalidCalendarInputException)
            {
                throw CreateAndLogValidationException(invalidCalendarInputException);
            }
            catch (NotFoundCalendarException nullCalendarException)
            {
                throw CreateAndLogValidationException(nullCalendarException);
            }
        }

        private CalendarValidationException CreateAndLogValidationException(Exception exception)
        {
            var CalendarValidationException = new CalendarValidationException(exception);
            this.loggingBroker.LogError(CalendarValidationException);

            return CalendarValidationException;
        }
    }
}