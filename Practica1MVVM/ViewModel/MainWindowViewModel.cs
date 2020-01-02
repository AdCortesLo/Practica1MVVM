using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MvvmDialogs.ViewModels;
using Practica1MVVM.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Practica1MVVM.ViewModel
{
    class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged 
    {
        Contactes2Entities context = new Contactes2Entities();
        public event PropertyChangedEventHandler PropertyChanged;
        public RelayCommand<string> BtsCommand { get; set; }

        public MainWindowViewModel()
        {
            BtsCommand = new RelayCommand<string>(BtsClick);
            EnableContactes();
            RbColumnaActual = "contactes";
            PopulateContactes();
            BtsClick("rbSenseContactes");
        }

        public void BtsClick(string rbName)
        {
            RbNameActual = rbName;
            switch (rbName)
            {

                case "HabilitarMulti":
                    IsMultiEnabled = true;
                    break;

                /* Buttons CRUD */

                case "afegirContactes":
                    contacte co = new contacte();
                    co = ContacteSelected;
                    context.contactes.Add(co);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { }
                    BtsClick("rbContactes");
                    PopulateEmailsTelefons();
                    break;

                case "modificarContactes":
                    OnNewModalDialog();
                    if (respuesta == 1) { 
                        contacte c = context.contactes.Where(x => x.contacteId == ContacteSelected.contacteId).FirstOrDefault();
                    try
                    {
                        c = ContacteSelected;
                    }
                    catch (Exception ex)
                    {
                    }
                    context.SaveChanges();
                    //BtsClick("rbContactes");
                    PopulateEmailsTelefons();
                    }
                    break;

                case "esborrarContactes":
                    OnNewModalDialog();
                    if (respuesta == 1)
                    {
                        try
                        {
                            context.contactes.Remove(ContacteSelected);
                        }
                        catch (Exception ex) { }
                        context.SaveChanges();
                        PopulateEmailsTelefons();
                        BtsClick("rbContactes"); 
                    }
                    break;

                case "DuplicarContactes":
                    OnNewModalDialog();
                    if (respuesta == 1)
                    {
                        contacte con = new contacte();
                        con.nom = ContacteSelected.nom;
                        con.cognoms = ContacteSelected.cognoms;
                        context.contactes.Add(con);
                        context.SaveChanges();

                        List<telefon> telefons = ContacteSelected.telefons.ToList();
                        List<email> emails = ContacteSelected.emails.ToList();

                        foreach (telefon item in telefons)
                        {
                            telefon tel = new telefon();
                            tel.contacteId = con.contacteId;
                            tel.telefon1 = item.telefon1;
                            tel.tipus = item.tipus;
                            con.telefons.Add(tel);
                        }
                        
                        foreach (email item in emails)
                        {
                            email ema = new email();
                            ema.contacteId = con.contacteId;
                            ema.email1 = item.email1;
                            ema.tipus = item.tipus;
                            con.emails.Add(ema);
                        }
                        
                        try
                        {
                            context.SaveChanges();
                        }
                        catch (Exception){}
                        PopulateEmailsTelefons();
                        BtsClick("rbContactes");
                    }
                    break;

                case "afegirTelefons":
                    telefon t = new telefon();
                    t.tipus = TipusT;
                    t.telefon1 = Telefon1;

                    ContacteSelected.telefons.Add(t);
                    //context.telefons.Add(t);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { }
                    PopulateEmailsTelefons();
                    //BtsClick("rbTelefons");
                    break;

                case "modificarTelefons":
                    OnNewModalDialog();
                    if (respuesta == 1)
                    {
                        telefon te = context.telefons.Where(x => x.contacteId == TelefonSelected.contacteId).FirstOrDefault();
                        try
                        {
                            te.telefon1 = Telefon1;
                            te.tipus = TipusT;
                        }
                        catch (Exception ex)
                        {
                        }
                        context.SaveChanges();
                        PopulateEmailsTelefons(); 
                    }
                    //BtsClick("rbTelefons");
                    break;

                case "esborrarTelefons":
                    OnNewModalDialog();
                    if (respuesta == 1)
                    {
                        try
                        {
                            context.telefons.Remove(TelefonSelected);
                        }
                        catch (Exception ex) { }
                        context.SaveChanges();
                        PopulateEmailsTelefons(); 
                    }
                    //BtsClick("rbTelefons");
                    break;

                case "afegirEmails":
                    email e = new email();
                    e.tipus = TipusE;
                    e.email1 = Email1;

                    ContacteSelected.emails.Add(e);
                    try
                    {
                        context.SaveChanges();
                    }
                    catch (Exception ex) { }
                    //BtsClick("rbEmails");
                    PopulateEmailsTelefons();
                    break;

                case "modificarEmails":
                    OnNewModalDialog();
                    if (respuesta == 1)
                    {
                        email em = context.emails.Where(x => x.contacteId == EmailSelected.contacteId).FirstOrDefault();
                        try
                        {
                            em.email1 = Email1;
                            em.tipus = TipusE;
                        }
                        catch (Exception ex)
                        {
                        }
                        context.SaveChanges();
                        PopulateEmailsTelefons(); 
                    }
                    //BtsClick("rbEmails");
                    break;

                case "esborrarEmails":
                    OnNewModalDialog();
                    if (respuesta == 1)
                    {
                        try
                        {
                            context.emails.Remove(EmailSelected);
                        }
                        catch (Exception ex)
                        {
                        }
                        context.SaveChanges();
                        PopulateEmailsTelefons(); 
                    }
                    //BtsClick("rbEmails");
                    break;


                /* Radio Buttons Columnas */

                case "rbContactes":
                    BtsClick("rbSenseContactes");
                    EnableContactes();
                    RbColumnaActual = "contactes";
                    Contactes = context.contactes.ToList().OrderBy(x => x.cognoms).ThenBy(x => x.nom).ToList();
                    ContacteSelected = Contactes.FirstOrDefault();
                    break;

                case "rbTelefons":
                    BtsClick("rbSenseTelefons");
                    EnableTelefons();
                    RbColumnaActual = "telefons";
                    Telefons = context.telefons.OrderBy(x => x.tipus).ToList();
                    TelefonSelected = Telefons.FirstOrDefault();
                    break;

                case "rbEmails":
                    BtsClick("rbSenseEmails");
                    EnableEmails();
                    RbColumnaActual = "emails";
                    Emails = context.emails.ToList().OrderBy(x => x.email1).ToList();
                    EmailSelected = Emails.FirstOrDefault();
                    break;


                /* Contactes */

                case "rbConteContacte":
                    Contactes = context.contactes.Where(x => x.nom.Contains(TextBoxContactes) || x.cognoms.Contains(TextBoxContactes)).OrderBy(x => x.cognoms).ThenBy(x => x.nom).ToList();
                    ContacteSelected = Contactes.FirstOrDefault();
                    EnableContactes();
                    RbColumnaActual = "contactes";
                    break;

                case "rbComençaContacte":
                    Contactes = context.contactes.Where(x => x.nom.StartsWith(TextBoxContactes) || x.cognoms.StartsWith(TextBoxContactes)).OrderBy(x => x.cognoms).ThenBy(x => x.nom).ToList();
                    ContacteSelected = Contactes.FirstOrDefault();
                    EnableContactes();
                    RbColumnaActual = "contactes";
                    break;

                case "rbAcabaContacte":
                    Contactes = context.contactes.Where(x => x.nom.EndsWith(TextBoxContactes) || x.cognoms.EndsWith(TextBoxContactes)).OrderBy(x => x.cognoms).ThenBy(x => x.nom).ToList();
                    ContacteSelected = Contactes.FirstOrDefault();
                    EnableContactes();
                    RbColumnaActual = "contactes";
                    break;

                case "rbSenseContacte":
                    BtsClick("rbSenseContactes");
                    EnableContactes();
                    RbColumnaActual = "contactes";
                    Contactes = context.contactes.ToList().OrderBy(x => x.cognoms).ThenBy(x => x.nom).ToList();
                    ContacteSelected = Contactes.FirstOrDefault();
                    break;


                /* Telèfons */

                case "rbConteTelefon":
                    Telefons = context.telefons.Where(x => x.telefon1.Contains(TextBoxTelefons) || x.tipus.Contains(TextBoxTelefons)).OrderBy(x => x.tipus).ToList();
                    TelefonSelected = Telefons.FirstOrDefault();
                    EnableTelefons();
                    RbColumnaActual = "telefons";
                    break;

                case "rbComençaTelefon":
                    Telefons = context.telefons.Where(x => x.telefon1.StartsWith(TextBoxTelefons) || x.tipus.StartsWith(TextBoxTelefons)).OrderBy(x => x.tipus).ToList();
                    TelefonSelected = Telefons.FirstOrDefault();
                    EnableTelefons();
                    RbColumnaActual = "telefons";
                    break;

                case "rbAcabaTelefon":
                    Telefons = context.telefons.Where(x => x.telefon1.EndsWith(TextBoxTelefons) || x.tipus.EndsWith(TextBoxTelefons)).OrderBy(x => x.tipus).ToList();
                    TelefonSelected = Telefons.FirstOrDefault();
                    EnableTelefons();
                    RbColumnaActual = "telefons";
                    break;

                case "rbSenseTelefon":
                    BtsClick("rbSenseTelefons");
                    EnableTelefons();
                    RbColumnaActual = "telefons";
                    Telefons = context.telefons.OrderBy(x => x.tipus).ToList();
                    TelefonSelected = Telefons.FirstOrDefault();
                    break;


                /* Emails */

                case "rbConteEmail":
                    Emails = context.emails.Where(x => x.email1.Contains(TextBoxEmails) || x.tipus.Contains(TextBoxEmails)).OrderBy(x => x.email1).ToList();
                    EmailSelected = Emails.FirstOrDefault();
                    EnableEmails();
                    RbColumnaActual = "emails";
                    break;

                case "rbComençaEmail":
                    Emails = context.emails.Where(x => x.email1.StartsWith(TextBoxEmails) || x.tipus.StartsWith(TextBoxEmails)).OrderBy(x => x.email1).ToList();
                    EmailSelected = Emails.FirstOrDefault();
                    EnableEmails();
                    RbColumnaActual = "emails";
                    break;

                case "rbAcabaEmail":
                    Emails = context.emails.Where(x => x.email1.EndsWith(TextBoxEmails) || x.tipus.EndsWith(TextBoxEmails)).OrderBy(x => x.email1).ToList();
                    EmailSelected = Emails.FirstOrDefault();
                    EnableEmails();
                    RbColumnaActual = "emails";
                    break;

                case "rbSenseEmail":
                    BtsClick("rbSenseEmails");
                    EnableEmails();
                    RbColumnaActual = "emails";
                    Emails = context.emails.ToList().OrderBy(x => x.email1).ToList();
                    EmailSelected = Emails.FirstOrDefault();
                    break;
            }
        }

        private void EnableContactes()
        {
            IsContactesEnabled = "#FFE0D693";
            IsEmailsEnabled = "White";
            IsTelefonsEnabled = "White";
            IsRbContactesChecked = true;
        }

        private void EnableTelefons()
        {
            IsContactesEnabled = "White";
            IsEmailsEnabled = "White";
            IsTelefonsEnabled = "#FFE0D693";
            IsRbTelefonsChecked = true;
        }

        private void EnableEmails()
        {
            IsContactesEnabled = "White";
            IsEmailsEnabled = "#FFE0D693";
            IsTelefonsEnabled = "White";
            IsRbEmailsChecked = true;
        }

        private string _rbNameActual;
        public string RbNameActual
        {
            get { return _rbNameActual; }
            set
            {
                _rbNameActual = value;
                NotifyPropertyChanged();
            }
        }

        private string _rbColumnaActual;
        public string RbColumnaActual
        {
            get { return _rbColumnaActual; }
            set
            {
                _rbColumnaActual = value;
                NotifyPropertyChanged();
            }
        }

        private string _textBoxContactes;
        public string TextBoxContactes
        {
            get { return _textBoxContactes; }
            set
            {
                _textBoxContactes = value;
                BtsClick(RbNameActual);
                NotifyPropertyChanged();
            }
        }

        private string _textBoxTelefons;
        public string TextBoxTelefons
        {
            get { return _textBoxTelefons; }
            set
            {
                _textBoxTelefons = value;
                BtsClick(RbNameActual);
                NotifyPropertyChanged();
            }
        }

        private string _textBoxEmails;
        public string TextBoxEmails
        {
            get { return _textBoxEmails; }
            set
            {
                _textBoxEmails = value;
                BtsClick(RbNameActual);
                NotifyPropertyChanged();
            }
        }

        private contacte _contacteSelected;
        public contacte ContacteSelected
        {
            get
            {
                return _contacteSelected;
            }
            set
            {
                _contacteSelected = value;
                PopulateEmailsTelefons();
                NotifyPropertyChanged();
            }
        }

        private telefon _telefonSelected;
        public telefon TelefonSelected
        {
            get
            {
                return _telefonSelected;
            }
            set
            {
                _telefonSelected = value;
                Telefon1 ="";
                TipusT = "";

                if (RbColumnaActual != null && TelefonSelected != null && RbColumnaActual.Equals("telefons"))
                {
                    Contactes = context.contactes.ToList().Where(x => x.contacteId == TelefonSelected.contacteId).ToList();
                    ContacteSelected = Contactes.FirstOrDefault();
                    Emails = context.emails.ToList().Where(x => x.contacteId == TelefonSelected.contacteId).ToList();
                    EmailSelected = Emails.FirstOrDefault();
                }
                if (TelefonSelected != null)
                {
                    Telefon1 = TelefonSelected.telefon1;
                    TipusT = TelefonSelected.tipus;
                }
                NotifyPropertyChanged();
            }
        }

        private string _telefon1;
        public string Telefon1
        {
            get { return _telefon1; }
            set
            {
                _telefon1 = value;
                NotifyPropertyChanged();
            }
        }

        private string _tipusT;
        public string TipusT
        {
            get { return _tipusT; }
            set
            {
                _tipusT = value;
                NotifyPropertyChanged();
            }
        }

        private string _email1;
        public string Email1
        {
            get { return _email1; }
            set
            {
                _email1 = value;
                NotifyPropertyChanged();
            }
        }

        private string _tipusE;
        public string TipusE
        {
            get { return _tipusE; }
            set
            {
                _tipusE = value;
                NotifyPropertyChanged();
            }
        }

        private email _emailSelected;
        public email EmailSelected
        {
            get
            {
                return _emailSelected;
            }
            set
            {
                _emailSelected = value;
                Email1 = "";
                TipusE = "";
                if (RbColumnaActual != null && EmailSelected != null && RbColumnaActual.Equals("emails"))
                {
                    Contactes = context.contactes.ToList().Where(x => x.contacteId == EmailSelected.contacteId).ToList();
                    ContacteSelected = Contactes.FirstOrDefault();
                    Telefons = context.telefons.ToList().Where(x => x.contacteId == EmailSelected.contacteId).ToList();
                    TelefonSelected = Telefons.FirstOrDefault();
                }
                if (EmailSelected != null)
                {
                    Email1 = EmailSelected.email1;
                    TipusE = EmailSelected.tipus;
                }
                NotifyPropertyChanged();
            }
        }

        private List<contacte> _contactes;
        public List<contacte> Contactes
        {
            get
            {
                return _contactes;
            }
            set
            {
                _contactes = value;
                NotifyPropertyChanged();
            }
        }

        private List<telefon> _telefons;
        public List<telefon> Telefons
        {
            get
            {
                return _telefons;
            }
            set
            {
                _telefons = value;
                NotifyPropertyChanged();
            }
        }

        private List<email> _emails;
        public List<email> Emails
        {
            get
            {
                return _emails;
            }
            set
            {
                _emails = value;
                NotifyPropertyChanged();
            }
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set
            {
                _selectedIndex = value;
                NotifyPropertyChanged();
            }
        }

        private string _isContactesEnabled;
        public string IsContactesEnabled
        {
            get { return _isContactesEnabled; }
            set
            {
                _isContactesEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private string _isTelefonsEnabled;
        public string IsTelefonsEnabled
        {
            get { return _isTelefonsEnabled; }
            set
            {
                _isTelefonsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private string _isEmailsEnabled;
        public string IsEmailsEnabled
        {
            get { return _isEmailsEnabled; }
            set
            {
                _isEmailsEnabled = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isRbContactesChecked;
        public bool IsRbContactesChecked
        {
            get { return _isRbContactesChecked; }
            set
            {
                _isRbContactesChecked = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isRbTelefonsChecked;
        public bool IsRbTelefonsChecked
        {
            get { return _isRbTelefonsChecked; }
            set
            {
                _isRbTelefonsChecked = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isRbEmailsChecked;
        public bool IsRbEmailsChecked
        {
            get { return _isRbEmailsChecked; }
            set
            {
                _isRbEmailsChecked = value;
                NotifyPropertyChanged();
            }
        }


        private bool _isMultiEnabled;
        public bool IsMultiEnabled
        {
            get { return _isMultiEnabled; }
            set
            {
                _isMultiEnabled = value;
                NotifyPropertyChanged();
            }
        }

        public void PopulateContactes()
        {
            Contactes = context.contactes.ToList().OrderBy(x => x.cognoms).ThenBy(x => x.nom).ToList();
            ContacteSelected = Contactes.FirstOrDefault();
        }

        public void PopulateEmailsTelefons()
        {
            if (ContacteSelected != null && RbColumnaActual.Equals("contactes"))
            {
                Telefons = context.telefons.ToList().Where(x => x.contacteId == ContacteSelected.contacteId).OrderBy(x => x.telefon1).ToList();
                Emails = context.emails.ToList().Where(x => x.contacteId == ContacteSelected.contacteId).OrderBy(x => x.email1).ToList();
                TelefonSelected = Telefons.Where(x => x.contacteId == ContacteSelected.contacteId).FirstOrDefault();               
                EmailSelected = Emails.ToList().Where(x => x.contacteId == ContacteSelected.contacteId).OrderBy(x => x.email1).FirstOrDefault();
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private int respuesta { get; set; }

        private ObservableCollection<IDialogViewModel> _dialogs = new ObservableCollection<IDialogViewModel>();
        public ObservableCollection<IDialogViewModel> Dialogs { get { return _dialogs; } }

        public ICommand ModalCommand
        {
            get
            {
                return new RelayCommand(OnNewModalDialog);
            }
        }
        public void OnNewModalDialog()
        {
            this.Dialogs.Add(new DialogWindowViewModel
            { // inicialitzem les variables del diàleg a mostrar
                OnOk = (sender) =>  //quan acaba el diàleg amb Ok
                {
                    respuesta = 1;
                },

                OnCancel = (sender) => //quan acaba el diàleg amb Cancel
                {
                    respuesta = 0;
                },

                OnCloseRequest = (sender) => //quan acaba el diàleg amb Close (tancant la finestra)
                {
                    respuesta = 0;
                }
            });
        } 
    }
}

