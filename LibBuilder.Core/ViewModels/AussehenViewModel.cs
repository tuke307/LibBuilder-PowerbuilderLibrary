using Data;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace LibBuilder.Core.ViewModels
{
    public class AussehenViewModel : MvxViewModel
    {
        //private readonly ApplicationChanges color = new ApplicationChanges();

        public AussehenViewModel()
        {
            using (var db = new DatabaseContext())
                ToogleDarkmode = db.Settings.ToList().Last().DarkMode;

            //Swatches = new SwatchesProvider().Swatches;
            ApplyPrimaryCommand = new MvxCommand<object>(ApplyPrimary);
            ApplyAccentCommand = new MvxCommand<object>(ApplyAccent);
        }

        //public IEnumerable<Swatch> Swatches { get; }

        public IMvxCommand ApplyPrimaryCommand { get; set; }
        public IMvxCommand ApplyAccentCommand { get; set; }

        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        protected virtual void ApplyPrimary(object parameter)
        {
            //color.SetPrimary(parameter);
        }

        protected virtual void ApplyAccent(object parameter)
        {
            //color.SetAccent(parameter);
        }

        private bool _toogleDarkmode;

        public virtual bool ToogleDarkmode
        {
            get => _toogleDarkmode;
            set
            {
                SetProperty(ref _toogleDarkmode, value);
                //color.SetBaseTheme(value);
            }
        }
    }
}