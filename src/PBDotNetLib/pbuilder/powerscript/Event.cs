// project=PBDotNetLib, file=Event.cs, creation=2020:6:28 Copyright (c) 2020 Timeline
// Financials GmbH & Co. KG. All rights reserved.
namespace PBDotNetLib.pbuilder.powerscript
{
    /// <summary>
    /// event in pb
    /// </summary>
    public class Event
    {
        #region private

        private bool extended = false;
        private string name;
        private Parameter[] parameter;
        private string returntype;
        private string source;

        #endregion private

        #region properties

        public bool Extended
        {
            get
            {
                return extended;
            }
            set
            {
                extended = value;
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
        /// <param name="name">event name</param>
        /// <param name="returntype">return type of the event</param>
        /// <param name="parameter">parameter list of the event</param>
        /// <param name="source">source of the event</param>
        /// <param name="extended">flag if the event is extended</param>
        public Event(string name, string returntype, Parameter[] parameter, string source = "", bool extended = false)
        {
            this.name = name;
            this.parameter = parameter;
            this.returntype = returntype;
            this.source = source;
            this.extended = extended;
        }
    }
}