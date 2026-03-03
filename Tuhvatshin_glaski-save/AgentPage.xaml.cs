using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.ComponentModel;
using System.Data.Entity;

namespace Tuhvatshin_glaski_save
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();
            var currentAgent = Tuhvatshin_glaskiEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentAgent;
            TypeCombo.SelectedIndex = 0;
            SortCombo.SelectedIndex = 0;
            Update();
        }
        private void Update()
        {
            //тип
            var currentAgent = Tuhvatshin_glaskiEntities.GetContext().Agent.ToList();
            if (TypeCombo.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "МФО").ToList();
            }
            else if (TypeCombo.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ООО").ToList();
            }
            else if (TypeCombo.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ЗАО").ToList();
            }
            else if (TypeCombo.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "МКК").ToList();
            }
            else if (TypeCombo.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ОАО").ToList();
            }
            else if (TypeCombo.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ПАО").ToList();
            }
            //сортировка
            if (SortCombo.SelectedIndex == 1)
            {
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            }
            else if (SortCombo.SelectedIndex == 2)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            }
            else if (SortCombo.SelectedIndex == 3)
            {
                currentAgent = currentAgent.OrderBy(p => p.Sales).ToList();
            }
            else if (SortCombo.SelectedIndex == 4)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Sales).ToList();
            }
            else if (SortCombo.SelectedIndex == 5)
            {
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
            }
            else if (SortCombo.SelectedIndex == 6)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
            }
            //проверка на ввод номера
            string CleanPhoneNumber(string phoneNumber)
            {
                return phoneNumber.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "");
            }
            //сам поиск 
            currentAgent = currentAgent.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower()) || CleanPhoneNumber(p.Phone).Contains(CleanPhoneNumber(TBoxSearch.Text)) ||
            p.Email.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();
            AgentListView.ItemsSource = currentAgent;
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void TypeCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }

        private void SortCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Update();
        }
    }
}
