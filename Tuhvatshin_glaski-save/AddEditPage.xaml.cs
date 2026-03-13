using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tuhvatshin_glaski_save
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private Agent _currentAgent = new Agent();
        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            if(SelectedAgent !=null)
                _currentAgent = SelectedAgent;
            DataContext = _currentAgent;
            DelBtn.Visibility = (SelectedAgent != null && SelectedAgent.ID != 0) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void ChangePicBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();
            if(myOpenFileDialog.ShowDialog() == true)
            {
                _currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if(string.IsNullOrWhiteSpace(_currentAgent.Title))
                errors.AppendLine("Укажите название агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО агента");
            if (ComboType.SelectedItem == null || ComboType.SelectedIndex == 0)
                errors.AppendLine("Укажите тип агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (_currentAgent.Priority < 0)
                errors.AppendLine("Приоритет агента должен быть целым неотрицательным числом");
            if (string.IsNullOrWhiteSpace(_currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(_currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");
            if (_currentAgent.Phone.Length < 11)
                errors.AppendLine("Укажите правильно телефон агента");
            else
            {
                string ph = _currentAgent.Phone.Replace("(", "").Replace(")", "").Replace("-", "").Replace("+", "").Replace(" ", "");
                if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11)
                    || (ph[1] == '3' && ph.Length != 12))
                    errors.AppendLine("Укажите правильно телефон агента");
            }
            if(string.IsNullOrWhiteSpace(_currentAgent.Email))
                errors.AppendLine("Укажите email агента");
            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }
            
            if (_currentAgent.ID == 0)
                 Tuhvatshin_glaskiEntities.GetContext().Agent.Add(_currentAgent);
            try
            {
                Tuhvatshin_glaskiEntities.GetContext().SaveChanges();
                MessageBox.Show("Информация сохранена");
                Manager.MainFrame.Navigate(new AgentPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
            }
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgent = Tuhvatshin_glaskiEntities.GetContext();
            if (_currentAgent.ProductSale != null && _currentAgent.ProductSale.Any())
            {
                MessageBox.Show("Нельзя удалить: есть информация о реализации продукции");
                return;
            }
            if (MessageBox.Show("вы хотите удалить агента?","", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes)
                return;
            if (_currentAgent.AgentPriorityHistory!=null)
            {
                foreach(var item in currentAgent.AgentPriorityHistory.ToList())
                {
                    Tuhvatshin_glaskiEntities.GetContext().AgentPriorityHistory.Remove(item);
                }
            }
            if(_currentAgent.Shop!=null)
            {
                foreach(var shop in _currentAgent.Shop.ToList())
                {
                    Tuhvatshin_glaskiEntities.GetContext().Shop.Remove(shop);
                }
            }
            currentAgent.Agent.Remove(_currentAgent);
            try
            {
                currentAgent.SaveChanges();
                MessageBox.Show("Агент удалён.");
                Manager.MainFrame.Navigate(new AgentPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при удалении: " + ex.Message);
            }
            
        }
    }
}
