using Mvvmdialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mvvmdialogs.Presenters
{
    public interface IDialogBoxPresenter<T> where T : IDialogViewModel
    {
        void Show(T viewModel);
    }
}
