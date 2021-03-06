using Microsoft.Practices.Unity.Configuration.ConfigurationHelpers;
using System.Configuration;

namespace mmSquare.Betamax.Unity.Configuration
{
    [ConfigurationCollection(typeof(InterestingInterfaceElement))]
    public class InterestingInterfaceElementCollection : DeserializableConfigurationElementCollection<InterestingInterfaceElement>
    {
        public new InterestingInterfaceElement this[string policyName]
        {
            get
            {
                return (InterestingInterfaceElement)BaseGet(policyName);
            }
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((InterestingInterfaceElement)element).Name;
        }
    }
}