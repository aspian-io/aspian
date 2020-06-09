using System;
using Aspian.Application.Core.UserServices.DTOs;

namespace Aspian.Application.Core.OptionServices.DTOs
{
    public class OptionmetaDto
    {
        public string PublicKeyName { get; set; }
        public string Key { get; set; }
        public string KeyDescription { get; set; }
        public string Value { get; set; }
        public string ValueDescription { get; set; }
        public string DefaultValue { get; set; }
        public string DefaultValueDescription { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string ModifiedById { get; set; }
        public UserDto ModifiedBy { get; set; }
        public string UserAgent { get; set; }
        public string UserIPAddress { get; set; }
    }
}