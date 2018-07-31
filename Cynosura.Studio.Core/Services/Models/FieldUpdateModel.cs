using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Core.Services.Models
{
    public class FieldUpdateModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public FieldType Type { get; set; }
        public int? Size { get; set; }
        public bool IsRequired { get; set; }
    }
}