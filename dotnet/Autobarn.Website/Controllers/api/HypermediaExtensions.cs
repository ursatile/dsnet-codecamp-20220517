﻿using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Newtonsoft.Json;

namespace Autobarn.Website.Controllers.api {
    public static class HypermediaExtensions {
        public static dynamic ToDynamic(this object thing) {
            IDictionary<string, object> expando = new ExpandoObject();
            var properties = TypeDescriptor.GetProperties(thing.GetType());
            foreach (PropertyDescriptor property in properties) {
                if (Ignore(property)) continue;
                expando.Add(property.Name, property.GetValue(thing));
            }
            return (ExpandoObject)expando;
        }

        private static bool Ignore(PropertyDescriptor property) {
            return property.Attributes.OfType<JsonIgnoreAttribute>().Any();
        }
    }
}
