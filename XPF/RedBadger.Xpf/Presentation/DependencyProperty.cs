namespace RedBadger.Xpf.Presentation
{
    using System;

    public class DependencyProperty
    {
        private DependencyProperty(string name, Type propertyType)
        {
            this.Name = name;
            this.PropertyType = propertyType;
        }

        public string Name { get; private set; }

        public Type PropertyType { get; private set; }

        public static DependencyProperty Register(
            string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return new DependencyProperty(name, propertyType);
        }

        public static DependencyProperty RegisterAttached(
            string name, Type propertyType, Type ownerType, PropertyMetadata typeMetadata)
        {
            return new DependencyProperty(name, propertyType);
        }
    }
}