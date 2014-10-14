using System;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Reflection;
using System.Globalization;

using Common.Logging;

namespace X3Platform.Configuration
{
    /// <summary>Xml配置辅助操作类</summary>
    public static class XmlConfiguratonOperator
    {
        /// <summary>日志记录器</summary>
        private static readonly ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary></summary>
        /// <param name="list"></param>
        /// <param name="nodes"></param>
        public static void SetKeyValues(NameValueConfigurationCollection list, XmlNodeList nodes)
        {
            if (list == null) { throw new ArgumentNullException("list"); }

            foreach (XmlNode node in nodes)
            {
                bool isExist = false;

                NameValueConfigurationElement nameValue = new NameValueConfigurationElement(node.Attributes["name"].Value, node.Attributes["value"].Value);

                string[] keys = list.AllKeys;

                foreach (string key in keys)
                {
                    if (nameValue.Name == key)
                    {
                        list[nameValue.Name].Value = nameValue.Value;
                        isExist = true;
                        break;
                    }
                }

                if (!isExist)
                {
                    list.Add(nameValue);
                }
            }
        }

        #region 私有的常量

        // String constants used while parsing the XML data
        private const string CONFIGURATION_TAG = "logging";
        private const string RENDERER_TAG = "renderer";
        private const string APPENDER_TAG = "appender";
        private const string APPENDER_REF_TAG = "appender-ref";
        private const string PARAM_TAG = "param";

        // TODO: Deprecate use of category tags
        private const string CATEGORY_TAG = "category";
        // TODO: Deprecate use of priority tag
        private const string PRIORITY_TAG = "priority";

        private const string NAME_ATTR = "name";
        private const string TYPE_ATTR = "type";
        private const string VALUE_ATTR = "value";
        private const string LEVEL_TAG = "level";
        private const string REF_ATTR = "ref";
        private const string ADDITIVITY_ATTR = "additivity";
        private const string THRESHOLD_ATTR = "threshold";
        private const string CONFIG_DEBUG_ATTR = "configDebug";
        private const string INTERNAL_DEBUG_ATTR = "debug";
        private const string CONFIG_UPDATE_MODE_ATTR = "update";
        private const string RENDERING_TYPE_ATTR = "renderingClass";
        private const string RENDERED_TYPE_ATTR = "renderedClass";

        // flag used on the level element
        private const string INHERITED = "inherited";

        #endregion Private Constants

        /// <summary>
        /// Sets a parameter on an object.
        /// </summary>
        /// <param name="element">The parameter element.</param>
        /// <param name="target">The object to set the parameter on.</param>
        /// <remarks>
        /// The parameter name must correspond to a writable property
        /// on the object. The value of the parameter is a string,
        /// therefore this function will attempt to set a string
        /// property first. If unable to set a string property it
        /// will inspect the property and its argument type. It will
        /// attempt to call a static method called <c>Parse</c> on the
        /// type of the property. This method will take a single
        /// string argument and return a value that can be used to
        /// set the property.
        /// </remarks>
        public static void SetParameter(object target, XmlElement element)
        {
            // Get the property name
            string name = element.GetAttribute(NAME_ATTR);

            // If the name attribute does not exist then use the name of the element
            if (element.LocalName != PARAM_TAG || name == null || name.Length == 0)
            {
                name = element.LocalName;
            }

            // Look for the property on the target object
            Type targetType = target.GetType();
            Type propertyType = null;

            PropertyInfo propInfo = null;
            MethodInfo methInfo = null;

            // Try to find a writable property
            propInfo = targetType.GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.IgnoreCase);
            if (propInfo != null && propInfo.CanWrite)
            {
                // found a property
                propertyType = propInfo.PropertyType;
            }
            else
            {
                propInfo = null;

                // look for a method with the signature Add<property>(type)
                methInfo = FindMethodInfo(targetType, name);

                if (methInfo != null)
                {
                    propertyType = methInfo.GetParameters()[0].ParameterType;
                }
            }

            if (propertyType == null)
            {
                logger.Error("XmlHierarchyConfigurator: Cannot find Property [" + name + "] to set object on [" + target.ToString() + "]");
            }
            else
            {
                string propertyValue = null;

                if (element.GetAttributeNode(VALUE_ATTR) != null)
                {
                    propertyValue = element.GetAttribute(VALUE_ATTR);

                    if (!string.IsNullOrEmpty(propertyValue))
                    {
                        propertyValue = KernelConfigurationView.Instance.ReplaceKeyValue(propertyValue);
                    }
                }
                else if (element.HasChildNodes)
                {
                    // Concatenate the CDATA and Text nodes together
                    foreach (XmlNode childNode in element.ChildNodes)
                    {
                        if (childNode.NodeType == XmlNodeType.CDATA || childNode.NodeType == XmlNodeType.Text)
                        {
                            if (propertyValue == null)
                            {
                                propertyValue = childNode.InnerText;
                            }
                            else
                            {
                                propertyValue += childNode.InnerText;
                            }
                        }
                    }
                }

                if (propertyValue != null)
                {
                    try
                    {
                        // Expand environment variables in the string.
                        // propertyValue = OptionConverter.SubstituteVariables(propertyValue, Environment.GetEnvironmentVariables());
                    }
                    catch (System.Security.SecurityException)
                    {
                        // This security exception will occur if the caller does not have 
                        // unrestricted environment permission. If this occurs the expansion 
                        // will be skipped with the following warning message.
                        logger.Debug("XmlHierarchyConfigurator: Security exception while trying to expand environment variables. Error Ignored. No Expansion.");
                    }

                    Type parsedObjectConversionTargetType = null;

                    // Check if a specific subtype is specified on the element using the 'type' attribute
                    string subTypeString = element.GetAttribute(TYPE_ATTR);

                    if (subTypeString != null && subTypeString.Length > 0)
                    {
                        // Read the explicit subtype
                        try
                        {
                            Type subType = GetTypeFromString(subTypeString, true, true);

                            logger.Debug("XmlHierarchyConfigurator: Parameter [" + name + "] specified subtype [" + subType.FullName + "]");

                            if (!propertyType.IsAssignableFrom(subType))
                            {
                                // Check if there is an appropriate type converter
                                if (CanConvertTypeTo(subType, propertyType))
                                {
                                    // Must re-convert to the real property type
                                    parsedObjectConversionTargetType = propertyType;

                                    // Use sub type as intermediary type
                                    propertyType = subType;
                                }
                                else
                                {
                                    logger.Error("XmlHierarchyConfigurator: Subtype [" + subType.FullName + "] set on [" + name + "] is not a subclass of property type [" + propertyType.FullName + "] and there are no acceptable type conversions.");
                                }
                            }
                            else
                            {
                                // The subtype specified is found and is actually a subtype of the property
                                // type, therefore we can switch to using this type.
                                propertyType = subType;
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.Error("XmlHierarchyConfigurator: Failed to find type [" + subTypeString + "] set on [" + name + "]", ex);
                        }
                    }

                    // Now try to convert the string value to an acceptable type
                    // to pass to this property.

                    object convertedValue = ConvertStringTo(propertyType, propertyValue);

                    // Check if we need to do an additional conversion
                    if (convertedValue != null && parsedObjectConversionTargetType != null)
                    {
                        logger.Debug("XmlHierarchyConfigurator: Performing additional conversion of value from [" + convertedValue.GetType().Name + "] to [" + parsedObjectConversionTargetType.Name + "]");

                        convertedValue = XmlConfiguratonOperator.ConvertTypeTo(convertedValue, parsedObjectConversionTargetType);
                    }

                    if (convertedValue != null)
                    {
                        if (propInfo != null)
                        {
                            // Got a converted result
                            logger.Debug("XmlHierarchyConfigurator: Setting Property [" + propInfo.Name + "] to " + convertedValue.GetType().Name + " value [" + convertedValue.ToString() + "]");

                            try
                            {
                                // Pass to the property
                                propInfo.SetValue(target, convertedValue, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                            }
                            catch (TargetInvocationException targetInvocationEx)
                            {
                                logger.Error("XmlHierarchyConfigurator: Failed to set parameter [" + propInfo.Name + "] on object [" + target + "] using value [" + convertedValue + "]", targetInvocationEx.InnerException);
                            }
                        }
                        else if (methInfo != null)
                        {
                            // Got a converted result
                            logger.Debug("XmlHierarchyConfigurator: Setting Collection Property [" + methInfo.Name + "] to " + convertedValue.GetType().Name + " value [" + convertedValue.ToString() + "]");

                            try
                            {
                                // Pass to the property
                                methInfo.Invoke(target, BindingFlags.InvokeMethod, null, new object[] { convertedValue }, CultureInfo.InvariantCulture);
                            }
                            catch (TargetInvocationException targetInvocationEx)
                            {
                                logger.Error("XmlHierarchyConfigurator: Failed to set parameter [" + name + "] on object [" + target + "] using value [" + convertedValue + "]", targetInvocationEx.InnerException);
                            }
                        }
                    }
                    else
                    {
                        logger.Warn("XmlHierarchyConfigurator: Unable to set property [" + name + "] on object [" + target + "] using value [" + propertyValue + "] (with acceptable conversion types)");
                    }
                }
                else
                {
                    object createdObject = null;

                    if (propertyType == typeof(string) && !HasAttributesOrElements(element))
                    {
                        // If the property is a string and the element is empty (no attributes
                        // or child elements) then we special case the object value to an empty string.
                        // This is necessary because while the String is a class it does not have
                        // a default constructor that creates an empty string, which is the behavior
                        // we are trying to simulate and would be expected from CreateObjectFromXml
                        createdObject = "";
                    }
                    else
                    {
                        // No value specified
                        Type defaultObjectType = null;
                        if (IsTypeConstructible(propertyType))
                        {
                            defaultObjectType = propertyType;
                        }

                        createdObject = CreateObjectFromXml(element, defaultObjectType, propertyType);
                    }

                    if (createdObject == null)
                    {
                        logger.Error("XmlHierarchyConfigurator: Failed to create object to set param: " + name);
                    }
                    else
                    {
                        if (propInfo != null)
                        {
                            // Got a converted result
                            logger.Debug("XmlHierarchyConfigurator: Setting Property [" + propInfo.Name + "] to object [" + createdObject + "]");

                            try
                            {
                                // Pass to the property
                                propInfo.SetValue(target, createdObject, BindingFlags.SetProperty, null, null, CultureInfo.InvariantCulture);
                            }
                            catch (TargetInvocationException targetInvocationEx)
                            {
                                logger.Error("XmlHierarchyConfigurator: Failed to set parameter [" + propInfo.Name + "] on object [" + target + "] using value [" + createdObject + "]", targetInvocationEx.InnerException);
                            }
                        }
                        else if (methInfo != null)
                        {
                            // Got a converted result
                            logger.Debug("XmlHierarchyConfigurator: Setting Collection Property [" + methInfo.Name + "] to object [" + createdObject + "]");

                            try
                            {
                                // Pass to the property
                                methInfo.Invoke(target, BindingFlags.InvokeMethod, null, new object[] { createdObject }, CultureInfo.InvariantCulture);
                            }
                            catch (TargetInvocationException targetInvocationEx)
                            {
                                logger.Error("XmlHierarchyConfigurator: Failed to set parameter [" + methInfo.Name + "] on object [" + target + "] using value [" + createdObject + "]", targetInvocationEx.InnerException);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Test if an element has no attributes or child elements
        /// </summary>
        /// <param name="element">the element to inspect</param>
        /// <returns><c>true</c> if the element has any attributes or child elements, <c>false</c> otherwise</returns>
        public static bool HasAttributesOrElements(XmlElement element)
        {
            foreach (XmlNode node in element.ChildNodes)
            {
                if (node.NodeType == XmlNodeType.Attribute || node.NodeType == XmlNodeType.Element)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Test if a <see cref="Type"/> is constructible with <c>Activator.CreateInstance</c>.
        /// </summary>
        /// <param name="type">the type to inspect</param>
        /// <returns><c>true</c> if the type is creatable using a default constructor, <c>false</c> otherwise</returns>
        private static bool IsTypeConstructible(Type type)
        {
            if (type.IsClass && !type.IsAbstract)
            {
                ConstructorInfo defaultConstructor = type.GetConstructor(new Type[0]);
                if (defaultConstructor != null && !defaultConstructor.IsAbstract && !defaultConstructor.IsPrivate)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Look for a method on the <paramref name="targetType"/> that matches the <paramref name="name"/> supplied
        /// </summary>
        /// <param name="targetType">the type that has the method</param>
        /// <param name="name">the name of the method</param>
        /// <returns>the method info found</returns>
        /// <remarks>
        /// <para>
        /// The method must be a public instance method on the <paramref name="targetType"/>.
        /// The method must be named <paramref name="name"/> or "Add" followed by <paramref name="name"/>.
        /// The method must take a single parameter.
        /// </para>
        /// </remarks>
        private static MethodInfo FindMethodInfo(Type targetType, string name)
        {
            string requiredMethodNameA = name;
            string requiredMethodNameB = "Add" + name;

            MethodInfo[] methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (MethodInfo methInfo in methods)
            {
                if (!methInfo.IsStatic)
                {
                    if (string.Compare(methInfo.Name, requiredMethodNameA, true, System.Globalization.CultureInfo.InvariantCulture) == 0 ||
                        string.Compare(methInfo.Name, requiredMethodNameB, true, System.Globalization.CultureInfo.InvariantCulture) == 0)
                    {
                        // Found matching method name

                        // Look for version with one arg only
                        System.Reflection.ParameterInfo[] methParams = methInfo.GetParameters();
                        if (methParams.Length == 1)
                        {
                            return methInfo;
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Creates an object as specified in XML.
        /// </summary>
        /// <param name="element">The XML element that contains the definition of the object.</param>
        /// <param name="defaultTargetType">The object type to use if not explicitly specified.</param>
        /// <param name="typeConstraint">The type that the returned object must be or must inherit from.</param>
        /// <returns>The object or <c>null</c></returns>
        /// <remarks>
        /// <para>
        /// Parse an XML element and create an object instance based on the configuration
        /// data.
        /// </para>
        /// <para>
        /// The type of the instance may be specified in the XML. If not
        /// specified then the <paramref name="defaultTargetType"/> is used
        /// as the type. However the type is specified it must support the
        /// <paramref name="typeConstraint"/> type.
        /// </para>
        /// </remarks>
        private static object CreateObjectFromXml(XmlElement element, Type defaultTargetType, Type typeConstraint)
        {
            Type objectType = null;

            // Get the object type
            string objectTypeString = element.GetAttribute(TYPE_ATTR);
            if (objectTypeString == null || objectTypeString.Length == 0)
            {
                if (defaultTargetType == null)
                {
                    logger.Error("XmlHierarchyConfigurator: Object type not specified. Cannot create object of type [" + typeConstraint.FullName + "]. Missing Value or Type.");
                    return null;
                }
                else
                {
                    // Use the default object type
                    objectType = defaultTargetType;
                }
            }
            else
            {
                // Read the explicit object type
                try
                {
                    objectType = GetTypeFromString(objectTypeString, true, true);
                }
                catch (Exception ex)
                {
                    logger.Error("XmlHierarchyConfigurator: Failed to find type [" + objectTypeString + "]", ex);
                    return null;
                }
            }

            bool requiresConversion = false;

            // Got the object type. Check that it meets the typeConstraint
            if (typeConstraint != null)
            {
                if (!typeConstraint.IsAssignableFrom(objectType))
                {
                    // Check if there is an appropriate type converter
                    if (CanConvertTypeTo(objectType, typeConstraint))
                    {
                        requiresConversion = true;
                    }
                    else
                    {
                        logger.Error("XmlHierarchyConfigurator: Object type [" + objectType.FullName + "] is not assignable to type [" + typeConstraint.FullName + "]. There are no acceptable type conversions.");
                        return null;
                    }
                }
            }

            // Create using the default constructor
            object createdObject = null;
            try
            {
                createdObject = Activator.CreateInstance(objectType);
            }
            catch (Exception createInstanceEx)
            {
                logger.Error("XmlHierarchyConfigurator: Failed to construct object of type [" + objectType.FullName + "] Exception: " + createInstanceEx.ToString());
            }

            // Set any params on object
            foreach (XmlNode currentNode in element.ChildNodes)
            {
                if (currentNode.NodeType == XmlNodeType.Element)
                {
                    SetParameter(createdObject, (XmlElement)currentNode);
                }
            }

            // Check if we need to call ActivateOptions
            //IOptionHandler optionHandler = createdObject as IOptionHandler;
            //if (optionHandler != null)
            //{
            //    optionHandler.ActivateOptions();
            //}

            // Ok object should be initialized

            if (requiresConversion)
            {
                // Convert the object type
                // return OptionConverter.ConvertTypeTo(createdObject, typeConstraint);
                return null;
            }
            else
            {
                // The object is of the correct type
                return createdObject;
            }
        }

        /// <summary>
        /// Checks if there is an appropriate type conversion from the source type to the target type.
        /// </summary>
        /// <param name="sourceType">The type to convert from.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <returns><c>true</c> if there is a conversion from the source type to the target type.</returns>
        /// <remarks>
        /// Checks if there is an appropriate type conversion from the source type to the target type.
        /// <para>
        /// </para>
        /// </remarks>
        public static bool CanConvertTypeTo(Type sourceType, Type targetType)
        {
            if (sourceType == null || targetType == null)
            {
                return false;
            }

            // Check if we can assign directly from the source type to the target type
            if (targetType.IsAssignableFrom(sourceType))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Loads the type specified in the type string.
        /// </summary>
        /// <param name="relativeType">A sibling type to use to load the type.</param>
        /// <param name="typeName">The name of the type to load.</param>
        /// <param name="throwOnError">Flag set to <c>true</c> to throw an exception if the type cannot be loaded.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore the case of the type name; otherwise, <c>false</c></param>
        /// <returns>The type loaded or <c>null</c> if it could not be loaded.</returns>
        /// <remarks>
        /// <para>
        /// If the type name is fully qualified, i.e. if contains an assembly name in 
        /// the type name, the type will be loaded from the system using 
        /// <see cref="Type.GetType(string,bool)"/>.
        /// </para>
        /// <para>
        /// If the type name is not fully qualified, it will be loaded from the assembly
        /// containing the specified relative type. If the type is not found in the assembly 
        /// then all the loaded assemblies will be searched for the type.
        /// </para>
        /// </remarks>
        public static Type GetTypeFromString(Type relativeType, string typeName, bool throwOnError, bool ignoreCase)
        {
            return GetTypeFromString(relativeType.Assembly, typeName, throwOnError, ignoreCase);
        }

        /// <summary>
        /// Loads the type specified in the type string.
        /// </summary>
        /// <param name="typeName">The name of the type to load.</param>
        /// <param name="throwOnError">Flag set to <c>true</c> to throw an exception if the type cannot be loaded.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore the case of the type name; otherwise, <c>false</c></param>
        /// <returns>The type loaded or <c>null</c> if it could not be loaded.</returns>		
        /// <remarks>
        /// <para>
        /// If the type name is fully qualified, i.e. if contains an assembly name in 
        /// the type name, the type will be loaded from the system using 
        /// <see cref="Type.GetType(string,bool)"/>.
        /// </para>
        /// <para>
        /// If the type name is not fully qualified it will be loaded from the
        /// assembly that is directly calling this method. If the type is not found 
        /// in the assembly then all the loaded assemblies will be searched for the type.
        /// </para>
        /// </remarks>
        public static Type GetTypeFromString(string typeName, bool throwOnError, bool ignoreCase)
        {
            return GetTypeFromString(Assembly.GetCallingAssembly(), typeName, throwOnError, ignoreCase);
        }

        /// <summary>
        /// Loads the type specified in the type string.
        /// </summary>
        /// <param name="relativeAssembly">An assembly to load the type from.</param>
        /// <param name="typeName">The name of the type to load.</param>
        /// <param name="throwOnError">Flag set to <c>true</c> to throw an exception if the type cannot be loaded.</param>
        /// <param name="ignoreCase"><c>true</c> to ignore the case of the type name; otherwise, <c>false</c></param>
        /// <returns>The type loaded or <c>null</c> if it could not be loaded.</returns>
        /// <remarks>
        /// <para>
        /// If the type name is fully qualified, i.e. if contains an assembly name in 
        /// the type name, the type will be loaded from the system using 
        /// <see cref="Type.GetType(string,bool)"/>.
        /// </para>
        /// <para>
        /// If the type name is not fully qualified it will be loaded from the specified
        /// assembly. If the type is not found in the assembly then all the loaded assemblies 
        /// will be searched for the type.
        /// </para>
        /// </remarks>
        public static Type GetTypeFromString(Assembly relativeAssembly, string typeName, bool throwOnError, bool ignoreCase)
        {
            // Check if the type name specifies the assembly name
            if (typeName.IndexOf(',') == -1)
            {
                //LogLog.Debug("SystemInfo: Loading type ["+typeName+"] from assembly ["+relativeAssembly.FullName+"]");

                // Attempt to lookup the type from the relativeAssembly
                Type type = relativeAssembly.GetType(typeName, false, ignoreCase);
                if (type != null)
                {
                    // Found type in relative assembly
                    //LogLog.Debug("SystemInfo: Loaded type ["+typeName+"] from assembly ["+relativeAssembly.FullName+"]");
                    return type;
                }

                Assembly[] loadedAssemblies = null;
                try
                {
                    loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                }
                catch (System.Security.SecurityException)
                {
                    // Insufficient permissions to get the list of loaded assemblies
                }

                if (loadedAssemblies != null)
                {
                    // Search the loaded assemblies for the type
                    foreach (Assembly assembly in loadedAssemblies)
                    {
                        type = assembly.GetType(typeName, false, ignoreCase);
                        if (type != null)
                        {
                            // Found type in loaded assembly
                            //logger.Debug("SystemInfo: Loaded type [" + typeName + "] from assembly [" + assembly.FullName + "] by searching loaded assemblies.");
                            return type;
                        }
                    }
                }

                // Didn't find the type
                if (throwOnError)
                {
                    throw new TypeLoadException("Could not load type [" + typeName + "]. Tried assembly [" + relativeAssembly.FullName + "] and all loaded assemblies");
                }

                return null;
            }
            else
            {
                // Includes explicit assembly name
                //LogLog.Debug("SystemInfo: Loading type ["+typeName+"] from global Type");

                return Type.GetType(typeName, throwOnError, ignoreCase);
            }
        }

        /// <summary>
        /// Converts a string to an object.
        /// </summary>
        /// <param name="target">The target type to convert to.</param>
        /// <param name="txt">The string to convert to an object.</param>
        /// <returns>
        /// The object converted from a string or <c>null</c> when the 
        /// conversion failed.
        /// </returns>
        /// <remarks>
        /// <para>
        /// Converts a string to an object. Uses the converter registry to try
        /// to convert the string value into the specified target type.
        /// </para>
        /// </remarks>
        public static object ConvertStringTo(Type target, string value)
        {
            if (target == null)
            {
                throw new ArgumentNullException("target");
            }

            // If we want a string we already have the correct type
            if (typeof(string) == target || typeof(object) == target)
            {
                return value;
            }

            // First lets try to find a type converter

            if (target.IsEnum)
            {
                // Target type is an enum.

                // Use the Enum.Parse(EnumType, string) method to get the enum value
                return Enum.Parse(target, value, true);
            }
            else
            {
                // We essentially make a guess that to convert from a string
                // to an arbitrary type T there will be a static method defined on type T called Parse
                // that will take an argument of type string. i.e. T.Parse(string)->T we call this
                // method to convert the string to the type required by the property.
                MethodInfo method = target.GetMethod("Parse", new Type[] { typeof(string) });

                if (method != null)
                {
                    // Call the Parse method
                    return method.Invoke(null, BindingFlags.InvokeMethod, null, new object[] { value }, CultureInfo.InvariantCulture);
                }
                else
                {
                    // No Parse() method found.
                }
            }

            return null;
        }

        /// <summary>
        /// Converts an object to the target type.
        /// </summary>
        /// <param name="sourceInstance">The object to convert to the target type.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <returns>The converted object.</returns>
        /// <remarks>
        /// <para>
        /// Converts an object to the target type.
        /// </para>
        /// </remarks>
        public static object ConvertTypeTo(object sourceInstance, Type targetType)
        {
            Type sourceType = sourceInstance.GetType();

            // Check if we can assign directly from the source type to the target type
            if (targetType.IsAssignableFrom(sourceType))
            {
                return sourceInstance;
            }

            throw new ArgumentException("Cannot convert source object [" + sourceInstance.ToString() + "] to target type [" + targetType.Name + "]", "sourceInstance");
        }
    }
}
