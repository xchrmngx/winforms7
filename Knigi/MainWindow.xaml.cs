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
using Knigi.BD;
using Knigi.Pages;
using System.Reflection;
using word = Microsoft.Office.Interop.Word;

namespace Knigi
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : System.Windows.Window
    {
        book_Nechaev_KlimenkoEntities1 bd = new book_Nechaev_KlimenkoEntities1();
        int quant = 0;
        string Name = "";
        int cen = 0;
        BookAdd bookAdd;
        public MainWindow()
        {
            InitializeComponent();
            LViewDishes.ItemsSource = bd.books.ToList();
      
            bookAdd = new BookAdd(this);
        }

        private void Bok_Click(object sender, RoutedEventArgs e)
        {
            
            bookAdd.NameAt.ItemsSource = bd.autors.ToList();
            bookAdd.Janr.ItemsSource = bd.views.ToList();
            book book = new book();
            book = LViewDishes.SelectedItem as book;
            bookAdd.Addbook.Content = "Добавить книгу";
            bookAdd.Dobav.Content = "Добавить";
            bookAdd.Show();
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            quant++;
            Quantity.Text = quant.ToString();
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (Quantity.Text == "0")
            {
                Quantity.Text = "0";
            }
            else
            {
                quant--;
                Quantity.Text = quant.ToString();
            }
        }
        private void LViewDishes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (LViewDishes.SelectedItem is null) return;

            book book = new book();
            book = LViewDishes.SelectedItem as book;
            Info.Text = book.Описание.ToString();
            cen = Convert.ToInt32(book.Цена);
            Name = book.Название;
            Price.Content = "Цена " + book.Цена.ToString() + " р.";
            string put = Environment.CurrentDirectory.ToString();
            put = put.Remove(put.Length - 10, 10);

 
            string Put = $"{put}/Resource/{book.Изображение}";
            PhotoK.Source = new BitmapImage(new Uri(Put));

            

        }

        private void Oform_Click(object sender, RoutedEventArgs e)
        {

            int sum = quant * cen;
            word.Document document = null;
            word.Application app = new word.Application();
            string putword = Environment.CurrentDirectory.ToString() + @"\Документ.docx";
            document = app.Documents.Open(putword);
            document.Activate();
            word.Bookmarks bookm = document.Bookmarks;
            word.Range range;
            string[] data = new string[4] { DateTime.Now.ToString("dd.MM.yyyy HH:mm"), Name, quant.ToString(), sum.ToString() };
            int i = 0;
            foreach (word.Bookmark mark in bookm)
            {
                range = mark.Range;
                range.Text = data[i];
                i++;
            }
            document.Close();
            document = null;
            MessageBox.Show("Документ записан");
        }

        private void EditName_Click(object sender, RoutedEventArgs e)
        {
            Button btn = ((Button)sender);
            book book = new book();

            bookAdd.NameAt.ItemsSource = bd.autors.ToList();
            bookAdd.Janr.ItemsSource = bd.views.ToList();
            book = btn.DataContext as book;
            bookAdd.IDBok.Text = book.Номер.ToString(); 
            bookAdd.Addbook.Content = "Изменить книгу";
            bookAdd.Dobav.Content = "Изменить";
            bookAdd.NameAt.Text = book.autor.Автор.ToString();
            bookAdd.Janr.Text = book.view.Вид.ToString();
            bookAdd.NameB.Text = book.Название;
            bookAdd.InfoB.Text = book.Описание;
            bookAdd.PriceB.Text = book.Цена.ToString();
            string put = Environment.CurrentDirectory.ToString();
            put = put.Remove(put.Length - 10, 10);

  
            string Put = $"{put}/Resource/{book.Изображение}";
            bookAdd.PhotoB.Source = new BitmapImage(new Uri(Put));
            bookAdd.Show();


        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
