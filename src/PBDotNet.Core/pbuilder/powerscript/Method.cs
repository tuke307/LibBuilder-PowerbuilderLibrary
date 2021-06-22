// project=PBDotNet.Core, file=Method.cs, create=09:16 Copyright (c) 2021 tuke
// productions. All rights reserved.
namespace PBDotNet.Core.pbuilder.powerscript
{
    /// <summary>
    /// method (subroutine or function) in pb
    /// </summary>
    public class Method
    {
        #region private

        private string modifier;
        private string name;
        private Parameter[] parameter;
        private string returntype;
        private string source;

        #endregion private

        #region properties

        public string Modifier
        {
            get
            {
                return modifier;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Parameter[] Parameter
        {
            get
            {
                return parameter;
            }
        }

        public string Returntype
        {
            get
            {
                return returntype;
            }
        }

        public string Source
        {
            get
            {
                return source;
            }
        }

        #endregion properties

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="modifier">modifier (private, protected, public, ... )</param>
        /// <param name="name">name of the method</param>
        /// <param name="returntype">return type of the method</param>
        /// <param name="parameter">parameter list of the method</param>
        /// <param name="source">source of the method</param>
        public Method(string modifier, string name, string returntype, Parameter[] parameter, string source = "")
        {
            this.name = name;
            this.parameter = parameter;
            this.returntype = returntype;
            this.source = source;
            this.modifier = modifier;
        }
    }
}