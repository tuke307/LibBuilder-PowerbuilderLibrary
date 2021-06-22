// project=PBDotNet.Core, file=Orca.cs, create=09:16 Copyright (c) 2021 tuke productions.
// All rights reserved.
namespace PBDotNet.Core.orca
{
    using PBDotNet.Core.util;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Runtime.InteropServices;
    using System.Text;

    [AttributeUsage(AttributeTargets.Field)]
    public class DllNameAttribute : Attribute
    {
        public string DllName { get; private set; }

        public DllNameAttribute(string dllName)
        {
            this.DllName = dllName;
        }
    }

    /// <summary>
    /// wrapper to export objects from pbl with orca
    /// </summary>
    public class Orca : IDisposable
    {
        /// <summary>
        /// Rebuild Type.
        /// </summary>
        public enum PBORCA_REBLD_TYPE
        {
            PBORCA_FULL_REBUILD = 1,
            PBORCA_INCREMENTAL_REBUILD = 2,
            PBORCA_MIGRATE = 3,
            PBORCA_3PASS = 4
        }

        /// <summary>
        /// Result.
        /// </summary>
        public enum Result
        {
            [Description("Operation successful")]
            PBORCA_OK = 0,

            [Description("Invalid parameter list")]
            PBORCA_INVALIDPARMS = -1,

            [Description("Duplicate operation")]
            PBORCA_DUPOPERATION = -2,

            [Description("Object not found")]
            PBORCA_OBJNOTFOUND = -3,

            [Description("Bad library name")]
            PBORCA_BADLIBRARY = -4,

            [Description("Library list not set")]
            PBORCA_LIBLISTNOTSET = -5,

            [Description("Library not in library list")]
            PBORCA_LIBNOTINLIST = -6,

            [Description("Library I/O error")]
            PBORCA_LIBIOERROR = -7,

            [Description("Object exists")]
            PBORCA_OBJEXISTS = -8,

            [Description("Invalid name")]
            PBORCA_INVALIDNAME = -9,

            [Description("Buffer size is too small")]
            PBORCA_BUFFERTOOSMALL = -10,

            [Description("Compile error")]
            PBORCA_COMPERROR = -11,

            [Description("Link error")]
            PBORCA_LINKERROR = -12,

            [Description("Current application not set")]
            PBORCA_CURRAPPLNOTSET = -13,

            [Description("Object has no ancestors")]
            PBORCA_OBJHASNOANCS = -14,

            [Description("Object has no references")]
            PBORCA_OBJHASNOREFS = -15,

            [Description("Invalid # of PBDs")]
            PBORCA_PBDCOUNTERROR = -16,

            [Description("PBD create error")]
            PBORCA_PBDCREATERROR = -17,

            [Description("Source Management error (obsolete)")]
            PBORCA_CHECKOUTERROR = -18,

            [Description("Could not instantiate ComponentBuilder class")]
            PBORCA_CBCREATEERROR = -19,

            [Description("Component builder Init method failed")]
            PBORCA_CBINITERROR = -20,

            [Description("Component builder BuildProject method failed")]
            PBORCA_CBBUILDERROR = -21,

            [Description("Could not connect to source control")]
            PBORCA_SCCFAILURE = -22,

            [Description("Could not read registry")]
            PBORCA_REGREADERROR = -23,

            [Description("Could not load DLL")]
            PBORCA_SCCLOADDLLFAILED = -24,

            [Description("Could not initialize SCC connection")]
            PBORCA_SCCINITFAILED = -25,

            [Description("Could not open SCC project")]
            PBORCA_OPENPROJFAILED = -26,

            [Description("Target File not found")]
            PBORCA_TARGETNOTFOUND = -27,

            [Description("Unable to read Target File")]
            PBORCA_TARGETREADERR = -28,

            [Description("Unable to access SCC interface")]
            PBORCA_GETINTERFACEERROR = -29,

            [Description("Scc connect offline requires IMPORTONLY refresh option")]
            PBORCA_IMPORTONLY_REQ = -30,

            [Description("SCC connect offline requires GetConnectProperties with Exclude_Checkout")]
            PBORCA_GETCONNECT_REQ = -31,

            [Description("SCC connect offline with Exclude_Checkout requires PBC file")]
            PBORCA_PBCFILE_REQ = -32,
        }

        /// <summary>
        /// Version.
        /// </summary>
        public enum Version
        {
            [DllName(PB105_DllName)]
            PB105 = 105,

            [DllName(PB125_DllName)]
            PB125 = 125,

            [DllName(PB170_DllName)]
            PB170 = 170,

            [DllName(PB190_DllName)]
            PB190 = 190
        }

        public const string PB105_DllName = "pborc105";
        public const string PB125_DllName = "pborc125";
        public const string PB170_DllName = "pborc170";
        public const string PB190_DllName = "pborc";

        #region private

        private enum PBORCA_ENTRY_TYPE
        {
            PBORCA_APPLICATION,
            PBORCA_DATAWINDOW,
            PBORCA_FUNCTION,
            PBORCA_MENU,
            PBORCA_QUERY,
            PBORCA_STRUCTURE,
            PBORCA_USEROBJECT,
            PBORCA_WINDOW,
            PBORCA_PIPELINE,
            PBORCA_PROJECT,
            PBORCA_PROXYOBJECT,
            PBORCA_BINARY
        }

        private static int session = 0;
        private string currentLibrary = null;
        private Version currentVersion;
        private List<LibEntry> libEntries;
        private Result result;

        #endregion private

        #region PB10.5

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_ApplicationRebuild", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_ApplicationRebuild105(
           int hORCASession,
           PBORCA_REBLD_TYPE eRebldType,
           [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pCompErrorProc,
           IntPtr pUserData);

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_CompileEntryRegenerate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_CompileEntryRegenerate105(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszLibraryName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszEntryName,
            PBORCA_ENTRY_TYPE otEntryType,
            [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pCompErrorProc,
            IntPtr pUserData);

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_DynamicLibraryCreate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_DynamicLibraryCreate105(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszPbrName,
            IntPtr lFlags,
            IntPtr pbcPara);

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_LibraryCreate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_LibraryCreate105(int hORCASession, [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName, [MarshalAs(UnmanagedType.LPTStr)] string lpszLibComment);

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_LibraryDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_LibraryDirectory105(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibComments,
            int iCmntsBufflen,
            [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pListProc,
            IntPtr pUserData
        );

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_LibraryEntryExport", CharSet = CharSet.Auto)]
        private static extern int PBORCA_LibraryEntryExport105(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszLibraryName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszEntryName,
            PBORCA_ENTRY_TYPE otEntryType,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszExportBuffer,
            System.Int32 lExportBufferSize);

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_SessionClose", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern void PBORCA_SessionClose105(int hORCASession);

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_SessionOpen", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionOpen105();

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_SessionSetCurrentAppl", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionSetCurrentAppl105(int hORCASession, [MarshalAs(UnmanagedType.LPTStr)] string pLibName, [MarshalAs(UnmanagedType.LPWStr)] string lpstApplName);

        [DllImport(PB105_DllName, EntryPoint = "PBORCA_SessionSetLibraryList", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionSetLibraryList105(int hORCASession, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] lpszLibraryName, System.Int32 iNumberOfLibs);

        #endregion PB10.5

        #region PB12.5

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_ApplicationRebuild", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_ApplicationRebuild125(
           int hORCASession,
           PBORCA_REBLD_TYPE eRebldType,
           [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pCompErrorProc,
           IntPtr pUserData);

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_CompileEntryRegenerate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_CompileEntryRegenerate125(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszLibraryName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszEntryName,
            PBORCA_ENTRY_TYPE otEntryType,
            [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pCompErrorProc,
            IntPtr pUserData);

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_DynamicLibraryCreate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_DynamicLibraryCreate125(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszPbrName,
            IntPtr lFlags,
            IntPtr pbcPara);

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_LibraryCreate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_LibraryCreate125(int hORCASession, [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName, [MarshalAs(UnmanagedType.LPTStr)] string lpszLibComment);

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_LibraryDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_LibraryDirectory125(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibComments,
            int iCmntsBufflen,
            [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pListProc,
            IntPtr pUserData
        );

        [DllImport(PB125_DllName, CharSet = CharSet.Auto)]
        private static extern int PBORCA_LibraryEntryExport125(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszLibraryName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszEntryName,
            PBORCA_ENTRY_TYPE otEntryType,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszExportBuffer,
            System.Int32 lExportBufferSize);

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_SessionClose", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern void PBORCA_SessionClose125(int hORCASession);

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_SessionOpen", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionOpen125();

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_SessionSetCurrentAppl", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionSetCurrentAppl125(int hORCASession, [MarshalAs(UnmanagedType.LPTStr)] string pLibNames, [MarshalAs(UnmanagedType.LPWStr)] string lpstApplName);

        [DllImport(PB125_DllName, EntryPoint = "PBORCA_SessionSetLibraryList", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionSetLibraryList125(int hORCASession, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] lpszLibraryName, System.Int32 iNumberOfLibs);

        #endregion PB12.5

        #region PB17.0

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_ApplicationRebuild", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_ApplicationRebuild170(
         int hORCASession,
         PBORCA_REBLD_TYPE eRebldType,
         [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pCompErrorProc,
         IntPtr pUserData);

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_CompileEntryRegenerate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_CompileEntryRegenerate170(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszLibraryName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszEntryName,
            PBORCA_ENTRY_TYPE otEntryType,
            [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pCompErrorProc,
            IntPtr pUserData);

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_DynamicLibraryCreate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_DynamicLibraryCreate170(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszPbrName,
            IntPtr lFlags,
            IntPtr pbcPara);

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_LibraryCreate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_LibraryCreate170(int hORCASession, [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName, [MarshalAs(UnmanagedType.LPTStr)] string lpszLibComment);

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_LibraryDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_LibraryDirectory170(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibComments,
            int iCmntsBufflen,
            [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pListProc,
            IntPtr pUserData
        );

        [DllImport(PB170_DllName, CharSet = CharSet.Auto, EntryPoint = "PBORCA_LibraryEntryExport")]
        private static extern int PBORCA_LibraryEntryExport170(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszLibraryName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszEntryName,
            PBORCA_ENTRY_TYPE otEntryType,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszExportBuffer,
            System.Int32 lExportBufferSize);

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_SessionClose", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern void PBORCA_SessionClose170(int hORCASession);

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_SessionOpen", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionOpen170();

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_SessionSetCurrentAppl", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionSetCurrentAppl170(int hORCASession, [MarshalAs(UnmanagedType.LPTStr)] string pLibNames, [MarshalAs(UnmanagedType.LPWStr)] string lpstApplName);

        [DllImport(PB170_DllName, EntryPoint = "PBORCA_SessionSetLibraryList", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionSetLibraryList170(int hORCASession, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] lpszLibraryName, System.Int32 iNumberOfLibs);

        #endregion PB17.0

        #region PB19.0

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_ApplicationRebuild", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_ApplicationRebuild190(
         int hORCASession,
         PBORCA_REBLD_TYPE eRebldType,
         [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pCompErrorProc,
         IntPtr pUserData);

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_CompileEntryRegenerate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_CompileEntryRegenerate190(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszLibraryName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszEntryName,
            PBORCA_ENTRY_TYPE otEntryType,
            [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pCompErrorProc,
            IntPtr pUserData);

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_DynamicLibraryCreate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_DynamicLibraryCreate190(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszPbrName,
            IntPtr lFlags,
            IntPtr pbcPara);

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_LibraryCreate", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_LibraryCreate190(int hORCASession, [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName, [MarshalAs(UnmanagedType.LPTStr)] string lpszLibComment);

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_LibraryDirectory", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int PBORCA_LibraryDirectory190(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibName,
            [MarshalAs(UnmanagedType.LPTStr)] string lpszLibComments,
            int iCmntsBufflen,
            [MarshalAs(UnmanagedType.FunctionPtr)] PBORCA_CALLBACK pListProc,
            IntPtr pUserData
        );

        [DllImport(PB190_DllName, CharSet = CharSet.Auto, EntryPoint = "PBORCA_LibraryEntryExport")]
        private static extern int PBORCA_LibraryEntryExport190(
            int hORCASession,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszLibraryName,
            [MarshalAs(UnmanagedType.LPWStr)] string lpszEntryName,
            PBORCA_ENTRY_TYPE otEntryType,
            [MarshalAs(UnmanagedType.LPWStr)] StringBuilder lpszExportBuffer,
            System.Int32 lExportBufferSize);

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_SessionClose", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern void PBORCA_SessionClose190(int hORCASession);

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_SessionOpen", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionOpen190();

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_SessionSetCurrentAppl", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionSetCurrentAppl190(int hORCASession, [MarshalAs(UnmanagedType.LPTStr)] string pLibNames, [MarshalAs(UnmanagedType.LPWStr)] string lpstApplName);

        [DllImport(PB190_DllName, EntryPoint = "PBORCA_SessionSetLibraryList", CharSet = CharSet.Unicode, SetLastError = true)]
        private static unsafe extern int PBORCA_SessionSetLibraryList190(int hORCASession, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.LPWStr)] string[] lpszLibraryName, System.Int32 iNumberOfLibs);

        #endregion PB19.0

        #region extern and unsafe

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct PBORCA_COMPERR
        {
            public int iLevel;                              /* Error level */

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string lpszMessageNumber;                /* Pointer to message number */

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string lpszMessageText;                  /* Pointer to message text */

            public int iColumnNumber;                       /* Column number */
            public int iLineNumber;                         /* Line number */
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        private struct PBORCA_DIRENTRY
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
            public string szComments;

            public int lCreateTime;
            public int lEntrySize;
            public string lpszEntryName;
            public PBORCA_ENTRY_TYPE otEntryType;
        }

        //allgemeine callback
        private delegate void PBORCA_CALLBACK(IntPtr pDirEntry, IntPtr lpUserData);

        private static void PBORCA_COMPERRCallback(IntPtr pDirEntry, IntPtr lpUserData)
        {
            PBORCA_COMPERR error = (PBORCA_COMPERR)Marshal.PtrToStructure(pDirEntry, typeof(PBORCA_COMPERR));
        }

        private void PBORCA_DIRENTRYCallback(IntPtr pDirEntry, IntPtr lpUserData)
        {
            PBORCA_DIRENTRY myDirEntry = (PBORCA_DIRENTRY)Marshal.PtrToStructure(pDirEntry, typeof(PBORCA_DIRENTRY));
            DateTime myDateTime = new DateTime(1970, 01, 01, 00, 00, 00).AddSeconds((double)myDirEntry.lCreateTime);

            libEntries.Add(new LibEntry(myDirEntry.lpszEntryName, GetObjecttype(myDirEntry.otEntryType), myDateTime, myDirEntry.lEntrySize, currentLibrary, this.currentVersion, myDirEntry.szComments));
        }

        #endregion extern and unsafe

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="version">PB Version to use.</param>
        /// <param name="version">Force to create new session.</param>
        public Orca(Version version, bool newSession = false)
        {
            this.currentVersion = version;

            if (session == 0 || newSession)
                SessionOpen();
        }

        public Result ApplicationRebuild(PBORCA_REBLD_TYPE rEBLD_TYPE)
        {
            PBORCA_CALLBACK staticCallbackDel = new PBORCA_CALLBACK(PBORCA_COMPERRCallback);
            IntPtr dummy = new IntPtr();

            switch (this.currentVersion)
            {
                case Version.PB105:
                    result = (Result)PBORCA_ApplicationRebuild105(session, rEBLD_TYPE, staticCallbackDel, dummy);
                    break;

                case Version.PB125:
                    result = (Result)PBORCA_ApplicationRebuild125(session, rEBLD_TYPE, staticCallbackDel, dummy);
                    break;

                case Version.PB170:
                    result = (Result)PBORCA_ApplicationRebuild170(session, rEBLD_TYPE, staticCallbackDel, dummy);
                    break;

                case Version.PB190:
                    result = (Result)PBORCA_ApplicationRebuild190(session, rEBLD_TYPE, staticCallbackDel, dummy);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Creates the dynamic library.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="pbrFile">The PBR file.</param>
        /// <returns></returns>
        public Result CreateDynamicLibrary(string file, string pbrFile)
        {
            switch (this.currentVersion)
            {
                case Version.PB105:
                    result = (Result)PBORCA_DynamicLibraryCreate105(session, file, pbrFile, IntPtr.Zero, IntPtr.Zero);
                    break;

                case Version.PB125:
                    result = (Result)PBORCA_DynamicLibraryCreate125(session, file, pbrFile, IntPtr.Zero, IntPtr.Zero);
                    break;

                case Version.PB170:
                    result = (Result)PBORCA_DynamicLibraryCreate170(session, file, pbrFile, IntPtr.Zero, IntPtr.Zero);
                    break;

                case Version.PB190:
                    result = (Result)PBORCA_DynamicLibraryCreate190(session, file, pbrFile, IntPtr.Zero, IntPtr.Zero);
                    break;
            }

            return result;
        }

        /// <summary>
        /// creates a new pbl
        /// </summary>
        /// <param name="file">path to new pbl</param>
        /// <param name="comment">comment for thew lib</param>
        public Result CreateLibrary(string file, string comment = "")
        {
            int orcaSession = 0;

            if (session == 0)
            {
                switch (this.currentVersion)
                {
                    case Version.PB105:
                        orcaSession = PBORCA_SessionOpen105();
                        break;

                    case Version.PB125:
                        orcaSession = PBORCA_SessionOpen125();
                        break;

                    case Version.PB170:
                        orcaSession = PBORCA_SessionOpen170();
                        break;

                    case Version.PB190:
                        orcaSession = PBORCA_SessionOpen190();
                        break;
                }
            }
            else
                orcaSession = session;

            switch (this.currentVersion)
            {
                case Version.PB105:
                    result = (Result)PBORCA_LibraryCreate105(orcaSession, file, comment);
                    break;

                case Version.PB125:
                    result = (Result)PBORCA_LibraryCreate125(orcaSession, file, comment);
                    break;

                case Version.PB170:
                    result = (Result)PBORCA_LibraryCreate170(orcaSession, file, comment);
                    break;

                case Version.PB190:
                    result = (Result)PBORCA_LibraryCreate190(orcaSession, file, comment);
                    break;
            }

            if (session == 0)
            {
                switch (this.currentVersion)
                {
                    case Version.PB105:
                        PBORCA_SessionClose105(orcaSession);
                        break;

                    case Version.PB125:
                        PBORCA_SessionClose125(orcaSession);
                        break;

                    case Version.PB170:
                        PBORCA_SessionClose170(orcaSession);
                        break;

                    case Version.PB190:
                        PBORCA_SessionClose190(orcaSession);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// lists a library
        /// </summary>
        /// <param name="file">path to librarys</param>
        /// <returns>list of entries</returns>
        public List<LibEntry> DirLibrary(string file)
        {
            int orcaSession = 0;
            PBORCA_CALLBACK PBORCA_LibraryDirectoryCallback = new PBORCA_CALLBACK(PBORCA_DIRENTRYCallback);
            IntPtr dummy = new IntPtr();

            currentLibrary = file;
            libEntries = new List<LibEntry>();

            if (session == 0)
            {
                switch (this.currentVersion)
                {
                    case Version.PB105:
                        orcaSession = PBORCA_SessionOpen105();
                        break;

                    case Version.PB125:
                        orcaSession = PBORCA_SessionOpen125();
                        break;

                    case Version.PB170:
                        orcaSession = PBORCA_SessionOpen170();
                        break;

                    case Version.PB190:
                        orcaSession = PBORCA_SessionOpen190();
                        break;
                }
            }
            else
                orcaSession = session;

            switch (this.currentVersion)
            {
                case Version.PB105:
                    PBORCA_LibraryDirectory105(orcaSession, file, "", 0, PBORCA_LibraryDirectoryCallback, dummy);
                    break;

                case Version.PB125:
                    PBORCA_LibraryDirectory125(orcaSession, file, "", 0, PBORCA_LibraryDirectoryCallback, dummy);
                    break;

                case Version.PB170:
                    PBORCA_LibraryDirectory170(orcaSession, file, "", 0, PBORCA_LibraryDirectoryCallback, dummy);
                    break;

                case Version.PB190:
                    PBORCA_LibraryDirectory190(orcaSession, file, "", 0, PBORCA_LibraryDirectoryCallback, dummy);
                    break;
            }

            if (session == 0)
            {
                switch (this.currentVersion)
                {
                    case Version.PB105:
                        PBORCA_SessionClose105(orcaSession);
                        break;

                    case Version.PB125:
                        PBORCA_SessionClose125(orcaSession);
                        break;

                    case Version.PB170:
                        PBORCA_SessionClose170(orcaSession);
                        break;

                    case Version.PB190:
                        PBORCA_SessionClose190(orcaSession);
                        break;
                }
            }

            return libEntries;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or
        /// resetting unmanaged resources.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Dispose()
        {
            if (session != 0)
                SessionClose();
        }

        /// <summary>
        /// reads the source of an object
        /// </summary>
        /// <param name="libEntry">library entry to export</param>
        public Result FillCode(LibEntry libEntry)
        {
            int orcaSession = 0;
            StringBuilder sbSource = new StringBuilder(5242880); // 5 MB

            if (session == 0)
            {
                switch (this.currentVersion)
                {
                    case Version.PB105:
                        orcaSession = PBORCA_SessionOpen105();
                        break;

                    case Version.PB125:
                        orcaSession = PBORCA_SessionOpen125();
                        break;

                    case Version.PB170:
                        orcaSession = PBORCA_SessionOpen170();
                        break;

                    case Version.PB190:
                        orcaSession = PBORCA_SessionOpen190();
                        break;
                }
            }
            else
                orcaSession = session;

            switch (this.currentVersion)
            {
                case Version.PB105:
                    result = (Result)PBORCA_LibraryEntryExport105(orcaSession, libEntry.Library, libEntry.Name, GetEntryType(libEntry.Type), sbSource, sbSource.Capacity);
                    break;

                case Version.PB125:
                    result = (Result)PBORCA_LibraryEntryExport125(orcaSession, libEntry.Library, libEntry.Name, GetEntryType(libEntry.Type), sbSource, sbSource.Capacity);
                    break;

                case Version.PB170:
                    result = (Result)PBORCA_LibraryEntryExport170(orcaSession, libEntry.Library, libEntry.Name, GetEntryType(libEntry.Type), sbSource, sbSource.Capacity);
                    break;

                case Version.PB190:
                    result = (Result)PBORCA_LibraryEntryExport190(orcaSession, libEntry.Library, libEntry.Name, GetEntryType(libEntry.Type), sbSource, sbSource.Capacity);
                    break;
            }

            libEntry.Source = sbSource.ToString();

            if (session == 0)
            {
                switch (this.currentVersion)
                {
                    case Version.PB105:
                        PBORCA_SessionClose105(orcaSession);
                        break;

                    case Version.PB125:
                        PBORCA_SessionClose125(orcaSession);
                        break;

                    case Version.PB170:
                        PBORCA_SessionClose170(orcaSession);
                        break;

                    case Version.PB190:
                        PBORCA_SessionClose190(orcaSession);
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Regenerates the object.
        /// </summary>
        /// <param name="library">The library.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <param name="objecttype">The objecttype.</param>
        /// <returns></returns>
        public Result RegenerateObject(string library, string objectName, Objecttype objecttype)
        {
            PBORCA_CALLBACK staticCallbackDel = new PBORCA_CALLBACK(PBORCA_COMPERRCallback);
            IntPtr dummy = new IntPtr();

            switch (this.currentVersion)
            {
                case Version.PB105:
                    result = (Result)PBORCA_CompileEntryRegenerate105(session, library, objectName, GetEntryType(objecttype), staticCallbackDel, dummy);
                    break;

                case Version.PB125:
                    result = (Result)PBORCA_CompileEntryRegenerate125(session, library, objectName, GetEntryType(objecttype), staticCallbackDel, dummy);
                    break;

                case Version.PB170:
                    result = (Result)PBORCA_CompileEntryRegenerate170(session, library, objectName, GetEntryType(objecttype), staticCallbackDel, dummy);
                    break;

                case Version.PB190:
                    result = (Result)PBORCA_CompileEntryRegenerate190(session, library, objectName, GetEntryType(objecttype), staticCallbackDel, dummy);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Sets the current appl.
        /// </summary>
        /// <param name="applLibName">Name of the appl library.</param>
        /// <param name="applName">Name of the appl.</param>
        /// <returns></returns>
        public Result SetCurrentAppl(string applLibName, string applName)
        {
            switch (this.currentVersion)
            {
                case Version.PB105:
                    result = (Result)PBORCA_SessionSetCurrentAppl105(session, applLibName, applName);
                    break;

                case Version.PB125:
                    result = (Result)PBORCA_SessionSetCurrentAppl125(session, applLibName, applName);
                    break;

                case Version.PB170:
                    result = (Result)PBORCA_SessionSetCurrentAppl170(session, applLibName, applName);
                    break;

                case Version.PB190:
                    result = (Result)PBORCA_SessionSetCurrentAppl190(session, applLibName, applName);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Sets the library list.
        /// </summary>
        /// <param name="librarys">The librarys.</param>
        /// <param name="libCount">The library count.</param>
        /// <returns></returns>
        public Result SetLibraryList(string[] librarys, int libCount)
        {
            switch (this.currentVersion)
            {
                case Version.PB105:
                    result = (Result)PBORCA_SessionSetLibraryList105(session, librarys, libCount);
                    break;

                case Version.PB125:
                    result = (Result)PBORCA_SessionSetLibraryList125(session, librarys, libCount);
                    break;

                case Version.PB170:
                    result = (Result)PBORCA_SessionSetLibraryList170(session, librarys, libCount);
                    break;

                case Version.PB190:
                    result = (Result)PBORCA_SessionSetLibraryList190(session, librarys, libCount);
                    break;
            }

            return result;
        }

        /// <summary>
        /// converts the Objecttype to PBORCA_ENTRY_TYPE
        /// </summary>
        /// <param name="type">Objecttype</param>
        /// <returns></returns>
        private PBORCA_ENTRY_TYPE GetEntryType(Objecttype type)
        {
            PBORCA_ENTRY_TYPE entryType = PBORCA_ENTRY_TYPE.PBORCA_BINARY;

            switch (type)
            {
                case Objecttype.Application:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_APPLICATION;
                    break;

                case Objecttype.Binary:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_BINARY;
                    break;

                case Objecttype.Datawindow:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_DATAWINDOW;
                    break;

                case Objecttype.Function:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_FUNCTION;
                    break;

                case Objecttype.Menu:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_MENU;
                    break;

                case Objecttype.Pipeline:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_PIPELINE;
                    break;

                case Objecttype.Project:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_PROJECT;
                    break;

                case Objecttype.Proxyobject:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_PROXYOBJECT;
                    break;

                case Objecttype.Query:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_QUERY;
                    break;

                case Objecttype.Structure:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_STRUCTURE;
                    break;

                case Objecttype.Userobject:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_USEROBJECT;
                    break;

                case Objecttype.Window:
                    entryType = PBORCA_ENTRY_TYPE.PBORCA_WINDOW;
                    break;
            }

            return entryType;
        }

        /// <summary>
        /// converts the PBORCA_ENTRY_TYPE to Objecttype
        /// </summary>
        /// <param name="entryType"></param>
        /// <returns></returns>
        private Objecttype GetObjecttype(PBORCA_ENTRY_TYPE entryType)
        {
            Objecttype type = Objecttype.None;

            switch (entryType)
            {
                case PBORCA_ENTRY_TYPE.PBORCA_APPLICATION:
                    type = Objecttype.Application;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_BINARY:
                    type = Objecttype.Binary;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_DATAWINDOW:
                    type = Objecttype.Datawindow;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_FUNCTION:
                    type = Objecttype.Function;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_MENU:
                    type = Objecttype.Menu;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_PIPELINE:
                    type = Objecttype.Pipeline;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_PROJECT:
                    type = Objecttype.Project;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_PROXYOBJECT:
                    type = Objecttype.Proxyobject;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_QUERY:
                    type = Objecttype.Query;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_STRUCTURE:
                    type = Objecttype.Structure;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_USEROBJECT:
                    type = Objecttype.Userobject;
                    break;

                case PBORCA_ENTRY_TYPE.PBORCA_WINDOW:
                    type = Objecttype.Window;
                    break;
            }

            return type;
        }

        /// <summary>
        /// Sessions the close.
        /// </summary>
        private void SessionClose()
        {
            switch (this.currentVersion)
            {
                case Version.PB105:
                    PBORCA_SessionClose105(session);
                    break;

                case Version.PB125:
                    PBORCA_SessionClose125(session);
                    break;

                case Version.PB170:
                    PBORCA_SessionClose170(session);
                    break;

                case Version.PB190:
                    PBORCA_SessionClose190(session);
                    break;
            }

            session = 0;
        }

        /// <summary>
        /// Sessions the open.
        /// </summary>
        private void SessionOpen()
        {
            switch (this.currentVersion)
            {
                case Version.PB105:
                    session = PBORCA_SessionOpen105();
                    break;

                case Version.PB125:
                    session = PBORCA_SessionOpen125();
                    break;

                case Version.PB170:
                    session = PBORCA_SessionOpen170();
                    break;

                case Version.PB190:
                    session = PBORCA_SessionOpen190();
                    break;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Orca" /> class.
        /// </summary>
        ~Orca()
        {
            Dispose();
        }
    }
}