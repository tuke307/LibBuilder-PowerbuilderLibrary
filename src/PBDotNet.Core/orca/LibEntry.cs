// project=PBDotNet.Core, file=LibEntry.cs, create=09:16 Copyright (c) 2021 tuke
// productions. All rights reserved.
using System;

namespace PBDotNet.Core.orca
{
    /// <summary>
    /// entry in orca/pb library
    /// </summary>
    public class LibEntry : ILibEntry
    {
        #region private

        private string comment;
        private DateTime createTime;
        private string library;
        private string name;
        private Orca.Version pbVersion;
        private int size;
        private string source;
        private Objecttype type;

        #endregion private

        #region properties

        public string Comment
        {
            get
            {
                return comment;
            }
        }

        public DateTime Createtime
        {
            get
            {
                return createTime;
            }
        }

        public string Library
        {
            get
            {
                return library;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public int Size
        {
            get
            {
                return size;
            }
        }

        public string Source
        {
            get
            {
                if (String.IsNullOrEmpty(this.source))
                {
                    new Orca(this.pbVersion).FillCode(this);
                }

                return source;
            }
            set
            {
                source = value;
            }
        }

        public Objecttype Type
        {
            get
            {
                return type;
            }
        }

        #endregion properties

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="name">name of the entry</param>
        /// <param name="type">enum type of the object</param>
        /// <param name="createTime">creation datetime</param>
        /// <param name="size">size of object</param>
        /// <param name="library">path to library</param>
        /// <param name="comment">comment of entry</param>
        public LibEntry(string name, Objecttype type, DateTime createTime, int size, string library, orca.Orca.Version pbVersion, string comment = "")
        {
            this.name = name;
            this.type = type;
            this.createTime = createTime;
            this.size = size;
            this.comment = comment;
            this.library = library;
            this.pbVersion = pbVersion;
        }
    }
}