using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

namespace LibraryManagementApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DatabaseHandler dbHandler;
        BindingList<Book> listBooks;

        public MainWindow()
        {
            InitializeComponent();
            listBooks = new BindingList<Book>();
            dbHandler = null;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            dbHandler = DatabaseHandler.getInstance();

            SqlCommand cmd = new SqlCommand("SP_GetAllBooks", dbHandler.connection);
            cmd.CommandType = CommandType.StoredProcedure;
            DataTable dt;

            try
            {
                dbHandler.connection.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                dt = new DataTable();
                dt.Load(dr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                return;
            }

            PopulateListViewBooks(dt);
            dbHandler.connection.Close();
        }

        private void PopulateListViewBooks(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Book book = new Book()
                {
                    ID = int.Parse(dt.Rows[i][0].ToString()),
                    Name = dt.Rows[i][1].ToString(),
                    Category = dt.Rows[i][2].ToString(),
                    Author = dt.Rows[i][3].ToString(),
                    Publisher = dt.Rows[i][4].ToString(),
                    YearPublished = int.Parse(dt.Rows[i][5].ToString()),
                    Price = int.Parse(dt.Rows[i][6].ToString()),
                };
                listBooks.Add(book);
            }
            listViewBooks.ItemsSource = listBooks;
        }

        /* Menu Items Actions*/
        private void FileExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.MessageBox.Show(this,
                "Nguyễn Khánh Hoàng - 1712457",
                "About Me");
        }

        private void ListViewBooks_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddBookWindow window = new AddBookWindow();
            window.Show();
        }
    }
}
