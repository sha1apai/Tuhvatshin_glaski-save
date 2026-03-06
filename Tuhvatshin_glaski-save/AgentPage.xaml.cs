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
        int CurrentPage = 1;
        int PageSize = 10;
        int TotalPages = 1;
        private List<int> Pages = new List<int>();
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
            if(TypeCombo.SelectedIndex != 0)
            {
                currentAgent = currentAgent.Where(p => p.AgentTypeID == TypeCombo.SelectedIndex).ToList();
            }
            switch (SortCombo.SelectedIndex)
            {
                case 1:
                    currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
                    break;
                case 2:
                    currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
                    break;
                case 3:
                    currentAgent = currentAgent.OrderBy(p => p.Discount).ToList();
                    break;
                case 4:
                    currentAgent = currentAgent.OrderByDescending(p => p.Discount).ToList();
                    break;
                case 5:
                    currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
                    break;
                case 6:
                    currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
                    break;
                    default:
                    break;
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
            int TotalItems = currentAgent.Count();
            TotalPages = Math.Max(1, (int)Math.Ceiling(TotalItems / (double)PageSize));
            Pages = Enumerable.Range(1, TotalPages).ToList();
            if (PageLB != null)
            {
                PageLB.ItemsSource = Pages;
                PageLB.SelectedIndex = CurrentPage - 1;
            }
            var pagedAgents = currentAgent.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();
            AgentListView.ItemsSource = pagedAgents;

            if (LeftButton != null)
            {
                if (CurrentPage > 1)
                    LeftButton.IsEnabled = true;
                else
                    LeftButton.IsEnabled = false;
            }
            if (RightButton != null)
            { 
                if(CurrentPage < TotalPages)
                    RightButton.IsEnabled = true;
                else
                    RightButton.IsEnabled = false;
            }
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

        private void LeftButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage > 1)
                CurrentPage--;
            Update();
        }

        private void RightButton_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPage < TotalPages)
                CurrentPage++;
            Update();
        }

        private void PageLB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && int.TryParse(e.AddedItems[0].ToString(), out int selectedPage))
            {
                if (selectedPage != CurrentPage)
                {
                    CurrentPage = selectedPage;
                    Update();
                }
            }
        }
    }
}
