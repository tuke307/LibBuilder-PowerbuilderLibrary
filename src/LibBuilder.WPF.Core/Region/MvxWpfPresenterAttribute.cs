// project=LibBuilder.WPF.Core, file=MvxWpfPresenterAttribute.cs, create=09:16 Copyright
// (c) 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
using MvvmCross.Presenters.Attributes;
using MvvmCross.ViewModels;
using System;
using System.Linq;
using System.Windows;

namespace LibBuilder.WPF.Core.Region
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class MvxWpfPresenterAttribute : MvxBasePresentationAttribute
    {
        public string ContainerId { get; set; }

        public Func<object, string> ViewId { get; set; }

        public mvxViewPosition ViewPosition { get; set; }

        public MvxWpfPresenterAttribute(string containerId) : this(containerId, mvxViewPosition.New)
        {
        }

        public MvxWpfPresenterAttribute(string containerId, mvxViewPosition viewPosition)
        {
            ContainerId = containerId;
            ViewPosition = viewPosition;
            ViewId = DefaultViewId;
        }

        public MvxWpfPresenterAttribute(string containerId, mvxViewPosition viewPosition, string viewId) : this(containerId, viewPosition)
        {
            ViewId = (a) => viewId;
        }

        public MvxWpfPresenterAttribute(string containerId, mvxViewPosition viewPosition, Func<object, string> viewId) : this(containerId, viewPosition)
        {
            ViewId = viewId;
        }

        public static string DefaultViewId(object view) => view?.ToString();

        public static MvxWpfPresenterAttribute GetAttribute(FrameworkElement view, MvxViewModelRequest request)
        {
            if (view is MvvmCross.Presenters.IMvxOverridePresentationAttribute mvxView)
                if (mvxView.PresentationAttribute(request) is MvxWpfPresenterAttribute attr) return attr;
            return view.GetType().GetCustomAttributes(typeof(MvxWpfPresenterAttribute), true).FirstOrDefault() as MvxWpfPresenterAttribute;
        }

        public static string GetViewId(FrameworkElement view, MvxViewModelRequest request)
        {
            return GetAttribute(view, request)?.GetViewId(view);
        }

        public string GetViewId(FrameworkElement view)
        {
            if (view is MvvmCross.Views.IMvxView mvxView)
                return ViewId(mvxView?.ViewModel ?? mvxView?.DataContext ?? view?.DataContext);
            return ViewId(view?.DataContext);
        }
    }
}