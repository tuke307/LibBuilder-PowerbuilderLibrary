// project=LibBuilder.WPFCore, file=AussehenViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.WPFCore.Business;
using MaterialDesignColors;
using MvvmCross.Commands;
using MvvmCross.Logging;
using MvvmCross.Navigation;
using System.Collections.Generic;

namespace LibBuilder.WPFCore.ViewModels
{
    public class ColorSettingsViewModel : LibBuilder.Core.ViewModels.ColorSettingsViewModel
    {
        private bool _toogleDarkmode;

        public IMvxCommand ApplyAccentCommand { get; set; }

        public IMvxCommand ApplyPrimaryCommand { get; set; }

        public IEnumerable<Swatch> Swatches { get; }

        public bool ToogleDarkmode
        {
            get => _toogleDarkmode;
            set
            {
                SetProperty(ref _toogleDarkmode, value);
                ApplicationChanges.SetBaseTheme(value);
            }
        }

        public ColorSettingsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            //using (var db = new DatabaseContext())
            ToogleDarkmode = ApplicationChanges.IsDarkTheme();
            Swatches = new SwatchesProvider().Swatches;

            //Swatches = new SwatchesProvider().Swatches;
            ApplyPrimaryCommand = new MvxCommand<Swatch>(ApplyPrimary);
            ApplyAccentCommand = new MvxCommand<Swatch>(ApplyAccent);
        }

        protected void ApplyAccent(Swatch parameter)
        {
            ApplicationChanges.SetAccent(parameter);
        }

        protected void ApplyPrimary(Swatch parameter)
        {
            ApplicationChanges.SetPrimary(parameter);
        }
    }
}