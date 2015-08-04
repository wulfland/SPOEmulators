namespace SPOEmulators.EmulatedTypes
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Xml;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimFieldCollection : CollectionIsolator<Field, FieldCollection, ShimFieldCollection>
    {
        private Web _web;

        public List List { get; set; }

        public Web Web
        {
            get
            {
                Web result;
                if (_web != null)
                {
                    result = _web;
                }
                else
                {
                    if (this.List != null)
                    {
                        result = List.ParentWeb;
                    }
                    else
                    {
                        result = _web;
                    }
                }
                return result;
            }
            set
            {
                _web = value;
            }
        }

        public SimFieldCollection()
            : this(null)
        {
        }

        public SimFieldCollection(FieldCollection instance)
            : base(instance)
        {
            //this.Fake.Bind(this);

            this.Fake.AddFieldAsXmlStringBooleanAddFieldOptions = (string schema, bool addToView, AddFieldOptions options) =>
            {
                var field = this.CreateField();
                using (var xmlReader = XmlReader.Create(new StringReader(schema)))
                {
                    xmlReader.ReadToFollowing("Field");
                    xmlReader.MoveToAttribute("Name");
                    var name = xmlReader.Value;
                    xmlReader.MoveToAttribute("DisplayName");
                    var title = xmlReader.Value;

                    field.InternalName = string.IsNullOrEmpty(name) ? title : name;
                    field.Title = string.IsNullOrEmpty(title) ? name : title;
                }

                base.Add(field.Instance);

                return field.Instance;
            };

            this.Fake.AddField = (f) =>
            {
                base.Add(f);
                return f;
            };

            this.Fake.AddDependentLookupStringFieldString = (string displayName, Field primaryLookup, string lookupField) =>
            {
                var field = CreateField();
                field.InternalName = lookupField;
                field.Title = lookupField;

                return field.Instance;
            };

            this.Fake.GetByIdGuid = (id) => Get(f => f.Id == id);
            this.Fake.GetByInternalNameOrTitleString = (s) => Get(f => f.Title == s || f.InternalName == s);
            this.Fake.GetByTitleString = (s) => Get(f => f.Title == s);
        }

        private SimField CreateField()
        {
            return new SimField
            {
                Fields = this
            };
        }

        private Field Get(Func<Field, bool> predicate)
        {
            return this.First(predicate);
        }
    }
}
