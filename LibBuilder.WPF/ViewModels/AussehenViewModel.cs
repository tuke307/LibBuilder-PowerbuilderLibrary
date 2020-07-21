using LibBuilder.WPF.Business;
using MaterialDesignColors;
using System.Collections.Generic;

namespace LibBuilder.WPF.ViewModels
{
    public class AussehenViewModel : LibBuilder.Core.ViewModels.AussehenViewModel
    {
        private readonly ApplicationChanges color = new ApplicationChanges();

        public AussehenViewModel()
        {
            Swatches = new SwatchesProvider().Swatches;
        }

        public IEnumerable<Swatch> Swatches { get; }

        protected override void ApplyPrimary(object parameter)
        {
            color.SetPrimary(parameter);
        }

        protected override void ApplyAccent(object parameter)
        {
            color.SetAccent(parameter);
        }

        private bool _toogleDarkmode;

        public override bool ToogleDarkmode
        {
            get => _toogleDarkmode;
            set
            {
                SetProperty(ref _toogleDarkmode, value);
                color.SetBaseTheme(value);
            }
        }
    }
}