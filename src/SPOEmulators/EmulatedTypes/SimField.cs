namespace SPOEmulators.EmulatedTypes
{
    using System;
    using Microsoft.QualityTools.Testing.Fakes.Instances;
    using Microsoft.QualityTools.Testing.Fakes.Shims;
    using Microsoft.SharePoint.Client;
    using Microsoft.SharePoint.Client.Fakes;

    internal class SimField : Isolator<Field, ShimField>
    {

        public string InternalName { get; set; }

        public bool CanBeDeleted { get; set; }

        public string DefaultValue { get; set; }

        public string Description { get; set; }

        public string Direction { get; set; }

        public bool EnforceUniqueValues { get; set; }

        public string EntityPropertyName { get; set; }

        public FieldType FieldTypeKind { get; set; }

        public SimFieldCollection Fields { get; set; }

        public bool Filterable { get; set; }

        public bool FromBaseType { get; set; }

        public string Group { get; set; }

        public bool Hidden { get; set; }

        public Guid Id { get; set; }

        public bool Indexed { get; set; }

        public string JSLink { get; set; }

        public bool ReadOnlyField { get; set; }

        public bool Required { get; set; }

        public string SchemaXml { get; set; }

        public string Scope { get; set; }

        public bool Sealed { get; set; }

        public bool ShowInDisplayForm { get; set; }

        public bool ShowInEditForm { get; set; }

        public object ShowInNewForm { get; set; }

        public bool Sortable { get; set; }

        public string Title { get; set; }

        public string ValidationFormula { get; set; }

        public string ValidationMessage { get; set; }

#if !CLIENTSDKV15
        public bool AutoIndexed { get; set; }

        public UserResource DescriptionResource { get; set; }

        public UserResource TitleResource { get; set; }
#endif

        public SimField()
            : this(ShimRuntime.CreateUninitializedInstance<Field>())
        {
        }

        public SimField(Field instance)
            : base(instance)
        {
            this.Description = string.Empty;
            this.Fake.CanBeDeletedGet = () => this.CanBeDeleted;
            this.Fake.DefaultValueGet = () => this.DefaultValue;
            this.Fake.DefaultValueSetString = (s) => this.DefaultValue = s;
            this.Fake.DeleteObject = () => this.Delete();
            this.Fake.DescriptionGet = () => this.Description;
            this.Fake.DescriptionSetString = (s) => this.Description = s;
            this.Fake.DirectionGet = () => this.Direction;
            this.Fake.DirectionSetString = (s) => this.Direction = s;
            this.Fake.EnforceUniqueValuesGet = () => this.EnforceUniqueValues;
            this.Fake.EnforceUniqueValuesSetBoolean = (b) => this.EnforceUniqueValues = b;
            this.Fake.EntityPropertyNameGet = () => this.EntityPropertyName;
            this.Fake.FieldTypeKindGet = () => this.FieldTypeKind;
            this.Fake.FieldTypeKindSetFieldType = (f) => this.FieldTypeKind = f;
            this.Fake.FilterableGet = () => Filterable;
            this.Fake.FromBaseTypeGet = () => FromBaseType;
            this.Fake.GroupGet = () => this.Group;
            this.Fake.GroupSetString = (s) => this.Group = s;
            this.Fake.HiddenGet = () => this.Hidden;
            this.Fake.HiddenSetBoolean = (b) => this.Hidden = b;
            this.Fake.IdGet = () => this.Id;
            this.Fake.IndexedGet = () => this.Indexed;
            this.Fake.IndexedSetBoolean = (b) => this.Indexed = b;
            this.Fake.InternalNameGet = () => this.InternalName;
            this.Fake.JSLinkGet = () => this.JSLink;
            this.Fake.JSLinkSetString = (s) => this.JSLink = s;
            this.Fake.ReadOnlyFieldGet = () => this.ReadOnlyField;
            this.Fake.ReadOnlyFieldSetBoolean = (b) => this.ReadOnlyField = b;
            this.Fake.RequiredGet = () => this.Required;
            this.Fake.RequiredSetBoolean = (b) => this.Required = b;
            this.Fake.SchemaXmlGet = () => this.SchemaXml;
            this.Fake.SchemaXmlSetString = (s) => this.SchemaXml = s;
            this.Fake.SchemaXmlWithResourceTokensGet = () => this.SchemaXml;
            this.Fake.ScopeGet = () => this.Scope;
            this.Fake.SealedGet = () => this.Sealed;
            this.Fake.SetShowInDisplayFormBoolean = (b) => this.ShowInDisplayForm = b;
            this.Fake.SetShowInEditFormBoolean = (b) => this.ShowInEditForm = b;
            this.Fake.SetShowInNewFormBoolean = (b) => this.ShowInNewForm = b;
            this.Fake.SortableGet = () => this.Sortable;
            this.Fake.StaticNameGet = () => this.InternalName;
            this.Fake.StaticNameSetString = (s) => this.InternalName = s;
            this.Fake.TitleGet = () => this.Title;
            this.Fake.TitleSetString = (s) => this.Title = s;
            this.Fake.ValidationFormulaGet = () => this.ValidationFormula;
            this.Fake.ValidationFormulaSetString = (s) => this.ValidationFormula = s;
            this.Fake.ValidationMessageGet = () => this.ValidationMessage;
            this.Fake.ValidationMessageSetString = (s) => this.ValidationMessage = s;

#if !CLIENTSDKV15
            this.Fake.AutoIndexedGet = () => this.AutoIndexed;
            this.Fake.DescriptionResourceGet = () => this.DescriptionResource;
            this.Fake.TitleResourceGet = () => this.TitleResource;
#endif

            this.Fake.Update = () => { };
            this.Fake.UpdateAndPushChangesBoolean = (b) => { };
        }

        public void Delete()
        {
            Fields.Remove(this.Instance);
        }

        public static SimField FromInstance(Field instance)
        {
            return InstancedPool.CastAsInstanced<Field, SimField>(instance);
        }

        public static void Initialize()
        {
            ShimField.BehaveAsNotImplemented();
        }
    }
}
