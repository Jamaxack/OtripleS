﻿using System;

namespace OtripleS.Web.Api.Models.Classrooms
{
    public class Classroom : IAuditable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public ClassroomStatus Status { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public DateTimeOffset UpdatedDate { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
    }
}
