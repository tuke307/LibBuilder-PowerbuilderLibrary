using Data;
using LibBuilder.Business;
using MaterialDesignColors;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace LibBuilder.ViewModels
{
    public class AussehenViewModel : BaseViewModel
    {
        private ApplicationChanges color = new ApplicationChanges();

        public AussehenViewModel()
        {
            using (var db = new DatabaseContext())
                ToogleDarkmode = db.Settings.ToList().Last().DarkMode;

            Swatches = new SwatchesProvider().Swatches;
            ApplyPrimaryCommand = new ActionCommand(ApplyPrimary);
            ApplyAccentCommand = new ActionCommand(ApplyAccent);
        }

        public IEnumerable<Swatch> Swatches { get; }

        public ICommand ApplyPrimaryCommand { get; set; }
        public ICommand ApplyAccentCommand { get; set; }

        private void ApplyPrimary(object parameter)
        {
            color.SetPrimary(parameter);
        }

        private void ApplyAccent(object parameter)
        {
            color.SetAccent(parameter);
        }

        public bool ToogleDarkmode
        {
            get => Get<bool>();
            set
            {
                Set(value);
                color.SetBaseTheme(value);
            }
        }
    }
}