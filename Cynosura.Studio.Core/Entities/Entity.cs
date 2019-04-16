﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Cynosura.Studio.Core.Entities
{
    public class Entity : BaseEntity
    {
        public Guid Id { get; set; }

		[Required()]
		[StringLength(100)]
		public string Name { get; set; }

		[Required()]
		[StringLength(100)]
		public string PluralName { get; set; }

		[Required()]
		[StringLength(100)]
		public string DisplayName { get; set; }

		[Required()]
		[StringLength(100)]
		public string PluralDisplayName { get; set; }

        public IList<Field> Fields { get; set; }

        public Dictionary<string, object> Properties { get; set; }
    }
}
