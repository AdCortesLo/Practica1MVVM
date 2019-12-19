using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Input;

namespace Practica1MVVM.ViewModel
{
    public class DialogWindowViewModel : ViewModelBase, IUserDialogViewModel, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        public bool IsModal { get; private set; }
        public virtual void RequestClose()
        {
            if (this.OnCloseRequest != null)
            {
                this.OnCloseRequest(this);
            }
            Close();
        }
        public event EventHandler DialogClosing;

        public ICommand OkCommand { get { return new RelayCommand(Ok); } }
        protected virtual void Ok()
        {
            if (this.OnOk != null)
            {
                this.OnOk(this);
            }
            Close();
        }

        public ICommand CancelCommand { get { return new RelayCommand(Cancel); } }
        protected virtual void Cancel()
        {
            if (this.OnCancel != null)
            {
                this.OnCancel(this);
            }
            Close();
        }

        public Action<DialogWindowViewModel> OnOk { get; set; }
        public Action<DialogWindowViewModel> OnCancel { get; set; }
        public Action<DialogWindowViewModel> OnCloseRequest { get; set; }

        public DialogWindowViewModel()
        {
            IsModal = true;
        }

        public void Close()
        {
            if (this.DialogClosing != null)
                this.DialogClosing(this, new EventArgs());
        }
    }
}
