// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.

using System;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;

namespace Microsoft.VisualStudio.Package
{

    /// <summary>
    ///This class provides some useful static shell based methods. 
    /// </summary>
    [CLSCompliant(false)]
    public static class UIHierarchyUtilities
    {
        /// <summary>
        /// Get reference to IVsUIHierarchyWindow interface from guid persistence slot.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="persistenceSlot">Unique identifier for a tool window created using IVsUIShell::CreateToolWindow. 
        /// The caller of this method can use predefined identifiers that map to tool windows if those tool windows 
        /// are known to the caller. </param>
        /// <returns>A reference to an IVsUIHierarchyWindow interface.</returns>
        public static IVsUIHierarchyWindow GetUIHierarchyWindow(IServiceProvider serviceProvider, Guid persistenceSlot)
        {
            if (serviceProvider == null)
            {
                throw new ArgumentNullException("serviceProvider");
            }

            IVsUIShell shell = serviceProvider.GetService(typeof(SVsUIShell)) as IVsUIShell;

            Debug.Assert(shell != null, "Could not get the ui shell from the project");
            if (shell == null)
            {
                throw new InvalidOperationException();
            }

            object pvar = null;
            IVsWindowFrame frame = null;
            IVsUIHierarchyWindow uiHierarchyWindow = null;

            try
            {
                if (0 == shell.FindToolWindow(0, ref persistenceSlot, out frame))
                {
                    ErrorHandler.ThrowOnFailure(frame.GetProperty((int)__VSFPROPID.VSFPROPID_DocView, out pvar));
                }
            }
            finally
            {
                if (pvar != null)
                {
                    uiHierarchyWindow = (IVsUIHierarchyWindow)pvar;
                }
            }

            return uiHierarchyWindow;
        }
    }
}
