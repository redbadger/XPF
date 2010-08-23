namespace RedBadger.Xpf.Presentation
{
    using System;
    using System.Windows;

    public class XpfDependencyProperty
    {
        private XpfDependencyProperty(DependencyProperty dependencyProperty, string name, Type propertyType)
        {
            this.Value = dependencyProperty;
            this.Name = name;
            this.PropertyType = propertyType;
        }

        public string Name { get; private set; }

        public Type PropertyType { get; private set; }

        public DependencyProperty Value { get; private set; }

        public static XpfDependencyProperty Register(
            string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return new XpfDependencyProperty(
                DependencyProperty.Register(name, propertyType, ownerType, typeMetadata), name, propertyType);
        }

        public static XpfDependencyProperty RegisterAttached(
            string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return
                new XpfDependencyProperty(
                    DependencyProperty.RegisterAttached(name, propertyType, ownerType, typeMetadata), name, propertyType);
        }
    }
}