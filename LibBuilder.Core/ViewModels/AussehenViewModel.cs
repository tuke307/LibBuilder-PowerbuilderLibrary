// project=LibBuilder.Core, file=AussehenViewModel.cs, creation=2020:7:21 Copyright (c)
// 2020 Timeline Financials GmbH & Co. KG. All rights reserved.
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

        private bool _toogleDarkmode;

        public IMvxCommand ApplyAccentCommand { get; set; }

        public IMvxCommand ApplyPrimaryCommand { get; set; }

        public virtual bool ToogleDarkmode
        {
            get => _toogleDarkmode;
            set
            {
                SetProperty(ref _toogleDarkmode, value);
                //color.SetBaseTheme(value);
            }
        }

        public AussehenViewModel()
        {
            using (var db = new DatabaseContext())
                ToogleDarkmode = db.Settings.ToList().Last().DarkMode;

            //Swatches = new SwatchesProvider().Swatches;
            ApplyPrimaryCommand = new MvxCommand<object>(ApplyPrimary);
            ApplyAccentCommand = new MvxCommand<object>(ApplyAccent);
        }

        //public IEnumerable<Swatch> Swatches { get; }
        public override Task Initialize()
        {
            return base.Initialize();
        }

        public override void Prepare()
        {
            base.Prepare();
        }

        protected virtual void ApplyAccent(object parameter)
        {
            //color.SetAccent(parameter);
        }

        protected virtual void ApplyPrimary(object parameter)
        {
            //color.SetPrimary(parameter);
        }
    }
}