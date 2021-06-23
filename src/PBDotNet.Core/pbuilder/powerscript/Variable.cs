// project=PBDotNet.Core, file=Variable.cs, create=09:16 Copyright (c) 2021 Timeline
// Financials GmbH & Co. KG. All rights reserved.
namespace PBDotNet.Core.pbuilder.powerscript
{
    /// <summary>
    /// variable in pb
    /// </summary>
    public class Variable
    {
        #region private

        private string datatype;
        private string descriptor;
        private string name;
        private string value;

        #endregion private

        #region properties

        public string Datatype
        {
            get
            {
                return datatype;
            }
        }

        public string Descriptor
        {
            get
            {
                return descriptor;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Value
        {
            get
            {
                return value;
            }
        }

        #endregion properties

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="datatype">datatype of the variable</param>
        /// <param name="name">name of the variable</param>
        /// <param name="value">value of the variable (optional)</param>
        /// <param name="descriptor">descriptor of the variable (optional)</param>
        public Variable(string datatype, string name, string value = "", string descriptor = "")
        {
            this.name = name;
            this.datatype = datatype;
            this.value = value;
            this.descriptor = descriptor;
        }
    }
}