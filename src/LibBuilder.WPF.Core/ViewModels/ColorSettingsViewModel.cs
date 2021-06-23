// project=LibBuilder.WPF.Core, file=ColorSettingsViewModel.cs, create=09:16 Copyright (c)
// 2021 Timeline Financials GmbH & Co. KG. All rights reserved.
namespace LibBuilder.WPF.Core.ViewModels
{
    using LibBuilder.WPF.Core.Business;
    using MaterialDesignColors;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using System.Collections.Generic;

    /// <summary>
    /// ColorSettingsViewModel.
    /// </summary>
    /// <seealso cref="MvxNavigationViewModel" />
    public class ColorSettingsViewModel : MvxNavigationViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ColorSettingsViewModel" /> class.
        /// </summary>
        /// <param name="logProvider">The log provider.</param>
        /// <param name="navigationService">The navigation service.</param>
        public ColorSettingsViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            Log.LogInformation("---START Initialize ColorSettingsViewModel---");

            //using (var db = new DatabaseContext())
            ToogleDarkmode = ApplicationChanges.IsDarkTheme();
            Swatches = new SwatchesProvider().Swatches;

            //Swatches = new SwatchesProvider().Swatches;
            ApplyPrimaryCommand = new MvxCommand<Swatch>(ApplyPrimary);
            ApplyAccentCommand = new MvxCommand<Swatch>(ApplyAccent);

            Log.LogInformation("---END Initialize ColorSettingsViewModel---");
        }

        protected void ApplyAccent(Swatch parameter)
        {
            ApplicationChanges.SetAccent(parameter);
        }

        protected void ApplyPrimary(Swatch parameter)
        {
            ApplicationChanges.SetPrimary(parameter);
        }

        #region Properties

        private bool _toogleDarkmode;

        /// <summary>
        /// Gets or sets the apply accent command.
        /// </summary>
        /// <value>The apply accent command.</value>
        public IMvxCommand ApplyAccentCommand { get; set; }

        /// <summary>
        /// Gets or sets the apply primary command.
        /// </summary>
        /// <value>The apply primary command.</value>
        public IMvxCommand ApplyPrimaryCommand { get; set; }

        /// <summary>
        /// Gets the swatches.
        /// </summary>
        /// <value>The swatches.</value>
        public IEnumerable<Swatch> Swatches { get; }

        /// <summary>
        /// Gets or sets a value indicating whether [toogle darkmode].
        /// </summary>
        /// <value><c>true</c> if [toogle darkmode]; otherwise, <c>false</c>.</value>
        public bool ToogleDarkmode
        {
            get => _toogleDarkmode;
            set
            {
                SetProperty(ref _toogleDarkmode, value);
                ApplicationChanges.SetBaseTheme(value);
            }
        }

        #endregion Properties
    }
}