﻿using System;
using System.Collections.Generic;
using Cynosura.Studio.Core.Requests.Fields.Models;

namespace Cynosura.Studio.Core.Requests.Entities.Models
{
    public class EntityModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string PluralName { get; set; }
        public string DisplayName { get; set; }
        public string PluralDisplayName { get; set; }
        public IList<FieldModel> Fields { get; set; }
        public Dictionary<string, object> Properties { get; set; }
        public bool IsAbstract { get; set; }
        public Guid? BaseEntityId { get; set; }
        public Entities.Models.EntityShortModel BaseEntity { get; set; }
    }
}
