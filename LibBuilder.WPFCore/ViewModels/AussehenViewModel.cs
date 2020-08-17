// project=LibBuilder.WPFCore, file=AussehenViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
using LibBuilder.WPFCore.Business;
using MaterialDesignColors;
using System.Collections.Generic;

namespace LibBuilder.WPFCore.ViewModels
{
    public class AussehenViewModel : LibBuilder.Core.ViewModels.AussehenViewModel
    {
        private readonly ApplicationChanges color = new ApplicationChanges();

        private bool _toogleDarkmode;

        public IEnumerable<Swatch> Swatches { get; }

        public override bool ToogleDarkmode
        {
            get => _toogleDarkmode;
            set
            {
                SetProperty(ref _toogleDarkmode, value);
                color.SetBaseTheme(value);
            }
        }

        public AussehenViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;
        }

        protected override void ApplyAccent(object parameter)
        {
            color.SetAccent(parameter);
        }

        protected override void ApplyPrimary(object parameter)
        {
            color.SetPrimary(parameter);
        }
    }
}