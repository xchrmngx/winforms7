using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Knigi.BD;
using Microsoft.Win32;

namespace Knigi.Pages
{
    /// <summary>
    /// Логика взаимодействия для bookAdd.xaml
    /// </summary>
    public partial class BookAdd : Window
    {
        book_Nechaev_KlimenkoEntities1 bd = new book_Nechaev_KlimenkoEntities1();
        MainWindow main;
        public BookAdd(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
            
        }

        private void UploadB_Click(object sender, RoutedEventArgs e)
        {
            string txtEditor;
            string txtPhoto;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JPG Files|*.jpg";
            if (openFileDialog.ShowDialog() == true)
            {
                txtEditor = openFileDialog.FileName.Trim();
                txtPhoto = openFileDialog.SafeFileName;
                PhotoB.Source = new BitmapImage(new Uri(txtEditor));

                string dir = $"{Environment.CurrentDirectory}";
                string file = $"{dir.Remove(dir.Length - 10, 10)}\\Resource\\{txtPhoto}";

                if (File.Exists(file)) return;

                File.Copy(txtEditor, $"{dir.Remove(dir.Length - 10, 10)}\\Resource\\{txtPhoto}");

            }
        }

        private void Dobav_Click(object sender, RoutedEventArgs e)
        {
           
            book book = new book();
            if (Dobav.Content.ToString().Equals("Добавить"))
            {


                book.Номер = bd.books.ToList().Count + 1;
                autor autor = new autor();
                autor = NameAt.SelectedItem as autor;
                book.НомерАвтор = autor.НомерАвтора;

                view view = new view();
                view = Janr.SelectedItem as view;
                book.НомерВида = view.НомерВида;

                book.Название = NameB.Text;
                book.Описание = InfoB.Text;
                book.Цена = Convert.ToDouble(PriceB.Text);

               
                book.Изображение = System.IO.Path.GetFileName(((BitmapImage)PhotoB.Source).UriSource.ToString());

                bd.books.Add(book);
               

            }
            else if (Dobav.Content.ToString().Equals("Изменить"))
            {
                book.Номер = Convert.ToInt32(IDBok.Text);
                var uvar = bd.books.Where(w => w.Номер == book.Номер).FirstOrDefault();
                autor autor = new autor();
                autor = NameAt.SelectedItem as autor;
                uvar.НомерАвтор = autor.НомерАвтора;

                view view = new view();
                view = Janr.SelectedItem as view;
                uvar.НомерВида = view.НомерВида;

                uvar.Название = NameB.Text;
                uvar.Описание = InfoB.Text;
                uvar.Цена = Convert.ToDouble(PriceB.Text);

                uvar.Изображение = System.IO.Path.GetFileName(((BitmapImage)PhotoB.Source).UriSource.ToString());


            }
            bd.SaveChanges();


            main.LViewDishes.ItemsSource = bd.books.ToList();
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }
    }
}
